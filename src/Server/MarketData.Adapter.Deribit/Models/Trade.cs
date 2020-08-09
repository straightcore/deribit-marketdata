using System;
using System.ComponentModel.DataAnnotations;

namespace MarketData.Adapter.Deribit.Models
{
    public class Trade
    {
        public decimal Amount { get; set; }
        public string BlockTradeId { get; set; }
        public string Direction { get; set; }
        public decimal IndexPrice { get; set; }
        public string InstrumentName { get; set; }
        public decimal? Volatility { get; set; }
        public string Liquidation { get; set; }
        public decimal MarkPrice { get; set; }
        public decimal Price { get; set; }
        public int TickDirection { get; set; }
        public DateTime Date { get; set; }
        public string TradeId { get; set; }
        public int TradeSequence { get; set; }
        [Key]
        public string Id { get; set; }
    }
}