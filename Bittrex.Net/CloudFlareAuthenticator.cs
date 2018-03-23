using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bittrex.Net.Interfaces;
using CloudFlareUtilities;

namespace Bittrex.Net
{
    internal class CloudFlareAuthenticator: ICloudFlareAuthenticator
    {
        public async Task<CookieContainer> GetCloudFlareCookies(string address, string userAgent, int maxRetries)
        {
            var currentTry = 0;
            while (currentTry < maxRetries)
            {
                try
                {
                    // Create a request and a shared cookie container
                    var cookies = new CookieContainer();
                    var msg = new HttpRequestMessage
                    {
                        RequestUri = new Uri(address),
                        Method = HttpMethod.Get
                    };
                    msg.Headers.TryAddWithoutValidation("User-Agent", userAgent);

                    var client = new HttpClient(new ClearanceHandler(new HttpClientHandler
                    {
                        UseCookies = true,
                        CookieContainer = cookies
                    })
                    {
                        ClearanceDelay = 7000
                    });

                    await client.SendAsync(msg).ConfigureAwait(false);

                    // Return the cookie container which should now contain the cloudflare access data
                    return cookies;
                }
                catch (Exception)
                {
                    currentTry += 1;
                }
            }
            
            return null;
        }
    }
}
