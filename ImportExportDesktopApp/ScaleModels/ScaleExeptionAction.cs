using ImportExportDesktopApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/11/2021 9:22:10 PM 
*/

namespace ImportExportDesktopApp.ScaleModels
{
    class ScaleExeptionAction
    {
        public ScaleExeptionAction(EGate gate, EScaleExceptionAction action)
        {
            Gate = gate;
            Action = action;
        }

        public EGate Gate { get; set; }
        public EScaleExceptionAction Action { get; set; }
    }
}
