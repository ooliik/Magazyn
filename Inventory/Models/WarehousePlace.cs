using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class WarehousePlace
    {
        public WarehousePlace()
        {
        }

        public WarehousePlace(WarehousePlace warehousePlace)
        {
            WarehouseName = warehousePlace.WarehouseName;
            WarehousePlaceName = warehousePlace.WarehousePlaceName;
        }

        public WarehousePlace(string warehouseName, string warehousePlaceName)
        {
            WarehouseName = warehouseName;
            WarehousePlaceName = warehousePlaceName;
        }

        public string WarehouseName { get; set; }
        public string WarehousePlaceName { get; set; }


        public Warehouse Warehouse { get; set; }
    }
}
