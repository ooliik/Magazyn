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

    public sealed partial class WarehousePlacePage : Page
    {

        public WarehousePlacePage()
        {
            this.InitializeComponent();
        }

        private void BindList()
        {
            using (DBContext dBCtx = new DBContext())
            {
                var list = dBCtx.WarehousePlaces.Where(x => x.WarehouseName == WarehouseNameTb.Text).AsEnumerable();
                WPList.ItemsSource = list;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Warehouse warehouse = (Warehouse)e.Parameter;
            if (warehouse != null)
            {
                WarehouseNameTb.Text = warehouse.WarehouseName;
                WarehouseNameTb.IsReadOnly = true;
            }

            BindList();
        }

        private void BackDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dBCtx = new DBContext())
            {
                Warehouse warehouse = dBCtx.Warehouses.First(x => x.WarehouseName == WarehouseNameTb.Text);
                this.Frame.Navigate(typeof(WarehousePage), warehouse);
            }
        }

        private async void AddWPToListButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dBCtx = new DBContext())
            {
                WarehousePlace warehousePlace = new WarehousePlace(WarehouseNameTb.Text, WarehousePlaceTb.Text);
                string errorMsg = InvMgt.isWarehousePlaceValid(warehousePlace);
                if (String.IsNullOrEmpty(errorMsg))
                {
                    dBCtx.WarehousePlaces.Add(warehousePlace);
                    dBCtx.SaveChanges();
                }
                else
                {
                    MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                    await message.ShowAsync();
                }
            }
            BindList();
        }

        private async void DeleteWP_Click(object sender, RoutedEventArgs e)
        {
            Button DeleteWP = (Button)sender;
            string errorMsg = InvMgt.isWarehousePlaceAvaibleToDelete((WarehousePlace)DeleteWP.DataContext);
            using (DBContext dBCtx = new DBContext())
            {
                if (String.IsNullOrEmpty(errorMsg))
                {
                    dBCtx.WarehousePlaces.Remove((WarehousePlace)DeleteWP.DataContext);
                    dBCtx.SaveChanges();
                }
                else
                {
                    MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                    await message.ShowAsync();
                    return;
                }
            }
            BindList();
        }

        private void BackWarehousePlacesButton_Clik(object sender, RoutedEventArgs e)
        {
            using (DBContext dBCtx = new DBContext())
            {
                Warehouse warehouse = dBCtx.Warehouses.First(x => x.WarehouseName == WarehouseNameTb.Text);
                this.Frame.Navigate(typeof(WarehousePage), warehouse);
            }
        }
    }
}
