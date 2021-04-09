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

        public void updateTimetemplateItemWeight(DateTime TimeOut, float weight, int partnerTypeId)
        {
            TimeSpan timeOut = TimeSpan.Parse(TimeOut.ToString("HH:mm"));
            List<TimeTemplateItem> timeTemplateItems = ie.TimeTemplateItems.Where(i => i.ScheduleTime > TimeOut.TimeOfDay).ToList();
            foreach (var item in timeTemplateItems)
            {
                if (partnerTypeId == 1)
                {
                    item.Inventory -= weight;
                }
                else if (partnerTypeId == 2)
                {
                    item.Inventory += weight;
                }

                if (item.Inventory < 0)
                {
                    item.Inventory = 0;
                }
                ie.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
        }
    }
}
