using System;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Symbol summary info
    /// </summary>
    public class BittrexSymbolSummaryV3: ICommonTicker
    {
        /// <summary>
        /// the symbol the summary is for
        /// </summary>
        public string Symbol { get; set; } = "";
        /// <summary>
        /// The high price for this symbol in the last 24 hours
        /// </summary>
        public decimal High { get; set; }
        /// <summary>
        /// The low price for this symbol in the last 24 hours
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        /// Volume within the last 24 hours
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// Quote volume within the last 24 hours
        /// </summary>
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// The percentage change of this symbol for the last 24 hours
        /// </summary>
        public decimal PercentChange { get; set; }
        /// <summary>
        /// The timestamp of when this summary was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        string ICommonTicker.CommonSymbol => Symbol;
        decimal ICommonTicker.CommonHigh => High;
        decimal ICommonTicker.CommonLow => Low;
        decimal ICommonTicker.CommonVolume => Volume;
    }
}
