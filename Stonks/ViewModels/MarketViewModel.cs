using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Stonks
{
    public class MarketViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<StockModel> _stocks;
        private string _searchText = string.Empty;
        private Command _searchCommand;

        public ObservableCollection<StockModel> Stocks
        {
            get
            {
                ObservableCollection<StockModel> theCollection = new ObservableCollection<StockModel>();

                if (_stocks != null)
                {
                    List<StockModel> entities = (from e in _stocks
                        where e.Name.Contains(_searchText)
                        select e).ToList<StockModel>();
                    if (entities != null && entities.Any())
                    {
                        theCollection = new ObservableCollection<StockModel>(entities);
                    }
                }

                return theCollection;
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value ?? string.Empty;
                    OnPropertyChanged();

                    // Perform the search
                    if (SearchCommand.CanExecute(null))
                    {
                        SearchCommand.Execute(null);
                    }
                }
            }
        }
        
        public ICommand SearchCommand
        {
            get
            {
                _searchCommand = _searchCommand ?? new Command(DoSearchCommand, CanExecuteSearchCommand);
                return _searchCommand;
            }
        }
        
        private void DoSearchCommand()
        {
            // Refresh the list, which will automatically apply the search text
            OnPropertyChanged(nameof(Stocks));
        }
        
        private bool CanExecuteSearchCommand()
        {
            return true;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MarketViewModel()
        {
            _stocks = new ObservableCollection<StockModel>
            {
                new StockModel("Dow Jones Industial Average", "DOW J", "33.050,98", "+0,15%"),
                new StockModel("AEX-INDEX", "^AEX", "682,57", "+0,27%"),
                new StockModel("Apple Inc.", "AAPL", "121,70", "-2,45%"),
                new StockModel("Microsoft Corporation", "MSFT", "232,77", "-1,80%"),
                new StockModel("Tesla, Inc.", "TSLA", "677,53", "-3,46%")
            };
        }
    }
}