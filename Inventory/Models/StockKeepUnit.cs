using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class StockKeepUnit
    {
        public StockKeepUnit()
        {
        }

        public StockKeepUnit(string code, string description, double quantityPerUnit)
        {
            Code = code;
            Description = description;
            QuantityPerUnit = quantityPerUnit;
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public double QuantityPerUnit { get; set; }

        public List<ItemStockKeepUnit> ItemStockKeepUnits { get; set; }
        public List<WarehouseEntry> WarehouseEntries { get; set; }
        public List<ReleaseLine> ReleaseLines { get; set; }
        public List<ReceiveLine> ReceiveLines { get; set; }
        public List<InventoryLine> InventoryLines { get; set; }

    }
}
