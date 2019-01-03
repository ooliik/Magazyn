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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryPage : Page
    {
        ObservableCollection<Warehouse> WarehousesColl;
        ObservableCollection<WarehousePlace> WarehousePlsColl;

        public Warehouse CurrWarehouse;

        public CategoryPage()
        {
            this.InitializeComponent();
            BindWarehouseNumberCB();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BindWarehouseNumberCB();

            using (DBContext dbCtx = new DBContext())
            {

                Category category = (Category)e.Parameter;


                if (category.CategoryID != null)
                {
                    CategoryTb.Text = category.CategoryID;
                    DescriptionTb.Text = category.Description;



                    Warehouse warehouse = dbCtx.Warehouses.FirstOrDefault(x => x.WarehouseName == category.DefaultWarehouse);
                    if (warehouse != null)
                    {
                        WarehouseCb.SelectedIndex = WarehousesColl.IndexOf(WarehousesColl.FirstOrDefault(x => x.WarehouseName == warehouse.WarehouseName));

                        if (warehouse.WarehouseName != null)
                        {
                            BindWarehousePlaceCB();
                            WarehousePlace warehousePlace = dbCtx.WarehousePlaces.FirstOrDefault(x => x.WarehousePlaceName == category.DefaultWarehousePlace && x.WarehouseName == category.DefaultWarehouse);

                            if (warehousePlace != null)
                                WarehousePlaceCb.SelectedIndex = WarehousePlsColl.IndexOf(WarehousePlsColl.FirstOrDefault(x => x.WarehousePlaceName == warehousePlace.WarehousePlaceName && x.WarehouseName == warehousePlace.WarehouseName));
                        }
                    }

                }

                else
                {
                    category = new Category();
                }
            }

        }

        private void BindWarehouseNumberCB()
        {
            using (DBContext dbCtx = new DBContext())
            {
                WarehousesColl = new ObservableCollection<Warehouse>();

                var Warehouses = dbCtx.Warehouses.ToList();
                foreach (Warehouse warehouse in Warehouses)
                {
                    WarehousesColl.Add(new Warehouse(warehouse));
                }

                WarehouseCb.ItemsSource = WarehousesColl;
            }
        }

        private async void BindWarehousePlaceCB()
        {
            using (DBContext dBCtx = new DBContext())
            {


                if (CurrWarehouse == null)
                {
                    MessageDialog message = new MessageDialog("Przed wyborem miejsca magazynowego należy wybrać magazyn.", "Operacja przerwana");
                    await message.ShowAsync();
                }
                else
                {
                    WarehousePlsColl = new ObservableCollection<WarehousePlace>();

                    var WP = dBCtx.WarehousePlaces.Where(x => x.WarehouseName == CurrWarehouse.WarehouseName).ToList();

                    foreach (WarehousePlace warpl in WP)
                    {
                        WarehousePlsColl.Add(new WarehousePlace(warpl));
                    }

                    WarehousePlaceCb.ItemsSource = WarehousePlsColl;
                }
            }
        }

        private void BackDetailsButton_Click(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(MainPage), "BackToCategoriesList");
        }

        private async void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dBCtx = new DBContext())
            {
                if (!dBCtx.Categories.Any(x => x.CategoryID == CategoryTb.Text))
                {
                    Warehouse warehouse = (Warehouse)WarehouseCb.SelectedItem;
                    WarehousePlace warehousePlace = (WarehousePlace)WarehousePlaceCb.SelectedItem;

                    Category category = new Category(CategoryTb.Text, DescriptionTb.Text, warehouse.WarehouseName, warehousePlace.WarehousePlaceName);
                    string errorMsg = InvMgt.isCategoryValid(category);
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        dBCtx.Categories.Add(category);
                        dBCtx.SaveChanges();
                    }
                    else
                    {
                        MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                        await message.ShowAsync();
                        return;
                    }
                }
                else
                {
                    Warehouse warehouse = (Warehouse)WarehouseCb.SelectedItem;
                    WarehousePlace warehousePlace = (WarehousePlace)WarehousePlaceCb.SelectedItem;

                    Category category = dBCtx.Categories.Single(x => x.CategoryID == CategoryTb.Text);
                    dBCtx.Attach(category);
                    category.Description = DescriptionTb.Text;
                    category.DefaultWarehouse = warehouse.WarehouseName;
                    category.DefaultWarehousePlace = warehousePlace.WarehousePlaceName;
                    dBCtx.SaveChanges();
                }
                this.Frame.Navigate(typeof(MainPage), "BackToCategoriesList");
            }
        }

        private async void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {

            using (DBContext dBCtx = new DBContext())
            {
                if (!dBCtx.Categories.Any(x => x.CategoryID == CategoryTb.Text))
                {
                    Category category = dBCtx.Categories.Single(x => x.CategoryID == CategoryTb.Text);
                    string errorMsg = InvMgt.isCategoryValid(category);
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        dBCtx.Categories.Remove(category);
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
            this.Frame.Navigate(typeof(MainPage), "BackToCategoriesList");

        }

        private void WarehouseCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrWarehouse = WarehouseCb.SelectedItem as Warehouse;
            BindWarehousePlaceCB();
        }


        private async void WarehousePlace_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (CurrWarehouse == null)
            {
                MessageDialog message = new MessageDialog("Przed wyborem miejsca magazynowego należy wybrać magazyn.", "Operacja przerwana");
                await message.ShowAsync();
            }
        }
    }
}
