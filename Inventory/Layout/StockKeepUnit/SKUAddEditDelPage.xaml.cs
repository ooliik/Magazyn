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

    public sealed partial class SKUAddEditDelPage : Page
    {
        DBContext dbCtx = new DBContext();
        List<StockKeepUnit> Units;
        Item itemm;
        ItemStockKeepUnit itemSKU;

        private void BindList()
        {
            var list = dbCtx.ItemStockKeepUnits.Where(x => x.ItemID == itemm.ItemID).AsEnumerable();
            ISKUList.ItemsSource = list;
        }

        public SKUAddEditDelPage()
        {
            this.InitializeComponent();

            var sku = dbCtx.StockKeepUnits.ToList();
            Units = sku;
            DataContext = Units;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            itemm = (Item)e.Parameter;

            BindList();

        }



        private void BackDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemPage), itemm);
        }

        private async void AddUnitToListButton_Click(object sender, RoutedEventArgs e)
        {

            var sku = UnitSelectCb.SelectedItem as StockKeepUnit;
            if (String.IsNullOrEmpty(SKUBarcodeTb.Text))
            {
                MessageDialog message = new MessageDialog("Nie wprowadzono kodu.", "Operacja przerwana");
                await message.ShowAsync();
                return;
            }

            if (sku == null)
            {
                MessageDialog message = new MessageDialog("Wybierz jednostkę z listy dostępnych.", "Operacja przerwana");
                await message.ShowAsync();
                return;
            }

            if (dbCtx.ItemStockKeepUnits.Any(x => x.ItemID == itemm.ItemID && x.Code == sku.Code))
            {
                MessageDialog message = new MessageDialog("Wybrana jednostka jest już na liście.", "Operacja przerwana");
                await message.ShowAsync();
                return;
            }

            if (dbCtx.ItemStockKeepUnits.Any(x => x.Barcode == SKUBarcodeTb.Text))
            {
                MessageDialog message = new MessageDialog("Podany kod jest już używany dla innego zapasu i innej jednostki.", "Operacja przerwana");
                await message.ShowAsync();
                return;
            }

            itemSKU = new ItemStockKeepUnit(itemm.ItemID, sku.Code, SKUBarcodeTb.Text);
            dbCtx.ItemStockKeepUnits.Add(itemSKU);
            dbCtx.SaveChanges();


            BindList();
        }

        private async void DeleteISKU_Click(object sender, RoutedEventArgs e)
        {

            Button DeleteISKU = (Button)sender;
            ItemStockKeepUnit isku = (ItemStockKeepUnit)DeleteISKU.DataContext;

            if (dbCtx.WarehouseEntries.Any(x => x.KeepUnit == isku.Code && x.ItemID == isku.ItemID))
            {

                MessageDialog message = new MessageDialog("Nie można usunąć jednostki, ponieważ jest wykorzystywana w zapisach magazynowych.", "Operacja przerwana");
                await message.ShowAsync();
            }

            else if (dbCtx.ReleaseLines.Any(x => x.StockKeepUnit == isku.Code && x.ItemID == isku.ItemID))
            {

                MessageDialog message = new MessageDialog("Nie można usunąć jednostki, ponieważ jest wykorzystywana w wydaniach magazynowych.", "Operacja przerwana");
                await message.ShowAsync();
            }
            else if (dbCtx.ReceiveLines.Any(x => x.StockKeepUnit == isku.Code && x.ItemID == isku.ItemID))
            {
                MessageDialog message = new MessageDialog("Nie można usunąć jednostki, ponieważ jest wykorzystywana w wydaniach magazynowych.", "Operacja przerwana");
                await message.ShowAsync();
            }


            else
            {
                dbCtx.ItemStockKeepUnits.Remove(isku);
                dbCtx.SaveChanges();
            }
            BindList();
        }

        
    }
}
