using System;
using System.Collections.Generic;
using System.Linq;
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

            if (!Regex.IsMatch(symbolString, "^((([A-Z]|[0-9]){2,})[-](([A-Z]|[0-9]){2,}))$"))
                throw new ArgumentException($"{symbolString} is not a valid Bittrex symbol. Should be [BaseCurrency]-[QuoteCurrency] e.g. ETH-BTC");
        }

        /// <summary>
        /// Get the sequence number from the response headers, or null if no sequence number is present
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static long? GetSequence(this IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            var sequence = headers.SingleOrDefault(r => r.Key == "Sequence").Value?.FirstOrDefault();
            if (sequence != null)
                return long.Parse(sequence);
            return null;
        }
    }
}
