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
        Proxy = WebRequest.DefaultWebProxy;
    }

    public ArraySegment<byte> GetOrCreateBuffer()
    {
        if (!buffer.HasValue)
            buffer = WebSocket.CreateClientBuffer(ReceiveBufferSize, SendBufferSize);
        
        return buffer.Value;
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
            return options;
        }
    }
    public override WebSocketCloseStatus? CloseStatus
    {
        get
        {
            return innerWebSocket != null ? innerWebSocket.CloseStatus : null;
        }
    }
    public override string CloseStatusDescription
    {
        get
        {
            return innerWebSocket != null ? innerWebSocket.CloseStatusDescription : null;
        }
    }
    public override string SubProtocol
    {
        get
        {
            return innerWebSocket != null ? innerWebSocket.SubProtocol : null;
        }
    }
    public override WebSocketState State
    {
        get
        {
            if (innerWebSocket != null)
            {
                return innerWebSocket.State;
            }
            switch (state)
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
        state = 0;
        options = new ClientWebSocketOptions2();
        cts = new CancellationTokenSource();
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
        int num = Interlocked.CompareExchange(ref state, 1, 0);
        if (num == 3)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
        if (num != 0)
        {
            throw new InvalidOperationException("net_WebSockets_AlreadyStarted");
        }
        return ConnectAsyncCore(uri, cancellationToken);
    }
    public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        ThrowIfNotConnected();
        return innerWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }
    public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        ThrowIfNotConnected();
        return innerWebSocket.ReceiveAsync(buffer, cancellationToken);
    }
    public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        ThrowIfNotConnected();
        return innerWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }
    public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        ThrowIfNotConnected();
        return innerWebSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
    }
    public override void Abort()
    {
        if (state == 3)
        {
            return;
        }
        innerWebSocket?.Abort();
        Dispose();
    }
    public override void Dispose()
    {
        int num = Interlocked.Exchange(ref state, 3);
        if (num == 3)
        {
            return;
        }
        cts.Cancel(false);
        cts.Dispose();
        innerWebSocket?.Dispose();
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
            HttpWebRequest httpWebRequest = CreateAndConfigureRequest(uri);

            cancellationTokenRegistration = cancellationToken.Register(AbortRequest, httpWebRequest, false);
            httpWebResponse = await httpWebRequest.GetResponseAsync().ConfigureAwait(false) as HttpWebResponse;

            string subProtocol = ValidateResponse(httpWebRequest, httpWebResponse);
            innerWebSocket = CreateClientWebSocket(httpWebResponse.GetResponseStream(), subProtocol, options.ReceiveBufferSize, options.SendBufferSize, options.KeepAliveInterval, false, options.GetOrCreateBuffer());

            if (Interlocked.CompareExchange(ref state, 2, 1) != 1)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
        catch (WebException innerException)
        {
            ConnectExceptionCleanup(httpWebResponse);
            WebSocketException ex = new WebSocketException("net_webstatus_ConnectFailure", innerException);
            throw ex;
        }
        catch (Exception)
        {
            ConnectExceptionCleanup(httpWebResponse);
            throw;
        }
        finally
        {
            cancellationTokenRegistration.Dispose();
        }
    }
    private void ConnectExceptionCleanup(HttpWebResponse response)
    {
        Dispose();
        response?.Dispose();
    }
    private HttpWebRequest CreateAndConfigureRequest(Uri uri)
    {
        HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
        if (httpWebRequest == null)
        {
            throw new InvalidOperationException("net_WebSockets_InvalidRegistration");
        }
        foreach (string name in options.RequestHeaders.Keys)
        {
            if (name.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
            {
                httpWebRequest.UserAgent = options.RequestHeaders[name];
            }
            else
                httpWebRequest.Headers.Add(name, options.RequestHeaders[name]);
        }
        if (options.RequestedSubProtocols.Count > 0)
        {
            httpWebRequest.Headers.Add("Sec-WebSocket-Protocol", string.Join(", ", options.RequestedSubProtocols));
        }
        if (options.UseDefaultCredentials)
        {
            httpWebRequest.UseDefaultCredentials = true;
        }
        else
        {
            if (options.Credentials != null)
            {
                httpWebRequest.Credentials = options.Credentials;
            }
        }
        if (options.InternalClientCertificates != null)
        {
            httpWebRequest.ClientCertificates = options.InternalClientCertificates;
        }
        httpWebRequest.Proxy = options.Proxy;
        httpWebRequest.CookieContainer = options.Cookies;
        cts.Token.Register(AbortRequest, httpWebRequest, false);
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
        if (!string.IsNullOrWhiteSpace(text4) && options.RequestedSubProtocols.Count > 0)
        {
            bool flag = false;
            foreach (string current in options.RequestedSubProtocols)
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
        if (state == 3)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
        if (state != 2)
        {
            throw new InvalidOperationException("net_WebSockets_NotConnected");
        }
    }
}