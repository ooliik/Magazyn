using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class ItemStockKeepUnit
    {
        public ItemStockKeepUnit()
        {
        }

        public ItemStockKeepUnit(ItemStockKeepUnit itemStockKeepUnit)
        {
            ItemID = itemStockKeepUnit.ItemID;
            Code = itemStockKeepUnit.Code;
        }

        public ItemStockKeepUnit(string itemID, string code, string barcode)
        {
            ItemID = itemID;
            Code = code;
            Barcode = barcode;
        }


        public string ItemID { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }

        public Item Item { get; set; }
        public StockKeepUnit StockKeepUnit { get; set; }
    }
}
