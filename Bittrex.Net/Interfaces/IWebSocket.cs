using System;

namespace Bittrex.Net.Interfaces
{
    public interface IWebsocket
    {
        event Action<Exception> OnError;
        event Action OnOpen;
        event Action OnClose;
        event Action<string> OnMessage;

        bool IsOpen();
        bool IsClosed();

        void Send(string data);

        void Open();
        void Close();
    }
}
