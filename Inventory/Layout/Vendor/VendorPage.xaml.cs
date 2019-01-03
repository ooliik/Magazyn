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

    public sealed partial class VendorPage : Page
    {
        public VendorPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                Vendor vendor = (Vendor)e.Parameter;

                if (vendor.VendorID != null)
                {
                    VendorIDTb.Text = vendor.VendorID;
                    NameTb.Text = vendor.Name;
                    AddressTb.Text = vendor.Address;
                    CityTb.Text = vendor.City;
                    CountryTb.Text = vendor.Country;
                }
                else
                {
                    vendor = new Vendor("");
                    VendorIDTb.Text = vendor.VendorID;
                    dbCtx.Vendors.Add(vendor);
                    dbCtx.SaveChanges();
                }

            }


        }

        private async void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {
            Vendor vendor;
            using (DBContext dbCtx = new DBContext())
            {
                vendor = dbCtx.Vendors.First(x => x.VendorID == VendorIDTb.Text);
            }
            string errorMsg = InvMgt.isVendorAvaibleToDelete(vendor);
            if (String.IsNullOrEmpty(errorMsg))
            {
                using (DBContext dbCtx = new DBContext())
                {
                    dbCtx.Vendors.Remove(vendor);
                    dbCtx.SaveChanges();
                    this.Frame.Navigate(typeof(MainPage), "BackToVendorList");
                }
            }
            else
            {
                MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana.");
                await message.ShowAsync();
            }
        }

        private async void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                Vendor vendor = dbCtx.Vendors.First(x => x.VendorID == VendorIDTb.Text);

                dbCtx.Attach(vendor);

                if (vendor != null)
                {
                    vendor.Name = NameTb.Text;
                    vendor.Address = AddressTb.Text;
                    vendor.City = CityTb.Text;
                    vendor.Country = CountryTb.Text;

                    string IsValidMsg = InvMgt.isVendorValid(vendor);
                    if (String.IsNullOrEmpty(IsValidMsg))
                    {
                        dbCtx.SaveChanges();
                        this.Frame.Navigate(typeof(MainPage), "BackToVendorList");
                    }
                    else
                    {
                        MessageDialog message = new MessageDialog(IsValidMsg, "Operacja przerwana. Nie wszystkie dane zostały wprowadzone");
                        await message.ShowAsync();
                    }
                }
            }
        }
    }
}
