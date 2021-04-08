using ImportExportDesktopApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/8/2021 12:07:08 AM 
*/

namespace ImportExportDesktopApp.ScaleModels
{
    class TransactionScale
    {
        public EDeviceType Device { get; set; }
        public EGate Gate { get; set; }
        public String Indentify { get; set; }
        public int PartnerId { get; set; }
        public float Weight { get; set; }
    }
}
