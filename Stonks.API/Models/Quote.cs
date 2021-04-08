using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stonks.API.Data;

namespace Stonks.API.Models
{
    [JsonConverter(typeof(QuoteJsonConverter))]
    public class Quote
    {
        [Key]
        public string Symbol { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Price { get; set; }
        public long Volume { get; set; }
        public DateTime LatestTradingDay { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal Change { get; set; }
        public string ChangePercent { get; set; }
    }
}