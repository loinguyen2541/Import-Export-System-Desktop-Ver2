using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Schedule> GetAllSchedule(int page)
        {
            return new ObservableCollection<Schedule>(ie.Schedules.OrderByDescending(s => s.CreatedDate).Take(10).Skip((page - 1) * 10));
        }
        public ObservableCollection<Schedule> SearchSchedule(DateTime searchDate, string searchName)
        {
            return new ObservableCollection<Schedule>(ie.Schedules.OrderByDescending(s => s.Partner.DisplayName.Contains(searchName) && s.ScheduleDate >= searchDate));
        }
        public int GetMaxPage(int pageSize)
        {
            int count = ie.Schedules.Count();
            double totalPage = count * (1.0) / pageSize * (1.0);
            return (int)Math.Ceiling(totalPage);
        }
    }
}
