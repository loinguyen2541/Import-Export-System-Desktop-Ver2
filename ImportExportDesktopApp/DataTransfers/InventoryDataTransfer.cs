using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/8/2021 5:20:02 PM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class InventoryDataTransfer
    {
        IEEntities ie;
        public InventoryDataTransfer()
        {
            ie = new IEEntities();
        }

        public Inventory CheckExist()
        {
            DateTime Today = DateTime.Now;
            Inventory inventory = ie.Inventories.Where(i => DbFunctions.TruncateTime(i.RecordedDate) == Today.Date).SingleOrDefault();
            return inventory;
        }

        public Inventory CreateInventoryToday(float GoodsInventory)
        {
            Inventory inventory = new Inventory();
            inventory.RecordedDate = DateTime.Now;
            inventory.OpeningStock = GoodsInventory;
            inventory = ie.Inventories.Add(inventory);
            ie.SaveChanges();
            return inventory;
        }
    }
}
