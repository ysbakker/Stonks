using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Stonks
{
    public class MarketViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Stock> Stocks { get; set; }
        public string SearchQuery { get; set; }

        public MarketViewModel()
        {
            Stocks = new ObservableCollection<Stock>
            {
                new Stock("Dow Jones Industial Average", "DOW J"),
                new Stock("AEX-INDEX", "^AEX"),
                new Stock("Apple Inc.", "AAPL"),
                new Stock("Microsoft Corporation", "MSFT"),
                new Stock("Tesla, Inc.", "TSLA")
            };
        }
    }
}