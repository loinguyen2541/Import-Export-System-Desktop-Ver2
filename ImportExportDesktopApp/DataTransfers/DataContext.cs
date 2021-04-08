using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/8/2021 8:55:18 PM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class DataContext
    {
        private static DataContext _instance;
        public IEEntities DB { get; set; }
        private DataContext()
        {
            DB = new IEEntities();
        }

        public static DataContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DataContext();
            }
            return _instance;
        }
    }
}
