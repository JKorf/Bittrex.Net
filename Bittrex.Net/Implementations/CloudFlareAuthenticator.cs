using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Logging;
using CloudFlareUtilities;

namespace Bittrex.Net.Implementations
{
    internal class CloudFlareAuthenticator: ICloudFlareAuthenticator
    {
        public CookieContainer GetCloudFlareCookies(string address, string userAgent, int maxRetries)
        {
            int currentTry = 0;
            while (currentTry < maxRetries)
            {
                try
                {
                    // Create a request and a shared cookie container
                    var cookies = new CookieContainer();
                    HttpRequestMessage msg = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(address),
                        Method = HttpMethod.Get
                    };
                    msg.Headers.TryAddWithoutValidation("User-Agent", userAgent);

                    var client1 = new HttpClient(new ClearanceHandler(new HttpClientHandler
                    {
                        UseCookies = true,
                        CookieContainer = cookies
                    }));

                    client1.SendAsync(msg).Wait();

                    // Return the cookie container which should now contain the cloudflare access data
                    return cookies;
                }
                catch (Exception e)
                {
                    currentTry += 1;
                }
            }
            
            return null;
        }
    }
}
