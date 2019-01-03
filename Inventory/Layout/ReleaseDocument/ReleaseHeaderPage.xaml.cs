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

    public sealed partial class ReleaseHeaderPage : Page
    {
        DBContext dbCtx = new DBContext();

        ReleaseHeader releaseHeader;

        ObservableCollection<Customer> CustomerColl;

        private void BindCustomerCB()
        {
            CustomerColl = new ObservableCollection<Customer>();

            var Customers = dbCtx.Customers.ToList();
            foreach (Customer customer in Customers)
            {
                CustomerColl.Add(customer);
            }

            CustomerIDCb.ItemsSource = CustomerColl;
        }

        private void BindReleaseLineList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.ReleaseLines.Where(x => x.DocumentID == releaseHeader.DocumentID).OrderBy(x => x.PositionNumber).AsEnumerable();
                ReleaseLineList.ItemsSource = list;
                ReleaseLineList.Visibility = Visibility.Visible;
            }
        }

        public ReleaseHeaderPage()
        {
            this.InitializeComponent();
            BindCustomerCB();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                releaseHeader = (ReleaseHeader)e.Parameter;
                if (releaseHeader.DocumentID != null)
                {
                    releaseHeader = dbCtx.ReleaseHeaders.First(x => x.DocumentID == releaseHeader.DocumentID);
                    DocumentIDTb.Text = releaseHeader.DocumentID;
                    DescriptionTb.Text = releaseHeader.Description;
                    ReleaseDateDP.Date = releaseHeader.ReleaseDate;

                    Customer customer = dbCtx.Customers.Single(x => x.CustomerID == releaseHeader.CustomerID);
                    if (customer != null)
                    {

                        CustomerIDCb.SelectedIndex = CustomerColl.IndexOf(CustomerColl.FirstOrDefault(x => x.CustomerID == releaseHeader.CustomerID));

                    }
                    dbCtx.ReleaseHeaders.Attach(releaseHeader);
                    BindReleaseLineList();

                }
                else
                {
                    releaseHeader = new ReleaseHeader("");
                    DocumentIDTb.Text = releaseHeader.DocumentID;
                    ReleaseDateDP.Date = DateTime.Today;
                    dbCtx.ReleaseHeaders.Add(releaseHeader);
                    dbCtx.SaveChanges();
                }
            }
        }



        private async void SaveReleaseHeaderButton_Click(object sender, RoutedEventArgs e)
        {

            using (DBContext dbCtx = new DBContext())
            {
                ReleaseHeader releaseHeader = dbCtx.ReleaseHeaders.First(x => x.DocumentID == DocumentIDTb.Text);
                dbCtx.Attach(releaseHeader);


                var customer = CustomerIDCb.SelectedItem as Customer;
                if (customer != null)
                {

                    releaseHeader.DocumentID = DocumentIDTb.Text;
                    releaseHeader.Description = DescriptionTb.Text;
                    releaseHeader.ReleaseDate = Convert.ToDateTime(ReleaseDateDP.Date.ToString());
                    releaseHeader.CustomerID = customer.CustomerID;
                    string errorMsg = InvMgt.isReleaseHeaderValid(releaseHeader);
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        dbCtx.SaveChanges();
                        this.Frame.Navigate(typeof(MainPage), "BackToReleaseHeaderList");
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

        private void ReleaseLineList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ReleaseLine releaseLine = ReleaseLineList.SelectedItem as ReleaseLine;
            Frame.Navigate(typeof(ReleaseLineEditPage), releaseLine);
        }

        private async void AddReleaseLineButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                ReleaseHeader releaseHeader = dbCtx.ReleaseHeaders.First(x => x.DocumentID == DocumentIDTb.Text);
                dbCtx.Attach(releaseHeader);


                var customer = CustomerIDCb.SelectedItem as Customer;
                if (customer != null)
                {

                    releaseHeader.DocumentID = DocumentIDTb.Text;
                    releaseHeader.Description = DescriptionTb.Text;
                    releaseHeader.ReleaseDate = Convert.ToDateTime(ReleaseDateDP.Date.ToString());
                    releaseHeader.CustomerID = customer.CustomerID;
                    string errorMsg = InvMgt.isReleaseHeaderValid(releaseHeader);
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        dbCtx.SaveChanges();
                        Frame.Navigate(typeof(ReleaseLineAddPage), new ReleaseLine(releaseHeader.DocumentID));
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
                    MessageDialog message = new MessageDialog("Nie wybrano klienta", "Operacja przerwana");
                    await message.ShowAsync();
                    return;
                }
            }

        }

        private void CustomerIDCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void PostReleaseHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = null;
            errorMsg = InvMgt.postReleaseHeader(releaseHeader);
            if (!String.IsNullOrEmpty(errorMsg))
            {
                MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                await message.ShowAsync();
                return;
            }
            BindReleaseLineList();
        }

       
    }
}
