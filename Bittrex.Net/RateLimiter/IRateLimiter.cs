namespace Bittrex.Net.RateLimiter
{
    public interface IRateLimiter
    {
        double LimitRequest(string url);
    }
}
