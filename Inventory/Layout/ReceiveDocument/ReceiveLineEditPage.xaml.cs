using Inventory.Models;
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



namespace Inventory
{

    public sealed partial class ReceiveLineEditPage : Page
    {
        ReceiveLine receiveLine;
        ReceiveHeader receiveHeader;

        public ReceiveLineEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                receiveLine = (ReceiveLine)e.Parameter;
                receiveHeader = dbCtx.ReceiveHeaders.First(x => x.DocumentID == receiveLine.DocumentID);

                DocumentIDTb.Text = receiveLine.DocumentID;

                QuantityTb.Text = receiveLine.Quantity.ToString();

                ReceiveQuantityTb.Text = receiveLine.ReceiveQuantity.ToString();
                ReceivedQuantityTb.Text = receiveLine.ReceivedQuantity.ToString();


            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReceiveHeaderPage), receiveHeader);
        }

        private void SaveReceiveLineButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                receiveLine = dbCtx.ReceiveLines.First(x => x.DocumentID == receiveLine.DocumentID && x.PositionNumber == receiveLine.PositionNumber);
                dbCtx.ReceiveLines.Attach(receiveLine);
                receiveLine.Quantity = Convert.ToDouble(QuantityTb.Text);
                receiveLine.ReceiveQuantity = Convert.ToDouble(ReceiveQuantityTb.Text);
                receiveLine.ReceivedQuantity = Convert.ToDouble(ReceivedQuantityTb.Text);

                dbCtx.SaveChanges();
            }
            this.Frame.Navigate(typeof(ReceiveHeaderPage), receiveHeader);
        }

        private void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                dbCtx.ReceiveLines.Remove(receiveLine);
                dbCtx.SaveChanges();
            }
            this.Frame.Navigate(typeof(ReceiveHeaderPage), receiveHeader);
        }

    }
}
