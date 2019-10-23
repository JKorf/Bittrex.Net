using System;
using System.Text.RegularExpressions;

namespace Bittrex.Net
{
    /// <summary>
    /// Helper functions
    /// </summary>
    public static class BittrexHelpers
    {
        /// <summary>
        /// Validate the string is a valid Bittrex symbol.
        /// </summary>
        /// <param name="symbolString">string to validate</param>
        public static void ValidateBittrexSymbol(this string symbolString)
        {
            if (string.IsNullOrEmpty(symbolString))
                throw new ArgumentException("Symbol is not provided");

            if (!Regex.IsMatch(symbolString, "^((([A-Z]|[0-9]){2,5})[-](([A-Z]|[0-9]){2,5}))$"))
                throw new ArgumentException($"{symbolString} is not a valid Bittrex symbol. Should be [BaseCurrency]-[QuoteCurrency] for V1 API or other way around for V3 API, e.g. BTC-ETH");
        }
    }
}
