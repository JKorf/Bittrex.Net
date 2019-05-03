using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexDepositAddressV3
    {
        [JsonConverter(typeof(DepositAddressStatusConverter))]
        public DepositAddressStatus Status { get; set; }
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; }
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; }
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; }
    }
}
