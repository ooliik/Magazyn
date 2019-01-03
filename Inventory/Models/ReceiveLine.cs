using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class ReceiveLine
    {
        public ReceiveLine()
        {
        }

        public ReceiveLine(string documentID)
        {
            DocumentID = documentID;
        }

        public ReceiveLine(string documentID, string itemID, string stockKeepUnit, string warehouseNumber, string warehousePlace, double quantity)
        {
            DocumentID = documentID;
            ItemID = itemID;
            StockKeepUnit = stockKeepUnit;
            WarehouseNumber = warehouseNumber;
            WarehousePlace = warehousePlace;
            Quantity = quantity;
        }

        public ReceiveLine(string documentID, int positionNumber, string itemID, string stockKeepUnit, string warehouseNumber, string warehousePlace, double quantity)
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionNumber { get; set; }
        public string ItemID { get; set; }
        public string StockKeepUnit { get; set; }
        public string WarehouseNumber { get; set; }
        public string WarehousePlace { get; set; }
        public double Quantity { get; set; }
        public double ReceiveQuantity { get; set; }
        public double ReceivedQuantity { get; set; }


        public ReceiveHeader ReceiveHeader { get; set; }
        public Item Item { get; set; }
        public StockKeepUnit StockKeepUnitt { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
