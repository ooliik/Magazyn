using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class ReceiveHeader
    {
        public ReceiveHeader()
        {
        }

        public ReceiveHeader(string ID)
        {
            DocumentID = InvMgt.GetNextReceiveHeader();
        }

        public ReceiveHeader(string documentID, string description, DateTime receiveDate, string vendorID) : this(documentID)
        {
            Description = description;
            ReceiveDate = receiveDate;
            VendorID = vendorID;
        }

        


        public string DocumentID { get; set; }
        public string Description { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string VendorID { get; set; }


        public Vendor Vendor { get; set; }

        public List<ReceiveLine> ReceiveLines { get; set; }
    }
}
