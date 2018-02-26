namespace Bittrex.Net.Interfaces
{
    public interface IConnectionFactory
    {
        IHubConnection Create(string url);
    }
}
