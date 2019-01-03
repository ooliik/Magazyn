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
    public sealed partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                Customer customer = (Customer)e.Parameter;

                if (customer.CustomerID != null)
                {
                    CustomerIDTb.Text = customer.CustomerID;
                    NameTb.Text = customer.Name;
                    AddressTb.Text = customer.Address;
                    CityTb.Text = customer.City;
                    CountryTb.Text = customer.Country;
                }
                else
                {
                    customer = new Customer("");
                    CustomerIDTb.Text = customer.CustomerID;
                    dbCtx.Customers.Add(customer);
                    dbCtx.SaveChanges();
                }

            }


        }

        private async void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer;
            using (DBContext dbCtx = new DBContext())
            {
                customer = dbCtx.Customers.First(x => x.CustomerID == CustomerIDTb.Text);
            }
            string errorMsg = InvMgt.isCustomerAvaibleToDelete(customer);
            if (String.IsNullOrEmpty(errorMsg))
            {
                using (DBContext dbCtx = new DBContext())
                {
                    dbCtx.Customers.Remove(customer);
                    dbCtx.SaveChanges();
                    this.Frame.Navigate(typeof(MainPage), "BackToCustomerList");
                }
            }
            else
            {
                MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                await message.ShowAsync();
            }
        }

        private async void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                Customer customer = dbCtx.Customers.First(x => x.CustomerID == CustomerIDTb.Text);

                dbCtx.Attach(customer);

                if (customer != null)
                {
                    customer.Name = NameTb.Text;
                    customer.Address = AddressTb.Text;
                    customer.City = CityTb.Text;
                    customer.Country = CountryTb.Text;

                    string IsValidMsg = InvMgt.isCustomerValid(customer);
                    if (String.IsNullOrEmpty(IsValidMsg))
                    {
                        dbCtx.SaveChanges();
                        this.Frame.Navigate(typeof(MainPage), "BackToCustomerList");
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
