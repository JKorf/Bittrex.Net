using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ClientWebSocketOptions2
{
    public int ReceiveBufferSize { get; set; }
    public int SendBufferSize { get; set; }
    public TimeSpan KeepAliveInterval { get; set; }
    public Dictionary<String, String> RequestHeaders { get; private set; }
    public List<String> RequestedSubProtocols { get; private set; }
    public bool UseDefaultCredentials { get; set; }
    public ICredentials Credentials { get; set; }
    public X509CertificateCollection InternalClientCertificates { get; set; }
    public IWebProxy Proxy { get; set; }
    public CookieContainer Cookies { get; set; }

    private ArraySegment<byte>? buffer;

    public ClientWebSocketOptions2()
    {
        ReceiveBufferSize = 16384;
        SendBufferSize = 16384;
        KeepAliveInterval = WebSocket.DefaultKeepAliveInterval;
        RequestedSubProtocols = new List<string>();
        RequestHeaders = new Dictionary<string, string>();
        this.Proxy = WebRequest.DefaultWebProxy;
    }

    public ArraySegment<byte> GetOrCreateBuffer()
    {
        if (!this.buffer.HasValue)
        {
            this.buffer = new ArraySegment<byte>?(WebSocket.CreateClientBuffer(this.ReceiveBufferSize, this.SendBufferSize));
        }
        return this.buffer.Value;
    }

    internal static string GetSecWebSocketAcceptString(string secWebSocketKey)
    {
        string result;
        using (SHA1 sHA = SHA1.Create())
        {
            string s = secWebSocketKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            result = Convert.ToBase64String(sHA.ComputeHash(bytes));
        }
        return result;
    }

}

