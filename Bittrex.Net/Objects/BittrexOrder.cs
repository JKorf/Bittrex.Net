﻿using System;
using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Data on an unplaced order
    /// </summary>
    public class BittrexUnplacedOrder
    {
        /// <summary>
        /// The symbol of the order
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The direction of the order
        /// </summary>
        [JsonProperty("direction"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// The type of order
        /// </summary>
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType Type { get; set; }
        /// <summary>
        /// The quantity of the order
        /// </summary>
        public decimal? Quantity { get; set; }
        /// <summary>
        /// The price of the order
        /// </summary>
        [JsonProperty("limit")]
        public decimal? Price { get; set; }
        /// <summary>
        /// The ceiling of the order
        /// </summary>
        public decimal? Ceiling { get; set; }
        /// <summary>
        /// The time in force of the order
        /// </summary>
        [JsonConverter(typeof(TimeInForceConverter))]
        public TimeInForce? TimeInForce { get; set; }

        /// <summary>
        /// Id to track the order by
        /// </summary>
        public string ClientOrderId { get; set; } = string.Empty;
        /// <summary>
        /// Use awards
        /// </summary>
        public bool UseAwards { get; set; }
    }

    /// <summary>
    /// Bittrex order info
    /// </summary>
    public class BittrexOrder: BittrexUnplacedOrder, ICommonOrder
    {
        /// <summary>
        /// The id of the order
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The quantity that's been filled
        /// </summary>
        [JsonProperty("fillQuantity")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// The fee paid for this order
        /// </summary>
        [JsonProperty("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// The quote quantity filled of this order
        /// </summary>
        [JsonProperty("proceeds")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// The status of the order
        /// </summary>
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// When the order was created
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// When the order was last updated
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// When the order was closed
        /// </summary>
        [JsonProperty("closedAt")]
        public DateTime? CloseTime { get; set; }

        /// <summary>
        /// Conditional order to cancel if this order executes
        /// </summary>
        public BittrexLinkedOrder? OrderToCancel { get; set; }

        string ICommonOrderId.CommonId => Id;
        string ICommonOrder.CommonSymbol => Symbol;
        decimal ICommonOrder.CommonPrice => Price ?? 0;
        decimal ICommonOrder.CommonQuantity => Quantity ?? 0;
        IExchangeClient.OrderStatus ICommonOrder.CommonStatus =>
            Status == OrderStatus.Open ? IExchangeClient.OrderStatus.Active :
            QuantityFilled < Quantity ? IExchangeClient.OrderStatus.Canceled :
            IExchangeClient.OrderStatus.Filled;
        bool ICommonOrder.IsActive => Status == OrderStatus.Open;
        DateTime ICommonOrder.CommonOrderTime => CreateTime;

        IExchangeClient.OrderSide ICommonOrder.CommonSide => Side == OrderSide.Sell
            ? IExchangeClient.OrderSide.Sell
            : IExchangeClient.OrderSide.Buy;

        IExchangeClient.OrderType ICommonOrder.CommonType
        {
            get
            {
                if (Type == OrderType.Limit)
                    return IExchangeClient.OrderType.Limit;
                return IExchangeClient.OrderType.Market;
            }
        }
    }
}
