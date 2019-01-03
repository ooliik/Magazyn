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
using Windows.UI.Popups;
using System.Collections.ObjectModel;


namespace Inventory
{ 

    public sealed partial class ItemPage : Page
    {
        ObservableCollection<Category> CategoryColl;

        private void BindCategoryCB()
        {
            using (DBContext dbCtx = new DBContext())
            {

                CategoryColl = new ObservableCollection<Category>();

                var Categories = dbCtx.Categories.ToList();
                foreach (Category category in Categories)
                {
                    CategoryColl.Add(category);
                }

                CategorySelectCb.ItemsSource = CategoryColl;
            }
        }

        public ItemPage()
        {


            this.InitializeComponent();
            BindCategoryCB();

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                Item item = (Item)e.Parameter;
                if (item.ItemID != null)
                {
                    ItemIDTb.Text = item.ItemID;
                    NameTb.Text = item.Name;
                    DescriptionTb.Text = item.Description;

                    Category category = dbCtx.Categories.Single(x => x.CategoryID == item.CategoryID);
                    if (category != null)
                    {
                        CategorySelectCb.SelectedIndex = CategoryColl.IndexOf(CategoryColl.FirstOrDefault(x => x.CategoryID == item.CategoryID));

                    }
                }
                else
                {
                    item = new Item("");
                    ItemIDTb.Text = item.ItemID;
                    dbCtx.Items.Add(item);
                    dbCtx.SaveChanges();
                }



            }


        }


        private async void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                Item item = dbCtx.Items.First(x => x.ItemID == ItemIDTb.Text);

                dbCtx.Attach(item);
                var cat = CategorySelectCb.SelectedItem as Category;
                if (cat != null)
                {
                    item.Name = NameTb.Text;
                    item.Description = DescriptionTb.Text;
                    item.CategoryID = cat.CategoryID;

                    string IsValidMsg = InvMgt.isItemValid(item);
                    if (String.IsNullOrEmpty(IsValidMsg))
                    {
                        dbCtx.SaveChanges();
                        this.Frame.Navigate(typeof(MainPage), "BackToItemList");
                    }
                    else
                    {
                        MessageDialog message = new MessageDialog(IsValidMsg, "Operacja przerwana");
                        await message.ShowAsync();
                    }
                }
            }


        }

        private async void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {
            Item item;
            using (DBContext dbCtx = new DBContext())
            {
                item = dbCtx.Items.First(x => x.ItemID == ItemIDTb.Text);
            }
            string errorMsg = InvMgt.isItemAvaibleToDelete(item);
            if (String.IsNullOrEmpty(errorMsg))
            {
                using (DBContext dbCtx = new DBContext())
                {
                    dbCtx.Items.Remove(item);
                    dbCtx.SaveChanges();
                    this.Frame.Navigate(typeof(MainPage), "BackToItemList");
                }
            }
            else
            {
                MessageDialog message = new MessageDialog(errorMsg, "Operacja przerwana");
                await message.ShowAsync();
            }
        }


        private async void SKUEditButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBContext dbCtx = new DBContext())
            {
                Item item = dbCtx.Items.First(x => x.ItemID == ItemIDTb.Text);

                dbCtx.Attach(item);
                var cat = CategorySelectCb.SelectedItem as Category;
                if (cat != null)
                {
                    item.Name = NameTb.Text;
                    item.Description = DescriptionTb.Text;
                    item.CategoryID = cat.CategoryID;

                    string IsValidMsg = InvMgt.isItemValid(item);
                    if (String.IsNullOrEmpty(IsValidMsg))
                    {
                        dbCtx.SaveChanges();
                        this.Frame.Navigate(typeof(SKUAddEditDelPage), item);
                    }
                    else
                    {
                        MessageDialog message = new MessageDialog(IsValidMsg, "Operacja przerwana");
                        await message.ShowAsync();
                    }
                }

                else
                {
                    MessageDialog message = new MessageDialog("Nie wybrano kategorii", "Operacja przerwana");
                    await message.ShowAsync();
                    return;
                }
            }
        }
    }

}
