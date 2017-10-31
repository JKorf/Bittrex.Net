namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Price of a market
    /// </summary>
    public class BittrexPrice
    {
        /// <summary>
        /// The highest bid on this market
        /// </summary>
        public double Bid { get; set; }
        /// <summary>
        /// The lowest ask on this market
        /// </summary>
        public double Ask { get; set; }
        /// <summary>
        /// The last price an order was completen at
        /// </summary>
        public double Last { get; set; }
    }
}
