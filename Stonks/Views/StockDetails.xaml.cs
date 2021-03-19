using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stonks.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stonks.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockDetails : ContentPage
    {
        public StockDetails(StockModel stock)
        {
            InitializeComponent();
        }
    }
}