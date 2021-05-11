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

        public bool CreateListTimeTemplateItem(String startWorking, String finishWorking, String startBreaking, String finishBreaking, String timeBetweenSlot)
        {
            TimeSpan startWorkingTime;
            TimeSpan finishWorkingTime;
            TimeSpan startBreakingTime;
            TimeSpan finishBreakingTime;
            int timeBetweenSlotInt = int.Parse(timeBetweenSlot);

            if (!TimeSpan.TryParse(startWorking, out startWorkingTime))
            {
                return false;
            }
            if (!TimeSpan.TryParse(finishWorking, out finishWorkingTime))
            {
                return false;
            }
            if (!TimeSpan.TryParse(startBreaking, out startBreakingTime))
            {
                return false;
            }
            if (!TimeSpan.TryParse(finishBreaking, out finishBreakingTime))
            {
                return false;
            }

            Disable(GetPendingList());

            TimeSpan TimeSpanBetweenSlot = new TimeSpan(0, timeBetweenSlotInt, 0);

            TimeTemplate timeTemplate = ie.TimeTemplates.Where(i => i.TimeTemplateStatus == 0).SingleOrDefault();

            for (TimeSpan i = startWorkingTime; i <= finishWorkingTime; i = i + TimeSpanBetweenSlot)
            {

                if (i < startBreakingTime || i >= finishBreakingTime)
                {
                    TimeTemplateItem timeTemplateItem = new TimeTemplateItem();
                    timeTemplateItem.TimeTemplateId = timeTemplate.TimeTemplateId;
                    timeTemplateItem.Inventory = 0;
                    timeTemplateItem.Status = 1;
                    timeTemplateItem.ScheduleTime = i;
                    ie.TimeTemplateItems.Add(timeTemplateItem);
                }
            }

            return true;
        }

        private List<TimeTemplateItem> GetPendingList()
        {
            return ie.TimeTemplateItems.Where(t => t.Status == 1).ToList();
        }

        private void Disable(List<TimeTemplateItem> timeTemplateItems)
        {
            foreach (var item in timeTemplateItems)
            {
                item.Status = 2;
                ie.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
        }
    }
}
