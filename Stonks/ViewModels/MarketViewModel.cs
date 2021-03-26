using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Stonks.Views;
using Stonks.Models;
using Stonks.ServicesHandler;
using Xamarin.Forms;

namespace Stonks.ViewModels
{
    public class MarketViewModel : INotifyPropertyChanged
    {
        private StockServices _stockServices = new StockServices();
        private ObservableCollection<StockModel> _stocks;
        private string _searchText = string.Empty;
        private Command _searchCommand;
        private bool _isRefreshing;
        public StockModel SelectedStock { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public ObservableCollection<StockModel> Stocks
        {
            get
            {
                var stockCollection = new ObservableCollection<StockModel>();

                if (_stocks == null) return stockCollection;
                
                var query = _searchText.ToLower();
                var stockList = _stocks.Where(stock =>
                    stock.Name.ToLower().Contains(query) || stock.Symbol.ToLower().Contains(query)).ToList();

                if (stockList.Any())
                    stockCollection = new ObservableCollection<StockModel>(stockList);

                return stockCollection;
            }
        }
        
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                
                _searchText = value ?? string.Empty;
                OnPropertyChanged();

                if (SearchCommand.CanExecute(null))
                    SearchCommand.Execute(null);
            }
        }

        public Command StockSelectionChangedCommand => new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(
                new StockDetails(new StockDetailsViewModel(SelectedStock)));
        });

        public Command SearchCommand
        {
            get
            {
                _searchCommand = _searchCommand ?? new Command(() => OnPropertyChanged(nameof(Stocks)));
                return _searchCommand;
            }
        }
        
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
        
        public Command RefreshCommand { get; set; }

        private async Task GetStocksAsync()
        {
            try
            {
                IsRefreshing = true;
                var stockList = await _stockServices.GetStocks();
                _stocks = new ObservableCollection<StockModel>(stockList);
                OnPropertyChanged(nameof(Stocks));
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public MarketViewModel()
        {
            RefreshCommand = new Command(async () => await GetStocksAsync());
            Task.Run(async () => {
                await GetStocksAsync();
            });
        }
    }
}