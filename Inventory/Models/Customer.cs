using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(string ID)
        {
            CustomerID = InvMgt.GetNextCustomerID();
        }

        public Customer(string customerID, string name, string address, string city, string country)
        {
            CustomerID = customerID;
            Name = name;
            Address = address;
            City = city;
            Country = country;
        }


        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public List<ReleaseHeader> ReleaseHeaders { get; set; }
        public List<WarehouseEntry> WarehouseEntries { get; set; }
    }
}
