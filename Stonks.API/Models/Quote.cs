﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Stonks.API.Models
{
    public class Quote
    {
        [Key]
        //[JsonPropertyName("01. symbol")]
        public string Symbol { get; set; }
        //[JsonPropertyName("02. open")]
        public decimal Open { get; set; }
        //[JsonPropertyName("03. high")]
        public decimal High { get; set; }
        //[JsonPropertyName("04. low")]
        public decimal Low { get; set; }
        //[JsonPropertyName("05. price")]
        public decimal Price { get; set; }
        //[JsonPropertyName("06. volume")]
        public long Volume { get; set; }
        //[JsonPropertyName("07. latest trading day")]
        public DateTime LatestTradingDay { get; set; }
        //[JsonPropertyName("08. previous close")]
        public decimal PreviousClose { get; set; }
        //[JsonPropertyName("09. change")]
        public decimal Change { get; set; }
        //[JsonPropertyName("10. change percent")]
        public string ChangePercent { get; set; }
    }
}