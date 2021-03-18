using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Stonks
{
    public class MarketViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<StockModel> Stocks { get; set; }
        // private string _searchQuery;
        // public string SearchQuery
        // {
        //     get => _searchQuery;
        //     set
        //     {
        //         SearchQuery = value;
        //         SearchStock(_searchQuery);
        //         RaisePropertyChanged("SearchQuery");
        //     }
        // }

        public MarketViewModel()
        {
            Stocks = new ObservableCollection<StockModel>
            {
                new StockModel("Dow Jones Industial Average", "DOW J", "33.050,98", "+0,15"),
                new StockModel("AEX-INDEX", "^AEX", "682,57", "+0,27"),
                new StockModel("Apple Inc.", "AAPL", "121,70", "-2,45"),
                new StockModel("Microsoft Corporation", "MSFT", "232,77", "-1,80"),
                new StockModel("Tesla, Inc.", "TSLA", "677,53", "-3,46")
            };
        }
    }
}