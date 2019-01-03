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

    public sealed partial class InventoryHeaderPage : Page
    {
        ObservableCollection<Warehouse> WarehousesColl;
        InventoryHeader inventoryHeader;
        InventoryLine inventoryLine;


        private void BindInventoryList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.InventoryLines.Where(x => x.DocumentID == DocumentIDTb.Text).AsEnumerable();
                InventoryLineList.ItemsSource = list;
                InventoryLineList.Visibility = Visibility.Visible;
            }
        }




        private void BindWarehouseNumberCB()
        {
            WarehousesColl = new ObservableCollection<Warehouse>();

            using (DBContext dbCtx = new DBContext())
            {
                var Warehouses = dbCtx.Warehouses.ToList();
                foreach (Warehouse warehouse in Warehouses)
                {
                    WarehousesColl.Add(new Warehouse(warehouse));
                }
            }
            WarehouseNameCb.ItemsSource = WarehousesColl;
        }

        public InventoryHeaderPage()
        {
            this.InitializeComponent();
            BindInventoryList();
            BindWarehouseNumberCB();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                inventoryHeader = (InventoryHeader)e.Parameter;

                if (inventoryHeader.DocumentID != null)
                {
                    inventoryHeader = dbCtx.InventoryHeaders.First(x => x.DocumentID == inventoryHeader.DocumentID);

                    DocumentIDTb.Text = inventoryHeader.DocumentID;
                    DescriptionTb.Text = inventoryHeader.Description;


                    Warehouse warehouse = dbCtx.Warehouses.Single(x => x.WarehouseName == inventoryHeader.WarehouseName);
                    if (warehouse != null)
                    {
                        WarehouseNameCb.SelectedIndex = WarehousesColl.IndexOf(WarehousesColl.FirstOrDefault(x => x.WarehouseName == inventoryHeader.WarehouseName));
                    }

                    InventoryDateDP.Date = inventoryHeader.InventoryDate;


                    dbCtx.InventoryHeaders.Attach(inventoryHeader);
                    BindInventoryList();
                }
                else
                {
                    inventoryHeader = new InventoryHeader("");
                    DocumentIDTb.Text = inventoryHeader.DocumentID;
                    dbCtx.InventoryHeaders.Add(inventoryHeader);
                    dbCtx.SaveChanges();
                }

            }
        }



        private async void SaveInventoryHeaderButton_Click(object sender, RoutedEventArgs e)
        {

            using (DBContext dbCtx = new DBContext())
            {
                InventoryHeader inventoryHeader = dbCtx.InventoryHeaders.First(x => x.DocumentID == DocumentIDTb.Text);
                dbCtx.Attach(inventoryHeader);

                var warehouse = WarehouseNameCb.SelectedItem as Warehouse;
                if (warehouse != null)
                {
                    inventoryHeader.DocumentID = DocumentIDTb.Text;
                    inventoryHeader.Description = DescriptionTb.Text;
                    inventoryHeader.InventoryDate = Convert.ToDateTime(InventoryDateDP.Date.ToString());
                    inventoryHeader.WarehouseName = warehouse.WarehouseName;

                    string errorMsg = InvMgt.isInventoryHeaderValid(inventoryHeader);
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        dbCtx.SaveChanges();
                        this.Frame.Navigate(typeof(MainPage), "BackToInventoryHeaderList");
                    }
                    else
                    {
                        MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                        await message.ShowAsync();
                        return;
                    }
                }
            }
        }


        private async void GenerateInventoryLines()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var warehouse = WarehouseNameCb.SelectedItem as Warehouse;
                if (warehouse == null)
                {

                    MessageDialog message = new MessageDialog("Nie wybrano magazynu.", "Operacja przerwana");
                    await message.ShowAsync();
                }
                else
                {
                    var invLineList = dbCtx.InventoryLines.Where(x => x.DocumentID == DocumentIDTb.Text);
                    foreach (var line in invLineList)
                    {
                        dbCtx.InventoryLines.Remove(line);
                    }

                    dbCtx.SaveChanges();

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
                  .Where(x => x.WarehouseNumber == warehouse.WarehouseName)
                  .ToList();
                    int Positon = 1;
                    List<InventoryLine> InventoryLineLst = new List<InventoryLine>();



                    foreach (var entry in result)
                    {
                        if (entry.Quantity > 0)
                        {
                            InventoryLineLst.Add(new InventoryLine(DocumentIDTb.Text, Positon, entry.ItemID, entry.KeepUnit, entry.WarehouseNumber, entry.WarehousePlace, entry.Quantity, 0.0));

                            dbCtx.InventoryLines.Add(new InventoryLine(DocumentIDTb.Text, Positon, entry.ItemID, entry.KeepUnit, entry.WarehouseNumber, entry.WarehousePlace, entry.Quantity, 0.0));

                            Positon++;
                        }
                    }
                    InventoryLineList.ItemsSource = InventoryLineLst;
                    dbCtx.SaveChanges();
                }
            }
        }



        private void GenerateInventoryLinesButton_Click(object sender, RoutedEventArgs e)
        {

            GenerateInventoryLines();
        }

        private async void PostByButton_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = InvMgt.PostInventory(inventoryHeader);
            if (!String.IsNullOrEmpty(errorMsg))
            {
                MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                await message.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(MainPage), "BackToInventoryHeaderList");
            }
        }

        
    }
}

