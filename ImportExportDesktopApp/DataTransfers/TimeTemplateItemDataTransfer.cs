using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/9/2021 7:04:28 AM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class TimeTemplateItemDataTransfer
    {
        IEEntities ie;
        public TimeTemplateItemDataTransfer()
        {
            ie = DataContext.GetInstance().DB;
        }

        public void updateTimetemplateItemWeight(int id, float weight, int partnerTypeId)
        {
            TimeTemplateItem timeTemplateItem = ie.TimeTemplateItems.Find(id);
            if (partnerTypeId == 1)
            {
                timeTemplateItem.Inventory -= weight;
            }
            else if (partnerTypeId == 2)
            {
                timeTemplateItem.Inventory += weight;
            }

            if (timeTemplateItem.Inventory < 0)
            {
                timeTemplateItem.Inventory = 0;
            }

            ie.Entry(timeTemplateItem).State = System.Data.Entity.EntityState.Modified;

        }
    }
}
