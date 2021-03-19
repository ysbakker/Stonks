using Stonks.Models;

namespace Stonks.ViewModels
{
    public class StockDetailsViewModel
    {
        public StockModel Stock { get; set; }

        public StockDetailsViewModel()
        {
            Stock = new StockModel();
        }
        public StockDetailsViewModel(StockModel stock)
        {
            Stock = stock;
        }

        public string TitleText => $"Page for {Stock.Name}.";
    }
}