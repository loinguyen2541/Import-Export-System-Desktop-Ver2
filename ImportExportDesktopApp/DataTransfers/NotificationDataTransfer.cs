using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.DataTransfers
{
    class NotificationDataTransfer
    {
        IEEntities ie;
        public NotificationDataTransfer()
        {
            ie = DataContext.GetInstance().DB;

        }
    }
}
