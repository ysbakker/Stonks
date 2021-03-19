using System;

namespace Stonks.API
{
    public class Stock
    {
        private string Symbol { get; set; }
        public string Name { get; set; }
        public string AssetType { get; set; }
        public string Description { get; set; }
        public string Sector { get; set; }
    }
}