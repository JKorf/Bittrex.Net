using System;
using CryptoExchange.Net.ExchangeInterfaces;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Symbol kline info
    /// </summary>
    public class BittrexKline: ICommonKline
    {
        /// <summary>
        /// The opening time of this kline
        /// </summary>
        public DateTime StartsAt { get; set; }
        /// <summary>
        /// The price at opening
        /// </summary>
        public decimal Open { get; set; }
        /// <summary>
        /// The price at closing
        /// </summary>
        public decimal Close { get; set; }
        /// <summary>
        /// The highest price during this kline
        /// </summary>
        public decimal High { get; set; }
        /// <summary>
        /// The lowest price during this kline
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        /// The volume during this kline
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// The quote volume of this candle
        /// </summary>
        public decimal QuoteVolume { get; set; }

        decimal ICommonKline.CommonHigh => High;
        decimal ICommonKline.CommonLow => Low;
        decimal ICommonKline.CommonOpen => Open;
        decimal ICommonKline.CommonClose => Close;
        decimal ICommonKline.CommonVolume => Volume;
        DateTime ICommonKline.CommonOpenTime => StartsAt;
    }
}
