namespace Bittrex.Net.Interfaces
{
    public interface IConnectionFactory
    {
        IHubConnection Create(string url);

        IHubConnection Create(string url, string proxyDns, int proxyPort);
    }
}
