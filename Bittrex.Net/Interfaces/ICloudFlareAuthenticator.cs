using System.Net;

namespace Bittrex.Net.Interfaces
{
    public interface ICloudFlareAuthenticator
    {
        CookieContainer GetCloudFlareCookies(string address, string userAgent, int maxRetries);
    }
}
