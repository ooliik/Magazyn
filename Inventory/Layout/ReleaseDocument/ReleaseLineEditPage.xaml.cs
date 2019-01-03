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

    public sealed partial class ReleaseLineEditPage : Page
    {
        ReleaseLine releaseLine;
        ReleaseHeader releaseHeader;

        public ReleaseLineEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                releaseLine = (ReleaseLine)e.Parameter;
                releaseHeader = dbCtx.ReleaseHeaders.First(x => x.DocumentID == releaseLine.DocumentID);

                DocumentIDTb.Text = releaseLine.DocumentID;

                QuantityTb.Text = releaseLine.Quantity.ToString();

                ReleaseQuantityTb.Text = releaseLine.ReleaseQuantity.ToString();
                ReleasedQuantityTb.Text = releaseLine.ReleasedQuantity.ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReleaseHeaderPage), releaseHeader);
        }

        private void SaveReleaseLineButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                releaseLine = dbCtx.ReleaseLines.First(x => x.DocumentID == releaseLine.DocumentID && x.PositionNumber == releaseLine.PositionNumber);
                dbCtx.ReleaseLines.Attach(releaseLine);
                releaseLine.Quantity = Convert.ToDouble(QuantityTb.Text);
                releaseLine.ReleaseQuantity = Convert.ToDouble(ReleaseQuantityTb.Text);
                releaseLine.ReleasedQuantity = Convert.ToDouble(ReleasedQuantityTb.Text);

                dbCtx.SaveChanges();
            }
            this.Frame.Navigate(typeof(ReleaseHeaderPage), releaseHeader);
        }

        private void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                dbCtx.ReleaseLines.Remove(releaseLine);
                dbCtx.SaveChanges();
            }
            this.Frame.Navigate(typeof(ReleaseHeaderPage), releaseHeader);
        }
    }
}