public sealed class CustomWebsocket : WebSocket
{
    private readonly ClientWebSocketOptions2 options;
    private WebSocket innerWebSocket;
    private readonly CancellationTokenSource cts;
    private int state;
    private const int created = 0;
    private const int connecting = 1;
    private const int connected = 2;
    private const int disposed = 3;
    public ClientWebSocketOptions2 Options
    {
        get
        {
            return this.options;
        }
    }
    public override WebSocketCloseStatus? CloseStatus
    {
        get
        {
            if (this.innerWebSocket != null)
            {
                return this.innerWebSocket.CloseStatus;
            }
            return null;
        }
    }
    public override string CloseStatusDescription
    {
        get
        {
            if (this.innerWebSocket != null)
            {
                return this.innerWebSocket.CloseStatusDescription;
            }
            return null;
        }
    }
    public override string SubProtocol
    {
        get
        {
            if (this.innerWebSocket != null)
            {
                return this.innerWebSocket.SubProtocol;
            }
            return null;
        }
    }
    public override WebSocketState State
    {
        get
        {
            if (this.innerWebSocket != null)
            {
                return this.innerWebSocket.State;
            }
            switch (this.state)
            {
                case 0:
                    return WebSocketState.None;
                case 1:
                    return WebSocketState.Connecting;
                case 3:
                    return WebSocketState.Closed;
            }
            return WebSocketState.Closed;
        }
    }
    public CustomWebsocket()
    {
        this.state = 0;
        this.options = new ClientWebSocketOptions2();
        this.cts = new CancellationTokenSource();
    }
    public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
    {
        if (uri == null)
        {
            throw new ArgumentNullException("uri");
        }
        if (!uri.IsAbsoluteUri)
        {
            throw new ArgumentException("net_uri_NotAbsolute");
        }
        if (String.IsNullOrWhiteSpace(uri.Scheme) || (!uri.Scheme.Equals("ws", StringComparison.OrdinalIgnoreCase) && !uri.Scheme.Equals("wss", StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("net_WebSockets_Scheme");
        }
        int num = Interlocked.CompareExchange(ref this.state, 1, 0);
        if (num == 3)
        {
            throw new ObjectDisposedException(base.GetType().FullName);
        }
        if (num != 0)
        {
            throw new InvalidOperationException("net_WebSockets_AlreadyStarted");
        }
        return this.ConnectAsyncCore(uri, cancellationToken);
    }
    public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        this.ThrowIfNotConnected();
        return this.innerWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }
    public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        this.ThrowIfNotConnected();
        return this.innerWebSocket.ReceiveAsync(buffer, cancellationToken);
    }
    public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        this.ThrowIfNotConnected();
        return this.innerWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }
    public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        this.ThrowIfNotConnected();
        return this.innerWebSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
    }
    public override void Abort()
    {
        if (this.state == 3)
        {
            return;
        }
        if (this.innerWebSocket != null)
        {
            this.innerWebSocket.Abort();
        }
        this.Dispose();
    }
    public override void Dispose()
    {
        int num = Interlocked.Exchange(ref this.state, 3);
        if (num == 3)
        {
            return;
        }
        this.cts.Cancel(false);
        this.cts.Dispose();
        if (this.innerWebSocket != null)
        {
            this.innerWebSocket.Dispose();
        }
    }
    static CustomWebsocket()
    {
        WebSocket.RegisterPrefixes();
    }
    private async Task ConnectAsyncCore(Uri uri, CancellationToken cancellationToken)
    {
        HttpWebResponse httpWebResponse = null;
        CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
        try
        {
            HttpWebRequest httpWebRequest = this.CreateAndConfigureRequest(uri);

            cancellationTokenRegistration = cancellationToken.Register(new Action<object>(this.AbortRequest), httpWebRequest, false);
            httpWebResponse = ((await httpWebRequest.GetResponseAsync().ConfigureAwait(false)) as HttpWebResponse);

            string subProtocol = this.ValidateResponse(httpWebRequest, httpWebResponse);
            this.innerWebSocket = WebSocket.CreateClientWebSocket(httpWebResponse.GetResponseStream(), subProtocol, this.options.ReceiveBufferSize, this.options.SendBufferSize, this.options.KeepAliveInterval, false, this.options.GetOrCreateBuffer());

            if (Interlocked.CompareExchange(ref this.state, 2, 1) != 1)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
        }
        catch (WebException innerException)
        {
            this.ConnectExceptionCleanup(httpWebResponse);
            WebSocketException ex = new WebSocketException("net_webstatus_ConnectFailure", innerException);
            throw ex;
        }
        catch (Exception)
        {
            this.ConnectExceptionCleanup(httpWebResponse);
            throw;
        }
        finally
        {
            cancellationTokenRegistration.Dispose();
        }
    }
    private void ConnectExceptionCleanup(HttpWebResponse response)
    {
        this.Dispose();
        if (response != null)
        {
            response.Dispose();
        }
    }
    private HttpWebRequest CreateAndConfigureRequest(Uri uri)
    {
        HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
        if (httpWebRequest == null)
        {
            throw new InvalidOperationException("net_WebSockets_InvalidRegistration");
        }
        foreach (string name in this.options.RequestHeaders.Keys)
        {
            if (name.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
            {
                httpWebRequest.UserAgent = this.options.RequestHeaders[name];
            }
            else
                httpWebRequest.Headers.Add(name, this.options.RequestHeaders[name]);
        }
        if (this.options.RequestedSubProtocols.Count > 0)
        {
            httpWebRequest.Headers.Add("Sec-WebSocket-Protocol", string.Join(", ", this.options.RequestedSubProtocols));
        }
        if (this.options.UseDefaultCredentials)
        {
            httpWebRequest.UseDefaultCredentials = true;
        }
        else
        {
            if (this.options.Credentials != null)
            {
                httpWebRequest.Credentials = this.options.Credentials;
            }
        }
        if (this.options.InternalClientCertificates != null)
        {
            httpWebRequest.ClientCertificates = this.options.InternalClientCertificates;
        }
        httpWebRequest.Proxy = this.options.Proxy;
        httpWebRequest.CookieContainer = this.options.Cookies;
        this.cts.Token.Register(new Action<object>(this.AbortRequest), httpWebRequest, false);
        return httpWebRequest;
    }
    private string ValidateResponse(HttpWebRequest request, HttpWebResponse response)
    {
        if (response.StatusCode != HttpStatusCode.SwitchingProtocols)
        {
            throw new WebSocketException("net_WebSockets_Connect101Expected");
        }
        string text = response.Headers["Upgrade"];
        if (!string.Equals(text, "websocket", StringComparison.OrdinalIgnoreCase))
        {
            throw new WebSocketException("net_WebSockets_InvalidResponseHeader");
        }
        string text2 = response.Headers["Connection"];
        if (!string.Equals(text2, "Upgrade", StringComparison.OrdinalIgnoreCase))
        {
            throw new WebSocketException("net_WebSockets_InvalidResponseHeader");
        }
        string text3 = response.Headers["Sec-WebSocket-Accept"];
        string secWebSocketAcceptString = ClientWebSocketOptions2.GetSecWebSocketAcceptString(request.Headers["Sec-WebSocket-Key"]);
        if (!string.Equals(text3, secWebSocketAcceptString, StringComparison.OrdinalIgnoreCase))
        {
            throw new WebSocketException("net_WebSockets_InvalidResponseHeader");
        }
        string text4 = response.Headers["Sec-WebSocket-Protocol"];
        if (!string.IsNullOrWhiteSpace(text4) && this.options.RequestedSubProtocols.Count > 0)
        {
            bool flag = false;
            foreach (string current in this.options.RequestedSubProtocols)
            {
                if (string.Equals(current, text4, StringComparison.OrdinalIgnoreCase))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new WebSocketException("net_WebSockets_AcceptUnsupportedProtocol");
            }
        }
        if (!string.IsNullOrWhiteSpace(text4))
        {
            return text4;
        }
        return null;
    }
    private void AbortRequest(object obj)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)obj;
        httpWebRequest.Abort();
    }
    private void ThrowIfNotConnected()
    {
        if (this.state == 3)
        {
            throw new ObjectDisposedException(base.GetType().FullName);
        }
        if (this.state != 2)
        {
            throw new InvalidOperationException("net_WebSockets_NotConnected");
        }
    }
}