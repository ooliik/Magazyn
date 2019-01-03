using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
using System.ComponentModel.DataAnnotations;
using Windows.UI.Core;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;

namespace Inventory
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            using (var db = new DBContext())
            {
                db.Database.Migrate();

            }
            InitTestData();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;

               
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }
        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        public void InitTestData()
        {

            Category category;
            Item item;
            StockKeepUnit stockKeepUnit;
            ItemStockKeepUnit itemStockKeepUnit;
            Warehouse warehouse;
            WarehouseEntry warehouseEntry;
            WarehousePlace warehousePlace;
            Customer customer;
            Vendor vendor;
            ReleaseHeader releaseHeader;
            ReleaseLine releaseLine;
            ReceiveHeader receiveHeader;
            ReceiveLine receiveLine;
            InventoryHeader inventoryHeader;
            InventoryLine inventoryLine;
            Serie serie;

            using (DBContext dbCtx = new DBContext())
            {
                serie = new Serie("ItemSerie", "PROD", "00003", TableType.Item);
                if (!dbCtx.Series.Any(u => u.SerieID == serie.SerieID))
                    dbCtx.Series.Add(serie);
                serie = new Serie("CustomerSerie", "KLIENT", "00002", TableType.Customer);
                if (!dbCtx.Series.Any(u => u.SerieID == serie.SerieID))
                    dbCtx.Series.Add(serie);
                serie = new Serie("VendorSerie", "DOSTAW", "00002", TableType.Vendor);
                if (!dbCtx.Series.Any(u => u.SerieID == serie.SerieID))
                    dbCtx.Series.Add(serie);
                serie = new Serie("ReceiveSerie", "PZ/18/", "00000", TableType.ReceiveHeader);
                if (!dbCtx.Series.Any(u => u.SerieID == serie.SerieID))
                    dbCtx.Series.Add(serie);
                serie = new Serie("ReleaseSerie", "WZ/18/", "00000", TableType.ReleaseHeader);
                if (!dbCtx.Series.Any(u => u.SerieID == serie.SerieID))
                    dbCtx.Series.Add(serie);
                serie = new Serie("InventorySerie", "INV/18/", "00000", TableType.InventoryHeader);
                if (!dbCtx.Series.Any(u => u.SerieID == serie.SerieID))
                    dbCtx.Series.Add(serie);


                category = new Category("HERBATY", "Herbaty liściaste i w torebkach");
                if (!dbCtx.Categories.Any(u => u.CategoryID == category.CategoryID))
                    dbCtx.Add(category);



                category = new Category("SOKI I NAPOJE", "Soki, nektary, napoje niegazowane");
                if (!dbCtx.Categories.Any(u => u.CategoryID == category.CategoryID))
                    dbCtx.Add(category);

                dbCtx.SaveChanges();
            }

            item = new Item("PROD00001", "Tetley Intensive", "Earl Grey", "HERBATY");
            using (DBContext dbCtx = new DBContext())
            {
                if (!dbCtx.Items.Any(u => u.ItemID == item.ItemID))
                    dbCtx.Add(item);
                dbCtx.SaveChanges();
            }

            item = new Item("PROD00002", "TET British Empire", "Czarna", "HERBATY");
            using (DBContext dbCtx = new DBContext())
            {
                if (!dbCtx.Items.Any(u => u.ItemID == item.ItemID))
                    dbCtx.Add(item);
                dbCtx.SaveChanges();
            }

            item = new Item("PROD00003", "Tymbark 1L", "Różne smaki", "SOKI I NAPOJE");
            using (DBContext dbCtx = new DBContext())
            {
                if (!dbCtx.Items.Any(u => u.ItemID == item.ItemID))
                    dbCtx.Add(item);
                dbCtx.SaveChanges();
            }




            using (DBContext dbCtx = new DBContext())
            {
                //stockKeepUnit = new StockKeepUnit("OP12", "Opakowanie 12 sztuk", 12.00);
                //if (!dbCtx.StockKeepUnits.Any(u => u.Code == stockKeepUnit.Code))
                //    dbCtx.StockKeepUnits.Add(stockKeepUnit);
                //stockKeepUnit = new StockKeepUnit("OP6", "Opakowanie 6 sztuk", 6.00);
                //if (!dbCtx.StockKeepUnits.Any(u => u.Code == stockKeepUnit.Code))
                //    dbCtx.StockKeepUnits.Add(stockKeepUnit);
                //stockKeepUnit = new StockKeepUnit("SZT", "1 sztuka", 1.00);
                //if (!dbCtx.StockKeepUnits.Any(u => u.Code == stockKeepUnit.Code))
                //    dbCtx.StockKeepUnits.Add(stockKeepUnit);

                //itemStockKeepUnit = new ItemStockKeepUnit("PROD00001", "SZT", "5900956100593");
                //if (!dbCtx.ItemStockKeepUnits.Any(u => u.ItemID == itemStockKeepUnit.ItemID && u.Code == itemStockKeepUnit.Code))
                //    dbCtx.ItemStockKeepUnits.Add(itemStockKeepUnit);
                //itemStockKeepUnit = new ItemStockKeepUnit("PROD00002", "OP12", "5060207694155");
                //if (!dbCtx.ItemStockKeepUnits.Any(u => u.ItemID == itemStockKeepUnit.ItemID && u.Code == itemStockKeepUnit.Code))
                //    dbCtx.ItemStockKeepUnits.Add(itemStockKeepUnit);
                //itemStockKeepUnit = new ItemStockKeepUnit("PROD00003", "SZT", "5900497112383");
                //if (!dbCtx.ItemStockKeepUnits.Any(u => u.ItemID == itemStockKeepUnit.ItemID && u.Code == itemStockKeepUnit.Code))
                //    dbCtx.ItemStockKeepUnits.Add(itemStockKeepUnit);
                //itemStockKeepUnit = new ItemStockKeepUnit("PROD00001", "OP6", "5410033851336");
                //if (!dbCtx.ItemStockKeepUnits.Any(u => u.ItemID == itemStockKeepUnit.ItemID && u.Code == itemStockKeepUnit.Code))
                //    dbCtx.ItemStockKeepUnits.Add(itemStockKeepUnit);

                warehouse = new Warehouse("M001", "Magazyn pierwszy", "Czestochowa, Zielona 20");
                if (!dbCtx.Warehouses.Any(u => u.WarehouseName == warehouse.WarehouseName))
                    dbCtx.Warehouses.Add(warehouse);

                warehouse = new Warehouse("M002", "Magazyn drugi", "Czestochowa, Zielona 20");
                if (!dbCtx.Warehouses.Any(u => u.WarehouseName == warehouse.WarehouseName))
                    dbCtx.Warehouses.Add(warehouse);

                //warehouseEntry = new WarehouseEntry(1, "PROD00001", "D0001", EntryType.Receive,12.00, 12.00, 1.00,"SZT", "M001", "P001");
                //if (!dbCtx.WarehouseEntries.Any(u => u.EntryNumber == warehouseEntry.EntryNumber))
                //    dbCtx.WarehouseEntries.Add(warehouseEntry);
                //warehouseEntry = new WarehouseEntry(2, "PROD00002", "D0002", EntryType.Receive,12.00, 1.00, 12.00, "OP12", "M001", "P002");
                //if (!dbCtx.WarehouseEntries.Any(u => u.EntryNumber == warehouseEntry.EntryNumber))
                //    dbCtx.WarehouseEntries.Add(warehouseEntry);
                //warehouseEntry = new WarehouseEntry(3, "PROD00003", "D0003", EntryType.Receive, 6.00, 6.00, 1.00, "SZT", "M001", "P003");
                //if (!dbCtx.WarehouseEntries.Any(u => u.EntryNumber == warehouseEntry.EntryNumber))
                //    dbCtx.WarehouseEntries.Add(warehouseEntry);
                //warehouseEntry = new WarehouseEntry(4, "PROD00001", "D0004", EntryType.Receive,6.00, 1.00, 6.00, "OP6", "M001", "P004");
                //if (!dbCtx.WarehouseEntries.Any(u => u.EntryNumber == warehouseEntry.EntryNumber))
                //    dbCtx.WarehouseEntries.Add(warehouseEntry);

                warehousePlace = new WarehousePlace("M001", "P001");
                if (!dbCtx.WarehousePlaces.Any(u => u.WarehouseName == warehousePlace.WarehouseName && u.WarehousePlaceName == warehousePlace.WarehousePlaceName))
                    dbCtx.WarehousePlaces.Add(warehousePlace);

                warehousePlace = new WarehousePlace("M001", "P002");
                if (!dbCtx.WarehousePlaces.Any(u => u.WarehouseName == warehousePlace.WarehouseName && u.WarehousePlaceName == warehousePlace.WarehousePlaceName))
                    dbCtx.WarehousePlaces.Add(warehousePlace);

                warehousePlace = new WarehousePlace("M001", "P003");
                if (!dbCtx.WarehousePlaces.Any(u => u.WarehouseName == warehousePlace.WarehouseName && u.WarehousePlaceName == warehousePlace.WarehousePlaceName))
                    dbCtx.WarehousePlaces.Add(warehousePlace);


                warehousePlace = new WarehousePlace("M002", "P001");
                if (!dbCtx.WarehousePlaces.Any(u => u.WarehouseName == warehousePlace.WarehouseName && u.WarehousePlaceName == warehousePlace.WarehousePlaceName))
                    dbCtx.WarehousePlaces.Add(warehousePlace);

                warehousePlace = new WarehousePlace("M002", "P002");
                if (!dbCtx.WarehousePlaces.Any(u => u.WarehouseName == warehousePlace.WarehouseName && u.WarehousePlaceName == warehousePlace.WarehousePlaceName))
                    dbCtx.WarehousePlaces.Add(warehousePlace);

                warehousePlace = new WarehousePlace("M002", "P003");
                if (!dbCtx.WarehousePlaces.Any(u => u.WarehouseName == warehousePlace.WarehouseName && u.WarehousePlaceName == warehousePlace.WarehousePlaceName))
                    dbCtx.WarehousePlaces.Add(warehousePlace);
                dbCtx.SaveChanges();



                customer = new Customer("KLIENT000001", "Delikatesy u mamy", "Ul. Opolska 24", "30-125 Kraków", "Polska");
                if (!dbCtx.Customers.Any(u => u.CustomerID == customer.CustomerID))
                    dbCtx.Customers.Add(customer);

                customer = new Customer("KLIENT000002", "Sklep spożywczy 'Danuta'", "Ul. Zielarska 59", "42-200 Częstochowa", "Polska");
                if (!dbCtx.Customers.Any(u => u.CustomerID == customer.CustomerID))
                    dbCtx.Customers.Add(customer);

                vendor = new Vendor("DOSTAW000001", "Spółdzielnia Mleczarska w Gostyniu", "ul. Wielkopolska 1", "63-800 Gostyń", "Polska");
                if (!dbCtx.Vendors.Any(u => u.VendorID == vendor.VendorID))
                    dbCtx.Vendors.Add(vendor);

                vendor = new Vendor("DOSTAW000002", "Zbyszko Company S.A.", "ul. Warszawska 239", "26-600 Radom", "Polska");
                if (!dbCtx.Vendors.Any(u => u.VendorID == vendor.VendorID))
                    dbCtx.Vendors.Add(vendor);
                dbCtx.SaveChanges();
            }
            /*
           releaseHeader = new ReleaseHeader("WZ/17/000001", "Wydanie zewnętrzne dla klienta", DateTime.Parse("12-11-2017"), "C001");
           dbCtx = new DBContext();
           if (!dbCtx.ReleaseHeaders.Any(u => u.DocumentID == releaseHeader.DocumentID))
               dbCtx.ReleaseHeaders.Add(releaseHeader);

           releaseHeader = new ReleaseHeader("WZ/17/000002", "Wydanie zewnętrzne dla klienta", DateTime.Parse("10-05-2017"), "C002");
           dbCtx = new DBContext();
           if (!dbCtx.ReleaseHeaders.Any(u => u.DocumentID == releaseHeader.DocumentID))
               dbCtx.ReleaseHeaders.Add(releaseHeader);

           releaseLine = new ReleaseLine("WZ/17/000001", 1, "N000003", "SZT", "M001", "P002", 8.00);
           dbCtx = new DBContext();
           if (!dbCtx.ReleaseLines.Any(u => u.DocumentID == releaseLine.DocumentID && u.PositionNumber == releaseLine.PositionNumber))
               dbCtx.ReleaseLines.Add(releaseLine);

           receiveHeader = new ReceiveHeader("PZ/17/000001", "Przyjęcie zewnętrzne od sprzedawcy", DateTime.Parse("12-11-2017"), "V001");
           dbCtx = new DBContext();
           if (!dbCtx.ReceiveHeaders.Any(u => u.DocumentID == receiveHeader.DocumentID))
               dbCtx.ReceiveHeaders.Add(receiveHeader);

           receiveHeader = new ReceiveHeader("PZ/17/000002", "Przyjęcie zewnętrzne od sprzedawcy", DateTime.Parse("09-03-2017"), "V002");
           dbCtx = new DBContext();
           if (!dbCtx.ReceiveHeaders.Any(u => u.DocumentID == receiveHeader.DocumentID))
               dbCtx.ReceiveHeaders.Add(receiveHeader);

           receiveLine = new ReceiveLine("PZ/17/000001", 1, "N000003", "SZT", "M001", "P002", 8.00);
           dbCtx = new DBContext();
           if (!dbCtx.ReceiveLines.Any(u => u.DocumentID == receiveLine.DocumentID && u.PositionNumber == receiveLine.PositionNumber))
               dbCtx.ReceiveLines.Add(receiveLine);

           inventoryHeader = new InventoryHeader("INV/17/000001", "Dokument inwentaryzacyjny", DateTime.Today, "M001");
           dbCtx = new DBContext();
           if (!dbCtx.InventoryHeaders.Any(u => u.DocumentID == inventoryHeader.DocumentID))
               dbCtx.InventoryHeaders.Add(inventoryHeader);

           inventoryHeader = new InventoryHeader("INV/17/000002", "Dokument inwentaryzacyjny", DateTime.Today, "M001");
           dbCtx = new DBContext();
           if (!dbCtx.InventoryHeaders.Any(u => u.DocumentID == inventoryHeader.DocumentID))
               dbCtx.InventoryHeaders.Add(inventoryHeader);

           inventoryLine = new InventoryLine("INV/17/000001", 1, "N000003", "SZT", "M001", "P002", 8.00, 0.0);
                if (!dbCtx.InventoryLines.Any(u => u.DocumentID == inventoryLine.DocumentID))
               dbCtx.InventoryLines.Add(inventoryLine);

           */

            //dbCtx.SaveChanges();

        }
    }
}
