using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class InventoryLine
    {
        public InventoryLine()
        {
        }

        public InventoryLine(string documentID, int positionNumber, string itemID, string stockKeepUnit, string warehouseNumber, string warehousePlace, double quantity, double countedQuantity)
        {
            DocumentID = documentID;
            PositionNumber = positionNumber;
            ItemID = itemID;
            StockKeepUnit = stockKeepUnit;
            WarehouseNumber = warehouseNumber;
            WarehousePlace = warehousePlace;
            Quantity = quantity;
            CountedQuantity = countedQuantity;
        }

        public string DocumentID { get; set; }
        public int PositionNumber { get; set; }
        public string ItemID { get; set; }
        public string StockKeepUnit { get; set; }
        public string WarehouseNumber { get; set; }
        public string WarehousePlace { get; set; }
        public double Quantity { get; set; }
        public double CountedQuantity { get; set; }

        public InventoryHeader InventoryHeader { get; set; }
        public Item Item { get; set; }
        public StockKeepUnit StockKeepUnitt { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
