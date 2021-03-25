using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Stonks.Views;
using Stonks.Models;
using Xamarin.Forms;

namespace Stonks.ViewModels
{
    public class MarketViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<StockModel> _stocks;
        private string _searchText = string.Empty;
        public Command StockSelectedCommand { get; }
        public Command SearchCommand { get; }
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
            
            _stocks = new ObservableCollection<StockModel>
            {
                new StockModel("Dow Jones Industrial Average", "DOW J", "33.050,98", "+0,15%"),
                new StockModel("AEX-INDEX", "^AEX", "682,57", "+0,27%"),
                new StockModel("Apple Inc.", "AAPL", "121,70", "-2,45%"),
                new StockModel("Microsoft Corporation", "MSFT", "232,77", "-1,80%"),
                new StockModel("Tesla, Inc.", "TSLA", "677,53", "-3,46%")
            };
        }
        
        public ObservableCollection<StockModel> Stocks
        {
            get
            {
                ObservableCollection<StockModel> stockCollection = new ObservableCollection<StockModel>();

                if (_stocks != null)
                {
                    string query = _searchText.ToLower();
                    List<StockModel> stockList = _stocks.Where(stock =>
                        stock.Name.ToLower().Contains(query) || stock.Symbol.ToLower().Contains(query)).ToList();

                    if (stockList.Any())
                        stockCollection = new ObservableCollection<StockModel>(stockList);
                }

                return stockCollection;
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

                    if (SearchCommand.CanExecute(null))
                        SearchCommand.Execute(null);
                }
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}