using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Inventory inventory = ie.Inventories.Where(i => DbFunctions.TruncateTime(i.RecordedDate) == Today.Date).FirstOrDefault();
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

        public ObservableCollection<Inventory> GetAllInventory(int page)
        {
            return new ObservableCollection<Inventory>(ie.Inventories.OrderByDescending(i => i.RecordedDate).Take(10).Skip((page - 1) * 10));
        }
        public ObservableCollection<Inventory> SearchInventory(DateTime startDate, DateTime endDate, int page)
        {
            return new ObservableCollection<Inventory>(ie.Inventories.Where(i => startDate <= i.RecordedDate && i.RecordedDate <= endDate).Take(10).Skip((page - 1) * 10));
        }
        public int GetMaxPage(int pageSize)
        {
            int count = ie.Inventories.Count();
            double totalPage = count * (1.0) / pageSize * (1.0);
            return (int)Math.Ceiling(totalPage);
        }
    }
}
