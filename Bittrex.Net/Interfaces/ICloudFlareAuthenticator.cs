using System.Net;
using System.Threading.Tasks;

namespace Bittrex.Net.Interfaces
{
    public interface ICloudFlareAuthenticator
    {
        Task<CookieContainer> GetCloudFlareCookies(string address, string userAgent, int maxRetries);
    }
}
