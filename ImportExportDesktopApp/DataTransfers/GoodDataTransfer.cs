using ImportExportDesktopApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/8/2021 4:25:54 PM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class GoodDataTransfer
    {
        IEEntities ie;

        public GoodDataTransfer()
        {
            ie = new IEEntities();
        }

        public float getInventory()
        {
            return ie.Goods.Where(p => p.GoodsStatus == "Available").First().QuantityOfInventory;
        }

        public void UpdateInventory(int partnerTypeId, float weight, int capacity)
        {
            Good good = ie.Goods.Where(p => p.GoodsStatus == "Available").First();
            if (partnerTypeId == 1)
            {
                good.QuantityOfInventory -= weight;
            }
            else if (partnerTypeId == 2)
            {
                good.QuantityOfInventory += weight;
            }
            ie.Entry(good).State = System.Data.Entity.EntityState.Modified;
            ie.SaveChanges();
        }
    }
}
