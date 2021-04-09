using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/9/2021 6:57:21 AM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class ScheduleDataTransfer
    {
        IEEntities ie;
        public ScheduleDataTransfer()
        {
            ie = DataContext.GetInstance().DB;
        }

        public Schedule CheckSchedule(int partnerId)
        {
            Schedule schedule = ie.Schedules.Where(s => s.PartnerId == partnerId).FirstOrDefault();
            return schedule;
        }
        public void UpdateSchedule(Schedule schedule)
        {
            ie.Entry(schedule).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
