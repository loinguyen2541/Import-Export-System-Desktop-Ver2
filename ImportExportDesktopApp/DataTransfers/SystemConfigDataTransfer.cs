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
            ie = new IEEntities();
        }

        public int GetStorageCappacity()
        {
            String storageCapacityString = ie.SystemConfigs.Where(s => s.AttributeKey.Contains("StorageCapacity")).Select(s => s.AttributeValue).FirstOrDefault();
            return int.Parse(storageCapacityString);
        }

        public SystemConfig GetTimeSchedule()
        {
            return ie.SystemConfigs.Where(s => s.AttributeKey == "AutoSchedule").SingleOrDefault() ;
        }
    }
}
