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
            Schedule schedule = ie.Schedules.Where(s => s.PartnerId == partnerId && s.ScheduleStatus == 0).Include(s => s.TimeTemplateItem).FirstOrDefault();
            return schedule;
        }
        public void UpdateSchedule(Schedule schedule)
        {
            ie.Entry(schedule).State = System.Data.Entity.EntityState.Modified;
        }
        public ObservableCollection<Schedule> GetAllSchedule(int page)
        {
            return new ObservableCollection<Schedule>(ie.Schedules.OrderByDescending(s => s.ScheduleDate).Skip((page - 1) * 10).Take(10));
        }
        public ObservableCollection<Schedule> SearchSchedule(String searchDate, string searchName, int type)
        {
            IQueryable<Schedule> queryable = ie.Schedules;
            if (type > -1)
            {
                queryable = queryable.Where(t => t.TransactionType == type);
            }
            if (searchName != null && searchName.Length > 0)
            {
                queryable = queryable.Where(t => t.Partner.DisplayName.ToLower().Contains(searchName.ToLower()));
            }
            DateTime dateTime;
            if (DateTime.TryParse(searchDate, out dateTime))
            {
                queryable = queryable.Where(t => DbFunctions.TruncateTime(t.CreatedDate) == dateTime.Date);
            }
            queryable = queryable.OrderByDescending(t => t.CreatedDate);
            return new ObservableCollection<Schedule>(queryable);
        }

        public int GetMaxPage(int pageSize)
        {
            int count = ie.Schedules.Count();
            double totalPage = (count * (1.0)) / (pageSize * (1.0));
            return (int)Math.Ceiling(totalPage);
        }
    }
}
