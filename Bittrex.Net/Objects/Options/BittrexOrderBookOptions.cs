using CryptoExchange.Net.Objects.Options;

namespace Bittrex.Net.Objects.Options
{
    /// <summary>
    /// Options for the Bittrex SymbolOrderBook
    /// </summary>
    public class BittrexOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for the Bittrex SymbolOrderBook
        /// </summary>
        public static BittrexOrderBookOptions Default { get; set; } = new BittrexOrderBookOptions();

        /// <summary>
        /// The number of entries in the order book, should be one of: 1/25/500
        /// </summary>
        public int? Limit { get; set; }

        internal BittrexOrderBookOptions Copy()
        {
            var options = Copy<BittrexOrderBookOptions>();
            options.Limit = Limit;
            return options;
        }
    }
}
