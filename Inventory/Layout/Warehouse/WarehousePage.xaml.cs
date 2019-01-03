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

    public sealed partial class WarehousePage : Page
    {
        public WarehousePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Warehouse warehouse = (Warehouse)e.Parameter;
            if (warehouse.WarehouseName != null)
            {
                WarehouseNameTb.Text = warehouse.WarehouseName;
                DescriptionTb.Text = warehouse.Description;
                AddressTb.Text = warehouse.Address;
                WarehouseNameTb.IsReadOnly = true;
            }

        }

        private void BackAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), "BackToWarehousesList");
        }

        private void WarehousesPlacesEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dBCtx = new DBContext())
            {
                Warehouse warehouse = dBCtx.Warehouses.Single(x => x.WarehouseName == WarehouseNameTb.Text);
                this.Frame.Navigate(typeof(WarehousePlacePage), warehouse);
            }
        }

        private async void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dBCtx = new DBContext())
            {
                Warehouse warehouse = new Warehouse();

                warehouse.WarehouseName = WarehouseNameTb.Text;
                warehouse.Description = DescriptionTb.Text;
                warehouse.Address = AddressTb.Text;

                string errorMsg = InvMgt.IsWarehouseValid(warehouse);
                if (String.IsNullOrEmpty(errorMsg))
                {
                    if (!dBCtx.Warehouses.Any(x => x.WarehouseName == WarehouseNameTb.Text))
                    {
                        dBCtx.Warehouses.Add(warehouse);
                        dBCtx.SaveChanges();
                        this.Frame.Navigate(typeof(MainPage), "BackToWarehousesList");
                    }
                    else
                    {
                        warehouse = dBCtx.Warehouses.Single(x => x.WarehouseName == WarehouseNameTb.Text);
                        dBCtx.Attach(warehouse);
                        warehouse.Description = DescriptionTb.Text;
                        warehouse.Address = AddressTb.Text;
                        dBCtx.SaveChanges();
                    }
                }
                else
                {
                    MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                    await message.ShowAsync();
                    return;
                }
            }
            this.Frame.Navigate(typeof(MainPage), "BackToWarehousesList");
        }

        private async void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dBCtx = new DBContext())
            {
                if (!dBCtx.Warehouses.Any(x => x.WarehouseName == WarehouseNameTb.Text))
                {
                    Warehouse warehouse = dBCtx.Warehouses.Single(x => x.WarehouseName == WarehouseNameTb.Text);
                    string errorMsg = InvMgt.isWarehouseAvaibleToDelete(warehouse);
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        dBCtx.Warehouses.Remove(warehouse);
                        dBCtx.SaveChanges();
                    }

                    else
                    {
                        MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                        await message.ShowAsync();
                        return;
                    }

                }
            }
            this.Frame.Navigate(typeof(MainPage), "BackToWarehousesList");
        }


    }
}
