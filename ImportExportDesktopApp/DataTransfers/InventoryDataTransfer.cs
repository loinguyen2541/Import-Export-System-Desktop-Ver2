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
            return new ObservableCollection<Inventory>(ie.Inventories.OrderByDescending(i => i.RecordedDate).Skip((page - 1) * 10).Take(10));
        }
        public ObservableCollection<Inventory> SearchInventory(string startDate, string endDate, int page)
        {
            IQueryable<Inventory> queryable = ie.Inventories;

            DateTime fromDatetime;
            if (DateTime.TryParse(startDate, out fromDatetime))
            {
                queryable = queryable.Where(t => DbFunctions.TruncateTime(t.RecordedDate) >= fromDatetime.Date);
            }

            DateTime toDatetime;
            if (DateTime.TryParse(endDate, out toDatetime))
            {
                queryable = queryable.Where(t => DbFunctions.TruncateTime(t.RecordedDate) <= toDatetime.Date);
            }
            queryable = queryable.OrderByDescending(t => t.RecordedDate);
            return new ObservableCollection<Inventory>(queryable);
        }
        public int GetMaxPage(int pageSize)
        {
            int count = ie.Inventories.Count();
            double totalPage = count * (1.0) / pageSize * (1.0);
            return (int)Math.Ceiling(totalPage);
        }
    }
}
