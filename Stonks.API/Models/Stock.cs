using System.ComponentModel.DataAnnotations;

namespace Stonks.API.Models
{
    public class Stock
    {
        [Key]
        public string Symbol { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string AssetType { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Sector { get; set; }
    }
}