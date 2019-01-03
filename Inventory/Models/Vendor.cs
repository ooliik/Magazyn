using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class Vendor
    {
        public Vendor()
        {
        }

        public Vendor(string ID)
        {
            VendorID = InvMgt.GetNextVendorID();
        }

        public Vendor(string vendorID, string name, string address, string city, string country)
        {
            VendorID = vendorID;
            Name = name;
            Address = address;
            City = city;
            Country = country;
        }


        public string VendorID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }


        public List<ReceiveHeader> ReceiveHeaders { get; set; }
        public List<WarehouseEntry> WarehouseEntries { get; set; }
    }
}
