namespace Bittrex.Net.Enums
{
    /// <summary>
    /// Interval of klines, int value represent the time in seconds
    /// </summary>
    public enum KlineInterval
    {
        /// <summary>
        /// 1m
        /// </summary>
        OneMinute = 60,
        /// <summary>
        /// 5m
        /// </summary>
        FiveMinutes = 60 * 5,
        /// <summary>
        /// 1h
        /// </summary>
        OneHour = 60 * 60,
        /// <summary>
        /// 1d
        /// </summary>
        OneDay = 60 * 60 * 24
    }
}
