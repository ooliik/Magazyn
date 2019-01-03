using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public static class InvMgt
    {

        #region //customer
        public static string GetNextCustomerID()
        {
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.Series.Any(x => x.Table == TableType.Customer))
                {
                    Serie serie = dBCtx.Series.Single(x => x.Table == TableType.Customer);

                    dBCtx.Series.Attach(serie);

                    serie.LastUsedNumber = (int.Parse(serie.LastUsedNumber) + 1).ToString("D5");

                    dBCtx.SaveChanges();
                    return serie.Prefix + serie.LastUsedNumber;
                }
                return "";
            }
        }

        public static string isCustomerValid(Customer customer)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(customer.CustomerID))
                errorMsg += "\nPusty ID klienta";

            if (String.IsNullOrEmpty(customer.Name))
                errorMsg += "\nPusta nazwa";

            if (String.IsNullOrEmpty(customer.Address))
                errorMsg += "\nPusty adres";

            if (String.IsNullOrEmpty(customer.City))
                errorMsg += "\nPusta nazwa miasta";

            if (String.IsNullOrEmpty(customer.Country))
                errorMsg += "\nPusta nazwa państwa";

            return errorMsg;
        }

        public static string isCustomerAvaibleToDelete(Customer customer)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.ReleaseHeaders.Any(x => x.CustomerID == customer.CustomerID))
                {
                    errorMsg = "Nie można usunąć klienta, ponieważ jest powiązany z wydaniami magazynowymi.";
                    return errorMsg;
                }

                if (dBCtx.WarehouseEntries.Any(x => x.CustomerID == customer.CustomerID))
                {
                    errorMsg = "Nie można usunąć klienta, ponieważ jest powiązany z zapisami magazynowymi.";
                    return errorMsg;
                }

                return errorMsg;
            }
        }

        #endregion

        #region //vendor
        public static string GetNextVendorID()
        {
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.Series.Any(x => x.Table == TableType.Vendor))
                {
                    Serie serie = dBCtx.Series.Single(x => x.Table == TableType.Vendor);

                    dBCtx.Series.Attach(serie);

                    serie.LastUsedNumber = (int.Parse(serie.LastUsedNumber) + 1).ToString("D5");

                    dBCtx.SaveChanges();
                    return serie.Prefix + serie.LastUsedNumber;
                }
                return "";
            }
        }

        public static string isVendorValid(Vendor vendor)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(vendor.VendorID))
                errorMsg += "\nPusty ID sprzedawcy";

            if (String.IsNullOrEmpty(vendor.Name))
                errorMsg += "\nPusta nazwa";

            if (String.IsNullOrEmpty(vendor.Address))
                errorMsg += "\nPusty adres";

            if (String.IsNullOrEmpty(vendor.City))
                errorMsg += "\nPusta nazwa miasta";

            if (String.IsNullOrEmpty(vendor.Country))
                errorMsg += "\nPusta nazwa państwa";

            return errorMsg;
        }

        public static string isVendorAvaibleToDelete(Vendor vendor)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.ReceiveHeaders.Any(x => x.VendorID == vendor.VendorID))
                {
                    errorMsg = "Nie można usunąć sprzedawcy, ponieważ jest powiązany z przyjęciami magazynowymi.";
                    return errorMsg;
                }

                if (dBCtx.WarehouseEntries.Any(x => x.VendorID == vendor.VendorID))
                {
                    errorMsg = "Nie można usunąć sprzedawcy, ponieważ jest powiązany z zapisami magazynowymi.";
                    return errorMsg;
                }

                return errorMsg;
            }
        }


        #endregion

        #region //item
        public static string GetNextItemID()
        {

            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.Series.Any(x => x.Table == TableType.Item))
                {
                    Serie serie = dBCtx.Series.Single(x => x.Table == TableType.Item);

                    dBCtx.Series.Attach(serie);

                    serie.LastUsedNumber = (int.Parse(serie.LastUsedNumber) + 1).ToString("D5");

                    dBCtx.SaveChanges();
                    return serie.Prefix + serie.LastUsedNumber;
                }
                return "";
            }

        }

        public static string isItemValid(Item item)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(item.ItemID))
                errorMsg += "\nPusty ID produktu";

            if (String.IsNullOrEmpty(item.Description))
                errorMsg += "\nPusty opis";

            if (String.IsNullOrEmpty(item.CategoryID))
                errorMsg += "\nNie wybrano kategorii";

            if (String.IsNullOrEmpty(item.Name))
                errorMsg += "\nPusta nazswa produkt";

            return errorMsg;
        }
        public static string isItemAvaibleToDelete(Item item)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.WarehouseEntries.Any(x => x.ItemID == item.ItemID))
                {
                    errorMsg = "Nie można usunąć produktu, ponieważ istnieją dla niego zapisy magazynowe.";
                    return errorMsg;
                }
                if (dBCtx.ItemStockKeepUnits.Any(x => x.ItemID == item.ItemID))
                {
                    errorMsg = "Nie można usunąć produktu, ponieważ istnieją dla niego zdefiniowane jednostki miary.";
                    return errorMsg;
                }
                if (dBCtx.ReceiveLines.Any(x => x.ItemID == item.ItemID))
                {
                    errorMsg = "Nie można usunąć produktu, ponieważ istnieją dla niego zdefiniowane przyjęcia magazynowe.";
                    return errorMsg;
                }
                if (dBCtx.ReleaseLines.Any(x => x.ItemID == item.ItemID))
                {
                    errorMsg = "Nie można usunąć produktu, ponieważ istnieją dla niego zdefiniowane wydania magazynowe.";
                    return errorMsg;
                }
                return errorMsg;
            }
        }
        #endregion

        #region //category
        public static string isCategoryValid(Category cat)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(cat.CategoryID))
                errorMsg += "\nPusta nazwa kategorii";

            if (String.IsNullOrEmpty(cat.Description))
                errorMsg += "\nPusty opis";

            return errorMsg;
        }
        public static string isCategoryAvaibleToDelete(Category category)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.Items.Any(x => x.CategoryID == category.CategoryID))
                {
                    errorMsg = "Nie można usunąć kategorii, ponieważ isnieją dla niej produkty.";
                    return errorMsg;
                }
                return errorMsg;
            }
        }

        #endregion

        #region //warehouse

        public static string IsWarehouseValid(Warehouse warehouse)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(warehouse.WarehouseName))
                errorMsg += "\nPusta nazwa magazynu";

            if (String.IsNullOrEmpty(warehouse.Description))
                errorMsg += "\nPusty opis";

            if (String.IsNullOrEmpty(warehouse.Address))
                errorMsg += "\nPusty adres";

            return errorMsg;
        }
        public static string isWarehouseAvaibleToDelete(Warehouse warehouse)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.WarehouseEntries.Any(x => x.WarehouseNumber == warehouse.WarehouseName))
                {
                    errorMsg = "Nie można usunąć magazynu, ponieważ isnieją dla niego zapisy magazynowe.";
                    return errorMsg;
                }

                if (dBCtx.WarehousePlaces.Any(x => x.WarehouseName == warehouse.WarehouseName))
                {
                    errorMsg = "Nie można usunąć magazynu, ponieważ isnieją dla miejsca magazynowe.";
                    return errorMsg;
                }


                if (dBCtx.ReleaseLines.Any(x => x.WarehouseNumber == warehouse.WarehouseName))
                {
                    errorMsg = "Nie można usunąć magazynu, ponieważ jest wykorzystywany w wydaniach magazynowych.";
                    return errorMsg;
                }

                if (dBCtx.ReceiveLines.Any(x => x.WarehouseNumber == warehouse.WarehouseName))
                {
                    errorMsg = "Nie można usunąć magazynu, ponieważ jest wykorzystywany w przyjęciach magazynowych.";
                    return errorMsg;
                }

                return errorMsg;
            }
        }

        #endregion


        #region //warehousePlaces
        public static string isWarehousePlaceValid(WarehousePlace warehousePlace)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.WarehousePlaces.Any(x => x.WarehousePlaceName == warehousePlace.WarehousePlaceName &&
                                                   x.WarehouseName == warehousePlace.WarehouseName))
                {
                    errorMsg = "Miejsce magazynowe juz istnieje";
                    return errorMsg;
                }

                if (String.IsNullOrEmpty(warehousePlace.WarehousePlaceName))
                    errorMsg += "\nPusta nazwa miejsca";
            }
            return errorMsg;
        }

        public static string isWarehousePlaceAvaibleToDelete(WarehousePlace warehousePlace)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.WarehouseEntries.Any(x => x.WarehousePlace == warehousePlace.WarehousePlaceName &&
                                                   x.WarehouseNumber == warehousePlace.WarehouseName))
                {
                    errorMsg = "Nie można usunąć miejsca magazynowego, ponieważ isnieją dla niego zapisy magazynowe.";
                    return errorMsg;
                }

                if (dBCtx.ReleaseLines.Any(x => x.WarehousePlace == warehousePlace.WarehousePlaceName &&
                                                   x.WarehouseNumber == warehousePlace.WarehouseName))
                {
                    errorMsg = "Nie można usunąć miejsca magazynowego, ponieważ jest wykorzystywane w wydaniach magazynowych.";
                    return errorMsg;
                }

                if (dBCtx.ReceiveLines.Any(x => x.WarehousePlace == warehousePlace.WarehousePlaceName &&
                                                   x.WarehouseNumber == warehousePlace.WarehouseName))
                {
                    errorMsg = "Nie można usunąć miejsca magazynowego, ponieważ jest wykorzystywane w przyjęciach magazynowych.";
                    return errorMsg;
                }

                return errorMsg;
            }
        }
        #endregion

        #region //relese
        //releaseHeader
        public static string GetNextReleaseNo()
        {
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.Series.Any(x => x.Table == TableType.ReleaseHeader))
                {
                    Serie serie = dBCtx.Series.Single(x => x.Table == TableType.ReleaseHeader);

                    dBCtx.Series.Attach(serie);

                    serie.LastUsedNumber = (int.Parse(serie.LastUsedNumber) + 1).ToString("D5");

                    dBCtx.SaveChanges();
                    return serie.Prefix + serie.LastUsedNumber;
                }
                return "";
            }
        }


        public static string isReleaseHeaderValid(ReleaseHeader releaseHeader)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(releaseHeader.DocumentID))
                errorMsg += "\nPusty ID dokumentu";

            if (String.IsNullOrEmpty(releaseHeader.Description))
                errorMsg += "\nPusty opis";


            if (releaseHeader.ReleaseDate < DateTime.Today)
                errorMsg += "\nNieprawidłowa data";


            if (String.IsNullOrEmpty(releaseHeader.CustomerID))
                errorMsg += "\nPusta nazwa klienta";

            return errorMsg;
        }

        public static string isReleaseHeaderAvaibleToDelete(ReleaseHeader releaseHeader)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {


                if (dBCtx.ReleaseLines.Any(x => x.DocumentID == releaseHeader.DocumentID))
                {
                    errorMsg = "Nie można usunąć nagłówka wydania, ponieważ istnieją dla niego zdefiniowane linie wydań.";
                    return errorMsg;
                }
                return errorMsg;
            }
        }

        //release line

        public static string isReleaseLineValid(ReleaseLine releaseLine)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(releaseLine.DocumentID))
                errorMsg += "\nPusty ID dokumentu";

            if (releaseLine.PositionNumber <= 0)
                errorMsg += "\nNieprawidłowy numer pozycji";


            if (String.IsNullOrEmpty(releaseLine.ItemID))
                errorMsg += "\nNieprawidłowe ID";

            if (String.IsNullOrEmpty(releaseLine.StockKeepUnit))
                errorMsg += "\nNieprawidłowa jednostka";

            if (String.IsNullOrEmpty(releaseLine.WarehouseNumber))
                errorMsg += "\nNieprawidłowy numer magazynu";

            if (String.IsNullOrEmpty(releaseLine.WarehousePlace))
                errorMsg += "\nNieprawidłowe miejsce magazynowe";

            if (releaseLine.Quantity <= 0.0)
                errorMsg += "\nNiepoprawna ilość";

            return errorMsg;
        }


        #endregion

        #region //receive

        public static string GetNextReceiveHeader()
        {
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.Series.Any(x => x.Table == TableType.ReceiveHeader))
                {
                    Serie serie = dBCtx.Series.Single(x => x.Table == TableType.ReceiveHeader);

                    dBCtx.Series.Attach(serie);

                    serie.LastUsedNumber = (int.Parse(serie.LastUsedNumber) + 1).ToString("D5");

                    dBCtx.SaveChanges();
                    return serie.Prefix + serie.LastUsedNumber;
                }
                return "";
            }
        }

        public static string isReceiveHeaderValid(ReceiveHeader receiveHeader)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(receiveHeader.DocumentID))
                errorMsg += "\nPusty ID dokumentu";

            if (String.IsNullOrEmpty(receiveHeader.Description))
                errorMsg += "\nPusty opis";

            if (receiveHeader.ReceiveDate < DateTime.Today)
                errorMsg += "\nNieprawidłowa data";
            if (String.IsNullOrEmpty(receiveHeader.VendorID))
                errorMsg += "\nPusta nazwa sprzedawcy";

            return errorMsg;
        }

        public static string isReceiveHeaderAvaibleToDelete(ReceiveHeader receiveHeader)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {


                if (dBCtx.ReceiveLines.Any(x => x.DocumentID == receiveHeader.DocumentID))
                {
                    errorMsg = "Nie można usunąć nagłówka przyjęcia, ponieważ istnieją dla niego zdefiniowane linie przyjęć.";
                    return errorMsg;
                }
                return errorMsg;
            }
        }
        //receive line
        public static string isReceiveLineValid(ReceiveLine receiveLine)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(receiveLine.DocumentID))
                errorMsg += "\nPusty ID dokumentu";

            if (receiveLine.PositionNumber <= 0)
                errorMsg += "\nNieprawidłowy numer pozycji";


            if (String.IsNullOrEmpty(receiveLine.ItemID))
                errorMsg += "\nNieprawidłowe ID";

            if (String.IsNullOrEmpty(receiveLine.StockKeepUnit))
                errorMsg += "\nNieprawidłowa jednostka";

            if (String.IsNullOrEmpty(receiveLine.WarehouseNumber))
                errorMsg += "\nNieprawidłowy numer magazynu";

            if (String.IsNullOrEmpty(receiveLine.WarehousePlace))
                errorMsg += "\nNieprawidłowe miejsce magazynowe";

            if (receiveLine.Quantity <= 0.0)
                errorMsg += "\nNiepoprawna ilość";

            return errorMsg;
        }
        #endregion

        #region //intentory

        public static string GetNextInventoryHeader()
        {
            using (DBContext dBCtx = new DBContext())
            {
                if (dBCtx.Series.Any(x => x.Table == TableType.InventoryHeader))
                {
                    Serie serie = dBCtx.Series.Single(x => x.Table == TableType.InventoryHeader);

                    dBCtx.Series.Attach(serie);

                    serie.LastUsedNumber = (int.Parse(serie.LastUsedNumber) + 1).ToString("D5");

                    dBCtx.SaveChanges();
                    return serie.Prefix + serie.LastUsedNumber;
                }
                return "";
            }
        }

        public static string isInventoryHeaderValid(InventoryHeader inventoryHeader)
        {
            string errorMsg = null;

            if (String.IsNullOrEmpty(inventoryHeader.DocumentID))
                errorMsg += "\nPusty ID dokumentu";

            if (String.IsNullOrEmpty(inventoryHeader.Description))
                errorMsg += "\nPusty opis";


            if (inventoryHeader.InventoryDate < DateTime.Today)
                errorMsg += "\nNieprawidłowa data";

            if (String.IsNullOrEmpty(inventoryHeader.WarehouseName))
                errorMsg += "\nPusta nazwa magazynu";

            return errorMsg;
        }

        public static string isInventoryHeaderAvaibleToDelete(InventoryHeader inventoryHeader)
        {
            string errorMsg = null;
            using (DBContext dBCtx = new DBContext())
            {


                if (dBCtx.InventoryLines.Any(x => x.DocumentID == inventoryHeader.DocumentID))
                {
                    errorMsg = "Nie można usunąć nagłówka inwentarzu, ponieważ istnieją dla niego zdefiniowane linie inwentaryzacyjne.";
                    return errorMsg;
                }
                return errorMsg;
            }
        }

        public static string PostInventory(InventoryHeader inventoryHeader)
        {
            List<InventoryLine> inventoryLines;
            using (DBContext dBCtx = new DBContext())
            {

                if (!dBCtx.InventoryLines.Any(x => x.DocumentID == inventoryHeader.DocumentID))
                {
                    return ("Dokument nie posiada pozycji");
                }

                inventoryLines = dBCtx.InventoryLines.Where(x => x.DocumentID == inventoryHeader.DocumentID).ToList();
            }
            foreach (InventoryLine inventoryLine in inventoryLines)
            {
                if (inventoryLine.CountedQuantity != inventoryLine.Quantity)
                    postInventoryLine(inventoryLine, inventoryHeader);
                else
                {
                    using (DBContext dBCtx = new DBContext())
                    {
                        dBCtx.InventoryLines.Remove(inventoryLine);
                        dBCtx.SaveChanges();
                    }

                }
            }

            using (DBContext dBCtx = new DBContext())
            {
                inventoryHeader = dBCtx.InventoryHeaders.First(x => x.DocumentID == inventoryHeader.DocumentID);
                dBCtx.InventoryHeaders.Remove(inventoryHeader);
                dBCtx.SaveChanges();


            }
            return "";
        }

        private static void postInventoryLine(InventoryLine inventoryLine, InventoryHeader inventoryHeader)
        {
            WarehouseEntry warehouseEntry = new WarehouseEntry();

            using (DBContext dBCtx = new DBContext())
            {
                StockKeepUnit stku = dBCtx.StockKeepUnits.Single(x => x.Code == inventoryLine.StockKeepUnit);


                warehouseEntry.ItemID = inventoryLine.ItemID;
                warehouseEntry.DocumentNumber = inventoryLine.DocumentID;
                warehouseEntry.EntryType = EntryType.Correction;
                warehouseEntry.TotalQuantity = (inventoryLine.CountedQuantity - inventoryLine.Quantity) * stku.QuantityPerUnit;
                warehouseEntry.Quantity = inventoryLine.CountedQuantity - inventoryLine.Quantity;
                warehouseEntry.QuantityPerUnit = stku.QuantityPerUnit;
                warehouseEntry.KeepUnit = inventoryLine.StockKeepUnit;
                warehouseEntry.WarehouseNumber = inventoryLine.WarehouseNumber;
                warehouseEntry.WarehousePlace = inventoryLine.WarehousePlace;
                warehouseEntry.DocumentDate = inventoryHeader.InventoryDate;
                warehouseEntry.DocumentDescription = inventoryHeader.Description;

                dBCtx.WarehouseEntries.Add(warehouseEntry);

                dBCtx.InventoryLines.Remove(inventoryLine);
                dBCtx.SaveChanges();
            }
        }
        #endregion


        #region //posting
        //Receive
        public static string postReceiveHeader(ReceiveHeader receiveHeader)
        {
            string errorMsg = null;
            bool isLineToPost = false;
            List<ReceiveLine> receiveLines;
            using (DBContext dBCtx = new DBContext())
            {
                if (!dBCtx.ReceiveLines.Any(x => x.DocumentID == receiveHeader.DocumentID))
                {
                    return ("Dokument nie posiada pozycji");
                }

                receiveLines = dBCtx.ReceiveLines.Where(x => x.DocumentID == receiveHeader.DocumentID).ToList();
                foreach (ReceiveLine receiveLine in receiveLines)
                {
                    errorMsg = chceckReceiveLine(receiveLine);
                    if (!String.IsNullOrEmpty(errorMsg))
                        return (errorMsg);

                    if (chceckQuantityToReceive(receiveLine))
                        isLineToPost = true;

                    if (!String.IsNullOrEmpty(errorMsg))
                        return errorMsg;

                }
            }

            if (!isLineToPost)
                return "Nie uzupełniono ilości do realizacji";

            foreach (ReceiveLine receiveLine in receiveLines)
            {
                if (receiveLine.ReceiveQuantity > 0.00)
                    postReceiveLine(receiveLine, receiveHeader);
            }

            return "";
        }

        private static string chceckReceiveLine(ReceiveLine receiveLine)
        {
            string errorMsg = null;
            if (receiveLine.Quantity < receiveLine.ReceiveQuantity + receiveLine.ReceivedQuantity)
            {
                errorMsg = "\nIlość do przyjęcia jest niepoprawna dla " + receiveLine.ItemID;
            }
            if (!String.IsNullOrEmpty(errorMsg))
                return errorMsg;

            errorMsg = isReceiveLineValid(receiveLine);

            return errorMsg;
        }

        private static void postReceiveLine(ReceiveLine receiveLine, ReceiveHeader receiveHeader)
        {
            WarehouseEntry warehouseEntry = new WarehouseEntry();

            using (DBContext dBCtx = new DBContext())
            {
                StockKeepUnit stku = dBCtx.StockKeepUnits.Single(x => x.Code == receiveLine.StockKeepUnit);


                warehouseEntry.ItemID = receiveLine.ItemID;
                warehouseEntry.DocumentNumber = receiveLine.DocumentID;
                warehouseEntry.EntryType = EntryType.Receive;
                warehouseEntry.TotalQuantity = receiveLine.ReceiveQuantity * stku.QuantityPerUnit;
                warehouseEntry.Quantity = receiveLine.ReceiveQuantity;
                warehouseEntry.QuantityPerUnit = stku.QuantityPerUnit;
                warehouseEntry.KeepUnit = receiveLine.StockKeepUnit;
                warehouseEntry.WarehouseNumber = receiveLine.WarehouseNumber;
                warehouseEntry.WarehousePlace = receiveLine.WarehousePlace;
                warehouseEntry.VendorID = receiveHeader.VendorID;
                warehouseEntry.DocumentDate = receiveHeader.ReceiveDate;
                warehouseEntry.DocumentDescription = receiveHeader.Description;

                dBCtx.WarehouseEntries.Add(warehouseEntry);

                ReceiveLine receiveLineUpdate = dBCtx.ReceiveLines.First(x => x.DocumentID == receiveLine.DocumentID && x.PositionNumber == receiveLine.PositionNumber);
                dBCtx.ReceiveLines.Attach(receiveLineUpdate);
                receiveLineUpdate.ReceivedQuantity = receiveLineUpdate.ReceivedQuantity + receiveLine.ReceiveQuantity;
                receiveLineUpdate.ReceiveQuantity = 0.00;
                dBCtx.SaveChanges();
            }
        }

        private static bool chceckQuantityToReceive(ReceiveLine receiveLine)
        {

            return receiveLine.ReceiveQuantity > 0.00;
        }

        //Release
        public static string postReleaseHeader(ReleaseHeader releaseHeader)
        {
            string errorMsg = null;
            bool isLineToPost = false;
            List<ReleaseLine> releaseLines;
            using (DBContext dBCtx = new DBContext())
            {
                if (!dBCtx.ReleaseLines.Any(x => x.DocumentID == releaseHeader.DocumentID))
                {
                    return ("Dokument nie posiada pozycji");
                }

                releaseLines = dBCtx.ReleaseLines.Where(x => x.DocumentID == releaseHeader.DocumentID).ToList();
                foreach (ReleaseLine releaseLine in releaseLines)
                {
                    errorMsg = chceckReleaseLine(releaseLine);

                    if (!String.IsNullOrEmpty(errorMsg))
                        return (errorMsg);

                    if (chceckQuantityToRelease(releaseLine))
                        isLineToPost = true;
                    if (!String.IsNullOrEmpty(errorMsg))
                        return errorMsg;
                }
            }

            if (!isLineToPost)
                return "Nie uzupełniono ilości do realizacji";

            foreach (ReleaseLine releaseLine in releaseLines)
            {
                if (releaseLine.ReleaseQuantity > 0.00)
                    postReleaseLine(releaseLine, releaseHeader);
            }

            return "";
        }

        private static string chceckReleaseLine(ReleaseLine releaseLine)
        {
            string errorMsg = null;
            if (releaseLine.Quantity < releaseLine.ReleaseQuantity + releaseLine.ReleasedQuantity)
            {
                errorMsg = "\nIlość do wydania jest niepoprawna dla " + releaseLine.ItemID;
            }
            if (!String.IsNullOrEmpty(errorMsg))
                return errorMsg;

            if (calculateAvailableQuantity(releaseLine.ItemID, releaseLine.StockKeepUnit, releaseLine.WarehouseNumber, releaseLine.WarehousePlace) < releaseLine.ReleaseQuantity)
            {
                errorMsg = "\nNiewystarczający stan magazynowy dla " + releaseLine.ItemID + " w jednostce " + releaseLine.StockKeepUnit + " w magazynie " + releaseLine.WarehouseNumber + " w miejscu " + releaseLine.WarehousePlace;
            }
            if (!String.IsNullOrEmpty(errorMsg))
                return errorMsg;
            // calculateAvailableQuantity
            errorMsg = isReleaseLineValid(releaseLine);

            return errorMsg;
        }

        private static void postReleaseLine(ReleaseLine releaseLine, ReleaseHeader releaseHeader)
        {
            WarehouseEntry warehouseEntry = new WarehouseEntry();

            using (DBContext dBCtx = new DBContext())
            {
                StockKeepUnit stku = dBCtx.StockKeepUnits.Single(x => x.Code == releaseLine.StockKeepUnit);


                warehouseEntry.ItemID = releaseLine.ItemID;
                warehouseEntry.DocumentNumber = releaseLine.DocumentID;
                warehouseEntry.EntryType = EntryType.Release;
                warehouseEntry.TotalQuantity = -(releaseLine.ReleaseQuantity * stku.QuantityPerUnit);
                warehouseEntry.Quantity = -(releaseLine.ReleaseQuantity);
                warehouseEntry.QuantityPerUnit = stku.QuantityPerUnit;
                warehouseEntry.KeepUnit = releaseLine.StockKeepUnit;
                warehouseEntry.WarehouseNumber = releaseLine.WarehouseNumber;
                warehouseEntry.WarehousePlace = releaseLine.WarehousePlace;
                warehouseEntry.CustomerID = releaseHeader.CustomerID;
                warehouseEntry.DocumentDate = releaseHeader.ReleaseDate;
                warehouseEntry.DocumentDescription = releaseHeader.Description;

                dBCtx.WarehouseEntries.Add(warehouseEntry);

                ReleaseLine releaseLineUpdate = dBCtx.ReleaseLines.First(x => x.DocumentID == releaseLine.DocumentID && x.PositionNumber == releaseLine.PositionNumber);
                dBCtx.ReleaseLines.Attach(releaseLineUpdate);
                releaseLineUpdate.ReleasedQuantity = releaseLineUpdate.ReleasedQuantity + releaseLine.ReleaseQuantity;
                releaseLineUpdate.ReleaseQuantity = 0.00;
                dBCtx.SaveChanges();
            }
        }

        private static bool chceckQuantityToRelease(ReleaseLine releaseLine)
        {

            return releaseLine.ReleaseQuantity > 0.00;
        }

        private static double calculateAvailableQuantity(string itemID, string stockKeepUnit, string warehouse, string warehousePlace)
        {
            using (DBContext dBCtx = new DBContext())
            {
                var result = dBCtx.WarehouseEntries
                .Where(x => x.ItemID == itemID && x.KeepUnit == stockKeepUnit && x.WarehouseNumber == warehouse && x.WarehousePlace == warehousePlace)
                .GroupBy(x => new { x.ItemID, x.StockKeepUnit, x.WarehouseNumber, x.WarehousePlace })
                .Select(g => new
                {
                    Quantity = g.Sum(x => x.Quantity)
                });
                if (result.Count() == 0)
                    return 0.0;
                else
                    return result.First().Quantity;
            }
        }

        #endregion
    }
}
