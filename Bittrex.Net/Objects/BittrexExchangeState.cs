using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public class BittrexExchangeState
    {
        public long Nounce { get; set; }

        // only has Quantity and Rate
        public List<BittrexOrderBookEntry> Buys { get; set; }

        // has additional not added fileds like id, Total, FillType(Fill, ParitalFill)
        public List<BittrexOrderBookFillExchangeState> Fills { get; set; }

        // only has Quantity and Rate
        public List<BittrexOrderBookEntry> Sells { get; set; }

        // API gives always NULL
        public String MarketName { get; set; }
    }

    // TODO: find meaning of field names
    public class BittrexOrderBookFillExchangeState
    {
        public FillTypeExchangeState FillType { get; set; }
        public long id { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Total { get; set; }
    }

    
    public enum FillTypeExchangeState
    {
        FILL,
        PARTIAL_FILL
    }
    
}
