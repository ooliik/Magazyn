using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory;
using System.Collections.ObjectModel;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Inventory
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Item CurrItem;
        ObservableCollection<ItemStockKeepUnit> ItemStockKeepUnitsColl;
        ObservableCollection<Item> ItemColl;

        private void BindPivot1()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.Items.OrderBy(x => x.ItemID).AsEnumerable();
                MyList.ItemsSource = list;
            }
        }

        private void BindPivot2()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var result = dbCtx.WarehouseEntries
            .GroupBy(cr => cr.ItemID)
            .Select(g => new
            {
                ItemID = g.First().ItemID,
                WarehouseNumber = g.First().WarehouseNumber,
                TotalQuantity = g.Sum(x => x.TotalQuantity)
            })
            .ToList();
                MyList2.ItemsSource = result;
            }
        }

        private void BindPivot3()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var last = dbCtx.WarehouseEntries
                            .OrderByDescending(c => c.ItemID).Take(10).OrderBy(c => c.ItemID);

                Last10List.ItemsSource = last;
            }
        }

        private void BindUnitsList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.StockKeepUnits.AsEnumerable();
                UnitsList.ItemsSource = list;
            }
        }

        private void BindCategoriesList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.Categories.AsEnumerable();
                CategoriesList.ItemsSource = list;
            }
        }

        private void BindWarehousesList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.Warehouses.AsEnumerable();
                WarehousesList.ItemsSource = list;
            }
        }

        private void BindCustomersList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.Customers.AsEnumerable();
                CustomersList.ItemsSource = list;
            }
        }

        private void BindVendorsList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.Vendors.AsEnumerable();
                VendorsList.ItemsSource = list;
            }
        }

        private void BindReleaseHeaderList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.ReleaseHeaders.AsEnumerable();
                ReleaseHeaderList.ItemsSource = list;
            }
        }

        private void BindReceiveHeaderList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.ReceiveHeaders.AsEnumerable();
                ReceiveHeaderList.ItemsSource = list;
            }
        }

        private void BindInventoryHeaderList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.InventoryHeaders.AsEnumerable();
                InventoryHeaderList.ItemsSource = list;
            }
        }

        private void BindHistoryList()
        {
            IEnumerable<WarehouseEntry> List;
            using (DBContext dbCtx = new DBContext())
            {
                if (String.IsNullOrEmpty(HistoryTb.Text))
                    List = dbCtx.WarehouseEntries.AsEnumerable();
                else
                    List = dbCtx.WarehouseEntries.Where(x => x.DocumentNumber.Contains(HistoryTb.Text));
                HistoryList.ItemsSource = List;
            }
        }

        private void BindSearchList()
        {

            Item item = ItemSearchCb.SelectedItem as Item;
            ItemStockKeepUnit isku = ISKUCb.SelectedItem as ItemStockKeepUnit;

            using (DBContext dbCtx = new DBContext())
            {
                if (item != null && isku != null)
                {
                    var result = dbCtx.WarehouseEntries
                             .GroupBy(cr => new { cr.ItemID, cr.KeepUnit, cr.WarehouseNumber, cr.WarehousePlace })
                             .Select(g => new
                             {
                                 ItemID = g.First().ItemID,
                                 KeepUnit = g.First().KeepUnit,
                                 WarehouseNumber = g.First().WarehouseNumber,
                                 WarehousePlace = g.First().WarehousePlace,
                                 Quantity = g.Sum(x => x.Quantity)
                             })
                             .Where(x => x.ItemID == item.ItemID && x.KeepUnit == isku.Code && x.Quantity > 0)
                             .ToList();
                    SearchList.ItemsSource = result;
                }
            }
        }

        private void BindItemCB()
        {
            ItemColl = new ObservableCollection<Item>();
            using (DBContext dbCtx = new DBContext())
            {
                var Items = dbCtx.Items.ToList();
                foreach (Item item in Items)
                {
                    ItemColl.Add(item);
                }
            }
            ItemSearchCb.ItemsSource = ItemColl;
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
                using (DBContext dbCtx = new DBContext())
                {
                    var ISKU = dbCtx.ItemStockKeepUnits.Where(x => x.ItemID == CurrItem.ItemID).ToList();

                    foreach (ItemStockKeepUnit itemsku in ISKU)
                    {
                        ItemStockKeepUnitsColl.Add(new ItemStockKeepUnit(itemsku));
                    }
                    ISKUCb.ItemsSource = ItemStockKeepUnitsColl;
                }
            }
        }

        private void SetVisibility(bool ItemListVisible, bool ReleaseHeaderListVisible, bool ReceiveListVisible, bool UnitsVisible, bool CategoriesVisible, bool WarehousesVisible, bool CustomersVisible, bool VendorsVisible, bool SearchVisible, bool InventoryVisible, bool HistoryVisible)
        {
            if (ItemListVisible)
                ItemListGrid.Visibility = Visibility.Visible;
            else
                ItemListGrid.Visibility = Visibility.Collapsed;

            if (ReleaseHeaderListVisible)
                ReleaseGrid.Visibility = Visibility.Visible;

            else
                ReleaseGrid.Visibility = Visibility.Collapsed;

            if (ReceiveListVisible)
                ReceiveGrid.Visibility = Visibility.Visible;
            else
                ReceiveGrid.Visibility = Visibility.Collapsed;

            if (UnitsVisible)
                UnitsGrid.Visibility = Visibility.Visible;
            else
                UnitsGrid.Visibility = Visibility.Collapsed;

            if (CategoriesVisible)
                CategoriesGrid.Visibility = Visibility.Visible;
            else
                CategoriesGrid.Visibility = Visibility.Collapsed;

            if (WarehousesVisible)
                WarehousesGrid.Visibility = Visibility.Visible;
            else
                WarehousesGrid.Visibility = Visibility.Collapsed;

            if (CustomersVisible)
                CustomersGrid.Visibility = Visibility.Visible;
            else
                CustomersGrid.Visibility = Visibility.Collapsed;

            if (VendorsVisible)
                VendorsGrid.Visibility = Visibility.Visible;
            else
                VendorsGrid.Visibility = Visibility.Collapsed;

            if (SearchVisible)
                SearchGrid.Visibility = Visibility.Visible;
            else
                SearchGrid.Visibility = Visibility.Collapsed;

            if (InventoryVisible)
                InventoryGrid.Visibility = Visibility.Visible;
            else
                InventoryGrid.Visibility = Visibility.Collapsed;

            if (HistoryVisible)
                HistoryGrid.Visibility = Visibility.Visible;
            else
                HistoryGrid.Visibility = Visibility.Collapsed;
        }


        public MainPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.InitializeComponent();
            SetVisibility(false, false, false, false, false, false, false, false, false, false, false);
            BindPivot1();
            BindPivot2();
            BindPivot3();
            BindUnitsList();
            BindCategoriesList();
            BindWarehousesList();
            BindCustomersList();
            BindVendorsList();
            BindReleaseHeaderList();
            BindReceiveHeaderList();
            BindInventoryHeaderList();
            BindHistoryList();
            BindItemCB();

        }


        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;

        }

        private void ItemsButton_Click(object sender, RoutedEventArgs e)
        {

            MySplitView.IsPaneOpen = false;
            SetVisibility(true, false, false, false, false, false, false, false, false, false, false);

        }
        private void ReleaseListButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, true, false, false, false, false, false, false, false, false, false);

        }

        private void ReceiveListButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, true, false, false, false, false, false, false, false, false);

        }

        private void UnitsButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, false, true, false, false, false, false, false, false, false);

        }

        private void CategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, false, false, true, false, false, false, false, false, false);

        }

        private void WarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, false, false, false, true, false, false, false, false, false);

        }

        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, false, false, false, false, true, false, false, false, false);
        }

        private void VendorButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, false, false, false, false, false, true, false, false, false);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, false, false, false, false, false, false, true, false, false);

        }

        private void InventButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, false, false, false, false, false, false, false, true, false);

        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            SetVisibility(false, false, false, false, false, false, false, false, false, false, true);

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemPage), new Item());
        }

        private void AddUnitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UnitAddPage), new StockKeepUnit());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


            if (string.Equals(e.Parameter, "BackToItemList"))
            {
                BindPivot1();
                SetVisibility(true, false, false, false, false, false, false, false, false, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }

            if (string.Equals(e.Parameter, "BackToUnitsList"))
            {
                BindUnitsList();
                SetVisibility(false, false, false, true, false, false, false, false, false, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }

            if (string.Equals(e.Parameter, "BackToCategoriesList"))
            {
                BindCategoriesList();
                SetVisibility(false, false, false, false, true, false, false, false, false, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }

            if (string.Equals(e.Parameter, "BackToWarehousesList"))
            {
                BindWarehousesList();
                SetVisibility(false, false, false, false, false, true, false, false, false, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }

            if (string.Equals(e.Parameter, "BackToReleaseHeaderList"))
            {
                BindReleaseHeaderList();
                SetVisibility(false, true, false, false, false, false, false, false, false, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }

            if (string.Equals(e.Parameter, "BackToReceiveHeaderList"))
            {
                BindReceiveHeaderList();
                SetVisibility(false, false, true, false, false, false, false, false, false, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }

            if (string.Equals(e.Parameter, "BackToInventoryHeaderList"))
            {
                BindInventoryHeaderList();
                SetVisibility(false, false, false, false, false, false, false, false, false, true, false);
                MySplitView.IsPaneOpen = false;
                return;
            }

            if (string.Equals(e.Parameter, "BackToCustomerList"))
            {
                BindCustomersList();
                SetVisibility(false, false, false, false, false, false, true, false, false, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }

            if (string.Equals(e.Parameter, "BackToVendorList"))
            {
                BindVendorsList();
                SetVisibility(false, false, false, false, false, false, false, true, false, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }
            if (string.Equals(e.Parameter, "BackToSearchList"))
            {

                SetVisibility(false, false, false, false, false, false, false, false, true, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }
            if (SetupScanningResult(e.Parameter.ToString()))
            {
                SetVisibility(false, false, false, false, false, false, false, false, true, false, false);
                MySplitView.IsPaneOpen = false;
                return;
            }



        }

        private bool SetupScanningResult(string barcode)
        {
            if (!String.IsNullOrEmpty(barcode))
            {
                using (DBContext dbCtx = new DBContext())
                {
                    ItemStockKeepUnit isku = dbCtx.ItemStockKeepUnits.FirstOrDefault(x => x.Barcode == barcode);

                    ItemSearchCb.SelectedIndex = ItemColl.IndexOf(ItemColl.FirstOrDefault(x => x.ItemID == isku.ItemID));
                    CurrItem = ItemSearchCb.SelectedItem as Item;

                    ISKUCb.SelectedIndex = ItemStockKeepUnitsColl.IndexOf(ItemStockKeepUnitsColl.FirstOrDefault(x => x.Code == isku.Code));
                    BindSearchList();

                    return true;
                }
            }

            return false;
        }

        private void MyList_Tapped(object sender, TappedRoutedEventArgs e)
        {

            Item item = MyList.SelectedItem as Item;
            Frame.Navigate(typeof(ItemPage), item);

        }

        private void UnitsList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StockKeepUnit stockKeepUnit = UnitsList.SelectedItem as StockKeepUnit;
            Frame.Navigate(typeof(DetailsEditUnitsPage), stockKeepUnit);
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CategoryPage), new Category());
        }

        private void CategoriesList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Category category = CategoriesList.SelectedItem as Category;
            Frame.Navigate(typeof(CategoryPage), category);
        }


        private void AddWarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WarehousePage), new Warehouse());
        }

        private void WarehousesList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Warehouse warehouse = WarehousesList.SelectedItem as Warehouse;
            Frame.Navigate(typeof(WarehousePage), warehouse);
        }

        private void AddReleaseHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReleaseHeaderPage), new ReleaseHeader());
        }

        private void ReleaseHeaderList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ReleaseHeader releaseHeader = ReleaseHeaderList.SelectedItem as ReleaseHeader;
            Frame.Navigate(typeof(ReleaseHeaderPage), releaseHeader);
        }

        private void AddReceiveHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReceiveHeaderPage), new ReceiveHeader());
        }

        private void ReceiveHeaderList_Tapped(object sender, TappedRoutedEventArgs e)
        {

            ReceiveHeader receiveHeader = ReceiveHeaderList.SelectedItem as ReceiveHeader;
            Frame.Navigate(typeof(ReceiveHeaderPage), receiveHeader);
        }

        private void AddInventoryHeaderButton_Click(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(InventoryHeaderPage), new InventoryHeader());
        }

        private void InventoryHeaderList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            InventoryHeader inventoryHeader = InventoryHeaderList.SelectedItem as InventoryHeader;
            Frame.Navigate(typeof(InventoryHeaderPage), inventoryHeader);
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CustomerPage), new Customer());
        }

        private void CustomersList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Customer customer = CustomersList.SelectedItem as Customer;
            Frame.Navigate(typeof(CustomerPage), customer);
        }

        private void AddVendorButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VendorPage), new Vendor());
        }

        private void VendorsList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Vendor vendor = VendorsList.SelectedItem as Vendor;
            Frame.Navigate(typeof(VendorPage), vendor);
        }


        private void SearchHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            BindHistoryList();

        }

        private void ItemSearchCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrItem = ItemSearchCb.SelectedItem as Item;
            BindItemStockKeepUnitCB();

        }

        private void ISKUCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            BindSearchList();
        }

        
    }
}
