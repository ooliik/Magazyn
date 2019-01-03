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
   
    public sealed partial class UnitAddPage : Page
    {


        public UnitAddPage()
        {
            this.InitializeComponent();
        }

        private void BackAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), "BackToUnitsList");
        }

        private async void SaveUnitButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dBCtx = new DBContext())
            {

                var quantity = Convert.ToDouble(QuantityPerUnitTb.Text);
                if (quantity == 0 || String.IsNullOrEmpty(CodeTb.Text))
                {
                    MessageDialog message = new MessageDialog("Nie uzupełniono wszystkich pól.", "Operacja przerwana");
                    await message.ShowAsync();
                    return;
                }

                StockKeepUnit stockKeepUnit = new StockKeepUnit(CodeTb.Text, DescriptionTb.Text, quantity);
                dBCtx.StockKeepUnits.Add(stockKeepUnit);
                dBCtx.SaveChanges();
                this.Frame.Navigate(typeof(MainPage), "BackToUnitsList");
            }
        }
    }
}
