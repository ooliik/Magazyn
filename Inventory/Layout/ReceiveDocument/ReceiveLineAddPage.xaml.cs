using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public sealed partial class ReceiveLineAddPage : Page
    {
        DBContext dbCtx = new DBContext();

        ObservableCollection<Item> ItemColl;
        ObservableCollection<ItemStockKeepUnit> ItemStockKeepUnitsColl;
        ObservableCollection<Warehouse> WarehousesColl;
        ObservableCollection<WarehousePlace> WarehousePlsColl;
        ReceiveHeader receiveHeader;
        ReceiveLine receiveLine;

        public Item CurrItem;
        public Warehouse CurrWarehouse;

        public ReceiveLineAddPage()
        {
            this.InitializeComponent();
            BindItemCB();
            BindWarehouseNumberCB();
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            receiveLine = (ReceiveLine)e.Parameter;

            if (receiveLine.DocumentID != null)
            {
                receiveHeader = dbCtx.ReceiveHeaders.FirstOrDefault(x => x.DocumentID == receiveLine.DocumentID);
                DocumentIDTb.Text = receiveLine.DocumentID;
                QuantityTb.Text = "0.0";
            }


        }

        private void BackAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReceiveHeaderPage), receiveHeader);
        }

        private async void SaveReceiveLineButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(QuantityTb.Text))
            {
                MessageDialog message = new MessageDialog("Nie podano ilości.", "Operacja przerwana");
                await message.ShowAsync();
                return;
            }
            var Itm = ItemIDCb.SelectedItem as Item;
            var ISKU = StockKeepUnitCb.SelectedItem as ItemStockKeepUnit;
            var MagNr = WarehouseNameCb.SelectedItem as Warehouse;
            var MagPl = WarehousePlaceCb.SelectedItem as WarehousePlace;
            if (Itm == null)
            {
                MessageDialog message = new MessageDialog("Nie wybrano produktu", "Operacja przerwana");
                await message.ShowAsync();
                return;
            }
            if (ISKU == null)
            {
                MessageDialog message = new MessageDialog("Nie wybrano jednostki.", "Operacja przerwana");
                await message.ShowAsync();
                return;
            }

            using (DBContext dbCtx = new DBContext())
            {

                int position = 0;
                if (dbCtx.ReceiveLines.Any(x => x.DocumentID == DocumentIDTb.Text))
                {
                    position = dbCtx.ReceiveLines.Last(x => x.DocumentID == DocumentIDTb.Text).PositionNumber + 1;
                }
                else
                {
                    position = 1;
                }
                double qty = 0.0;
                if (!String.IsNullOrEmpty(QuantityTb.Text))
                    qty = Convert.ToDouble(QuantityTb.Text);
                else
                    qty = Convert.ToDouble(0);

                receiveLine = new ReceiveLine(DocumentIDTb.Text, position, Itm.ItemID, ISKU.Code, MagNr.WarehouseName, MagPl.WarehousePlaceName, qty);
                string errorMsg = InvMgt.isReceiveLineValid(receiveLine);
                if (String.IsNullOrEmpty(errorMsg))
                {
                    dbCtx.ReceiveLines.Add(receiveLine);
                    dbCtx.SaveChanges();
                    ReceiveHeader receiveHeader = dbCtx.ReceiveHeaders.FirstOrDefault(x => x.DocumentID == receiveLine.DocumentID);

                    this.Frame.Navigate(typeof(ReceiveHeaderPage), receiveHeader);
                }
                else
                {
                    MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                    await message.ShowAsync();
                    return;
                }


            }
        }

        private async void StockKeepUnitCbTapped(object sender, TappedRoutedEventArgs e)
        {
            if (CurrItem == null)
            {
                MessageDialog message = new MessageDialog("Przed wyborem jednostek należy wybrać zapas.", "Operacja przerwana");
                await message.ShowAsync();
            }
        }

        private void ItemIDCbSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrItem = ItemIDCb.SelectedItem as Item;
            BindItemStockKeepUnitCB();

            Category category = dbCtx.Categories.FirstOrDefault(x => x.CategoryID == CurrItem.CategoryID);
            CurrWarehouse = dbCtx.Warehouses.FirstOrDefault(x => x.WarehouseName == category.DefaultWarehouse);
            if (CurrWarehouse != null)
            {
                WarehouseNameCb.SelectedIndex = WarehousesColl.IndexOf(WarehousesColl.FirstOrDefault(x => x.WarehouseName == CurrWarehouse.WarehouseName));
                BindWarehousePlaceCB();
                WarehousePlaceCb.SelectedIndex = WarehousePlsColl.IndexOf(WarehousePlsColl.FirstOrDefault(x => x.WarehousePlaceName == category.DefaultWarehousePlace && x.WarehouseName == CurrWarehouse.WarehouseName));

            }
        }

        private async void BindItemStockKeepUnitCB()
        {
            if (CurrItem == null)
            {
                MessageDialog message = new MessageDialog("Przed wyborem jednostek należy wybrać zapas.", "Operacja przerwana");
                await message.ShowAsync();
            }
            else
            {
                ItemStockKeepUnitsColl = new ObservableCollection<ItemStockKeepUnit>();

                var ISKU = dbCtx.ItemStockKeepUnits.Where(x => x.ItemID == CurrItem.ItemID).ToList();

                foreach (ItemStockKeepUnit itemsku in ISKU)
                {
                    ItemStockKeepUnitsColl.Add(new ItemStockKeepUnit(itemsku));
                }

                StockKeepUnitCb.ItemsSource = ItemStockKeepUnitsColl;
            }


        }

        private async void WarehousePlaceCbTapped(object sender, TappedRoutedEventArgs e)
        {
            if (CurrWarehouse == null)
            {
                MessageDialog message = new MessageDialog("Przed wyborem miejsca magazynowego należy wybrać magazyn.", "Operacja przerwana");
                await message.ShowAsync();
            }
        }

        private async void BindWarehousePlaceCB()
        {
            if (CurrWarehouse == null)
            {
                MessageDialog message = new MessageDialog("Przed wyborem miejsca magazynowego należy wybrać magazyn.", "Operacja przerwana");
                await message.ShowAsync();
            }
            else
            {
                WarehousePlsColl = new ObservableCollection<WarehousePlace>();

                var WP = dbCtx.WarehousePlaces.Where(x => x.WarehouseName == CurrWarehouse.WarehouseName).ToList();

                foreach (WarehousePlace warpl in WP)
                {
                    WarehousePlsColl.Add(new WarehousePlace(warpl));
                }

                WarehousePlaceCb.ItemsSource = WarehousePlsColl;
            }
        }

        private void BindItemCB()
        {
            ItemColl = new ObservableCollection<Item>();

            var Items = dbCtx.Items.ToList();
            foreach (Item item in Items)
            {
                ItemColl.Add(item);
            }

            ItemIDCb.ItemsSource = ItemColl;
        }

        private void BindWarehouseNumberCB()
        {
            WarehousesColl = new ObservableCollection<Warehouse>();

            var Warehouses = dbCtx.Warehouses.ToList();
            foreach (Warehouse warehouse in Warehouses)
            {
                WarehousesColl.Add(new Warehouse(warehouse));
            }

            WarehouseNameCb.ItemsSource = WarehousesColl;
        }

        private void WarehouseNameCbSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrWarehouse = WarehouseNameCb.SelectedItem as Warehouse;
            BindWarehousePlaceCB();
        }
    }
}
