using System.Collections.Generic;
using System.Linq;
using Bittrex.Net.Objects;

namespace Bittrex.Net.Errors
{
    internal static class BittrexErrors
    {
        internal static Dictionary<BittrexErrorKey, BittrexError> ErrorRegistrations =
            new Dictionary<BittrexErrorKey, BittrexError>()
            {
                { BittrexErrorKey.NoApiCredentialsProvided, new BittrexError(5000, "No api credentials provided, can't request private endpoints")},

                { BittrexErrorKey.ErrorWeb, new BittrexError(6001, "Server returned a not successful status")},
                { BittrexErrorKey.CantConnectToServer, new BittrexError(6002, "Could not connect to Bittrex server")},

                { BittrexErrorKey.ParseErrorReader, new BittrexError(7000, "Error reading the returned data. Data was not valid Json")},
                { BittrexErrorKey.ParseErrorSerialization, new BittrexError(7001, "Error parsing the returned data to object.")},

                { BittrexErrorKey.UnknownError, new BittrexError(8000, "An unknown error happened")},
            };

        internal static BittrexError GetError(BittrexErrorKey key)
        {
            return ErrorRegistrations.Single(e => e.Key == key).Value;
        }
    }
}
