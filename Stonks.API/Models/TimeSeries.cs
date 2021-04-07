using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stonks.API.Data;

namespace Stonks.API.Models
{
    public class TimeSeries
    {
        [Required]
        public DateTime TimeStamp { get; set; }
        
        [Required]
        public decimal Open { get; set; }
        
        [Required]
        public decimal High { get; set; }
        
        [Required]
        public decimal Low { get; set; }
        
        [Required]
        public decimal Close { get; set; }
        
        [Required]
        public long Volume { get; set; }
        
        [ForeignKey("Symbol")]
        public string Symbol { get; set; }
    }
}