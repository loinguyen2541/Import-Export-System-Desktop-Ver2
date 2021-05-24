using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/8/2021 4:03:57 PM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class SystemConfigDataTransfer
    {
        IEEntities ie;
        public SystemConfigDataTransfer()
        {
            ie = DataContext.GetInstance().DB;
        }

        public int GetStorageCappacity()
        {
            String storageCapacityString = ie.SystemConfigs.Where(s => s.AttributeKey.Contains("StorageCapacity")).Select(s => s.AttributeValue).FirstOrDefault();
            return int.Parse(storageCapacityString);
        }

        public SystemConfig GetStorageCappacityEntity()
        {

            return ie.SystemConfigs.Where(s => s.AttributeKey == "StorageCapacity").SingleOrDefault();
        }

        public SystemConfig GetTimeSchedule()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "AutoSchedule").SingleOrDefault();
        }

        public SystemConfig GetStartWorking()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "StartWorking").SingleOrDefault();
        }

        public SystemConfig GetFinishWorking()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "FinishWorking").SingleOrDefault();
        }
        public SystemConfig GetStartBreak()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "StartBreak").SingleOrDefault();
        }

        public SystemConfig GetFinishBreak()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "FinishBreak").SingleOrDefault();
        }

        public SystemConfig GetTimeBetweenSlot()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "TimeBetweenSlot").SingleOrDefault();
        }

        public SystemConfig GetMaximumSlot()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "MaximumSlot").SingleOrDefault();
        }

        public SystemConfig GetMaximumCanceledSchechule()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "MaximumCanceledSchechule").SingleOrDefault();
        }

        public void Update(SystemConfig systemConfig)
        {
            ie.Entry(systemConfig).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            ie.SaveChanges();
        }
    }
}
