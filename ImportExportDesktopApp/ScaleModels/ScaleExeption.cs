using ImportExportDesktopApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/11/2021 4:54:40 PM 
*/

namespace ImportExportDesktopApp.ScaleModels
{
    public class ScaleExeption
    {
        public ScaleExeption(TransactionScale transactionScale, Partner partner, Schedule schedule, EScaleExceptionType exceptionType, String message)
        {
            TransactionScale = transactionScale;
            Partner = partner;
            Schedule = schedule;
            ExceptionType = exceptionType;
            Message = message;
        }

        public TransactionScale TransactionScale { get; set; }
        public Partner Partner { get; set; }
        public Schedule Schedule { get; set; }
        public EScaleExceptionType ExceptionType { get; set; }
        public String Message { get; set; }
    }
}
