
namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Market tick
    /// </summary>
    public class BittrexTick
    {
        /// <summary>
        /// Symbol of the ticker
        /// </summary>
        public string Symbol { get; set; } = "";
        /// <summary>
        /// The price of the last trade
        /// </summary>
        public decimal LastTradeRate { get; set; }
        /// <summary>
        /// The highest bid price
        /// </summary>
        public decimal BidRate { get; set; }
        /// <summary>
        /// The lowest ask price
        /// </summary>
        public decimal AskRate { get; set; }
    }
}
