using Inventory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Inventory
{

    public sealed partial class DetailsEditUnitsPage : Page
    {
        DBContext dbCtx = new DBContext();
        StockKeepUnit stockKeepUnit;

        public DetailsEditUnitsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            stockKeepUnit = (StockKeepUnit)e.Parameter;

            CodeTb.Text = stockKeepUnit.Code;
            DescriptionTb.Text = stockKeepUnit.Description;
            QuantityPerUnitTb.Text = stockKeepUnit.QuantityPerUnit.ToString();

            dbCtx.StockKeepUnits.Attach(stockKeepUnit);

        }

        private void BackAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), "BackToUnitsList");
        }

        private void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            stockKeepUnit.Code = CodeTb.Text;
            stockKeepUnit.Description = DescriptionTb.Text;
            stockKeepUnit.QuantityPerUnit = Convert.ToDouble(QuantityPerUnitTb.Text);

            dbCtx.SaveChanges();

            this.Frame.Navigate(typeof(MainPage), "BackToUnitsList");
        }

        private async void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {

            if (dbCtx.WarehouseEntries.Any(x => x.KeepUnit == stockKeepUnit.Code))
            {

                MessageDialog message = new MessageDialog("Nie można usunąć jednostki, ponieważ jest wykorzystywana w zapisach magazynowych.", "Operacja przerwana");
                await message.ShowAsync();
            }
            else if (dbCtx.ItemStockKeepUnits.Any(x => x.Code == stockKeepUnit.Code))
            {
                MessageDialog message = new MessageDialog("Nie można usunąć jednostki, ponieważ jest wykorzystywana w jednostach miary dla produktu.", "Operacja przerwana");
                await message.ShowAsync();
            }
            else if (dbCtx.ReleaseLines.Any(x => x.StockKeepUnit == stockKeepUnit.Code))
            {

                MessageDialog message = new MessageDialog("Nie można usunąć jednostki, ponieważ jest wykorzystywana w wydaniach magazynowych.", "Operacja przerwana");
                await message.ShowAsync();
            }
            else if (dbCtx.ReceiveLines.Any(x => x.StockKeepUnit == stockKeepUnit.Code))
            {
                MessageDialog message = new MessageDialog("Nie można usunąć jednostki, ponieważ jest wykorzystywana w wydaniach magazynowych.", "Operacja przerwana");
                await message.ShowAsync();
            }

            else
            {
                dbCtx.StockKeepUnits.Remove(stockKeepUnit);
                dbCtx.SaveChanges();
            }
            this.Frame.Navigate(typeof(MainPage), "BackToUnitsList");
        }
    }
}



