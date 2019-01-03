using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class Item
    {
        public Item()
        {

        }
        public Item(string ID)
        {
            ItemID = InvMgt.GetNextItemID();
        }

       

        public Item(string name, string description, string category)
        {
            ItemID = InvMgt.GetNextItemID();
            Name = name;
            Description = description;
            CategoryID = category;
        }

        public Item(string ID, string name, string description, string category)
        {
            ItemID = ID;
            Name = name;
            Description = description;
            CategoryID = category;
        }

        public string ItemID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryID { get; set; }

        public List<ItemStockKeepUnit> ItemStockKeepUnits { get; set; }
        public List<WarehouseEntry> WarehouseEnrties { get; set; }
        public List<ReleaseLine> ReleaseLines { get; set; }
        public List<ReceiveLine> ReceiveLines { get; set; }
        public List<InventoryLine> InventoryLines { get; set; }

        public Category Category { get; set; }
    }
}
