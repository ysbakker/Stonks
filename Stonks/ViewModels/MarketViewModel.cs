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
        private bool _isRefreshing;
        public Command StockSelectedCommand { get; }
        public Command SearchCommand { get; }
        public Command RefreshCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        
        public MarketViewModel()
        {
            StockSelectedCommand = new Command(async (selected) =>
            {
                StockModel selectedStock = (StockModel) selected;
                await Application.Current.MainPage.Navigation.PushAsync(
                    new StockDetails(new StockDetailsViewModel(selectedStock)));
            });
            
            SearchCommand = new Command(() => OnPropertyChanged(nameof(Stocks)));

            RefreshCommand = new Command(async () => await GetStocksAsync());

            Task.Run(async () => await GetStocksAsync());
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
        
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}