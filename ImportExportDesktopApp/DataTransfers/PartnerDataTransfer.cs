using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/13/2021 1:28:39 AM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class PartnerDataTransfer
    {
        IEEntities ie;
        public PartnerDataTransfer()
        {
            ie = DataContext.GetInstance().DB;
        }

        public ObservableCollection<Partner> GetAllByType(int PartnerTypeId)
        {
            return new ObservableCollection<Partner>(ie.Partners.Where(p => p.PartnerStatus == 0 && p.PartnerTypeId == PartnerTypeId));
        }

        public ObservableCollection<Partner> GetAll()
        {
            return new ObservableCollection<Partner>(ie.Partners);
        }
    }
}
