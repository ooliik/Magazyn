using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class ReleaseLine
    {
        public ReleaseLine()
        {
        }

        public ReleaseLine(string documentID)
        {
            DocumentID = documentID;
        }

        public ReleaseLine(string documentID, string itemID, string stockKeepUnit, string warehouseNumber, string warehousePlace, double quantity)
        {
            DocumentID = documentID;
            ItemID = itemID;
            StockKeepUnit = stockKeepUnit;
            WarehouseNumber = warehouseNumber;
            WarehousePlace = warehousePlace;
            Quantity = quantity;

        }

        public ReleaseLine(string documentID, int positionNumber, string itemID, string stockKeepUnit, string warehouseNumber, string warehousePlace, double quantity)
        {
            DocumentID = documentID;
            PositionNumber = positionNumber;
            ItemID = itemID;
            StockKeepUnit = stockKeepUnit;
            WarehouseNumber = warehouseNumber;
            WarehousePlace = warehousePlace;
            Quantity = quantity;

        }


        public string DocumentID { get; set; }
        public int PositionNumber { get; set; }
        public string ItemID { get; set; }
        public string StockKeepUnit { get; set; }
        public string WarehouseNumber { get; set; }
        public string WarehousePlace { get; set; }
        public double Quantity { get; set; }
        public double ReleaseQuantity { get; set; }
        public double ReleasedQuantity { get; set; }


        public ReleaseHeader ReleaseHeader { get; set; }
        public Item Item { get; set; }
        public StockKeepUnit StockKeepUnitt { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
