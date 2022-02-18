namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Api addresses usable for the Bittrex clients
    /// </summary>
    public class BittrexApiAddresses
    {
        /// <summary>
        /// The address used by the BittrexClient for the rest API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the BittrexSocketClient for the socket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Bittrex.com API
        /// </summary>
        public static BittrexApiAddresses Default = new BittrexApiAddresses
        {
            RestClientAddress = "https://api.bittrex.com/",
            SocketClientAddress = "https://socket-v3.bittrex.com"
        };
    }
}
