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
    public sealed partial class ReceiveHeaderPage : Page
    {
        ReceiveHeader receiveHeader;
        ObservableCollection<Vendor> VendorColl;

        private void BindVendorCB()
        {
            using (DBContext dbCtx = new DBContext())
            {
                VendorColl = new ObservableCollection<Vendor>();

                var Vendors = dbCtx.Vendors.ToList();
                foreach (Vendor vendor in Vendors)
                {
                    VendorColl.Add(vendor);
                }
            }
            VendorIDCb.ItemsSource = VendorColl;
        }

        private void BindReceiveLineList()
        {
            using (DBContext dbCtx = new DBContext())
            {
                var list = dbCtx.ReceiveLines.Where(x => x.DocumentID == receiveHeader.DocumentID).OrderBy(x => x.PositionNumber).AsEnumerable();
                ReceiveLineList.ItemsSource = list;
                ReceiveLineList.Visibility = Visibility.Visible;
            }
        }

        public ReceiveHeaderPage()
        {
            this.InitializeComponent();
            BindVendorCB();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


            using (DBContext dbCtx = new DBContext())
            {
                receiveHeader = (ReceiveHeader)e.Parameter;

                if (receiveHeader.DocumentID != null)
                {
                    receiveHeader = dbCtx.ReceiveHeaders.First(x => x.DocumentID == receiveHeader.DocumentID);
                    DocumentIDTb.Text = receiveHeader.DocumentID;
                    DescriptionTb.Text = receiveHeader.Description;
                    ReceiveDateDP.Date = receiveHeader.ReceiveDate;

                    Vendor vendor = dbCtx.Vendors.Single(x => x.VendorID == receiveHeader.VendorID);
                    if (vendor != null)
                    {

                        VendorIDCb.SelectedIndex = VendorColl.IndexOf(VendorColl.FirstOrDefault(x => x.VendorID == receiveHeader.VendorID));

                    }
                    dbCtx.ReceiveHeaders.Attach(receiveHeader);
                    BindReceiveLineList();

                }
                else
                {
                    receiveHeader = new ReceiveHeader("");
                    DocumentIDTb.Text = receiveHeader.DocumentID;
                    ReceiveDateDP.Date = DateTime.Today;
                    dbCtx.ReceiveHeaders.Add(receiveHeader);
                    dbCtx.SaveChanges();
                }


            }

        }



        private async void SaveReceiveHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                ReceiveHeader receiveHeader = dbCtx.ReceiveHeaders.First(x => x.DocumentID == DocumentIDTb.Text);
                dbCtx.Attach(receiveHeader);


                var vendor = VendorIDCb.SelectedItem as Vendor;
                if (vendor != null)
                {

                    receiveHeader.DocumentID = DocumentIDTb.Text;
                    receiveHeader.Description = DescriptionTb.Text;
                    receiveHeader.ReceiveDate = Convert.ToDateTime(ReceiveDateDP.Date.ToString());
                    receiveHeader.VendorID = vendor.VendorID;
                    string errorMsg = InvMgt.isReceiveHeaderValid(receiveHeader);
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        dbCtx.SaveChanges();
                        this.Frame.Navigate(typeof(MainPage), "BackToReceiveHeaderList");
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

        private async void AddReceiveLineButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                ReceiveHeader receiveHeader = dbCtx.ReceiveHeaders.First(x => x.DocumentID == DocumentIDTb.Text);
                dbCtx.Attach(receiveHeader);


                var vendor = VendorIDCb.SelectedItem as Vendor;
                if (vendor != null)
                {

                    receiveHeader.DocumentID = DocumentIDTb.Text;
                    receiveHeader.Description = DescriptionTb.Text;
                    receiveHeader.ReceiveDate = Convert.ToDateTime(ReceiveDateDP.Date.ToString());
                    receiveHeader.VendorID = vendor.VendorID;
                    string errorMsg = InvMgt.isReceiveHeaderValid(receiveHeader);
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        dbCtx.SaveChanges();
                        Frame.Navigate(typeof(ReceiveLineAddPage), new ReceiveLine(receiveHeader.DocumentID));
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
                    MessageDialog message = new MessageDialog("Nie wybrano sprzedawcy", "Operacja przerwana");
                    await message.ShowAsync();
                    return;
                }
            }

        }

        private void ReceiveLineList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ReceiveLine receiveLine = ReceiveLineList.SelectedItem as ReceiveLine;
            Frame.Navigate(typeof(ReceiveLineEditPage), receiveLine);
        }

        private void VendorIDCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void PostReceiveHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = null;
            errorMsg = InvMgt.postReceiveHeader(receiveHeader);
            if (!String.IsNullOrEmpty(errorMsg))
            {
                MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                await message.ShowAsync();
                return;
            }
            BindReceiveLineList();
        }

        
    }
}
