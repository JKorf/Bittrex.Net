namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Price of a symbol
    /// </summary>
    public class BittrexPrice
    {
        /// <summary>
        /// The highest bid on this symbol
        /// </summary>
        public decimal Bid { get; set; }
        /// <summary>
        /// The lowest ask on this symbol
        /// </summary>
        public decimal Ask { get; set; }
        /// <summary>
        /// The last price an order was completed at
        /// </summary>
        public decimal Last { get; set; }
    }
}
