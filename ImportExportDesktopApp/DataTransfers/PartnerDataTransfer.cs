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

        public ObservableCollection<Partner> GetAllWithPaging(int page)
        {
            return new ObservableCollection<Partner>(ie.Partners.OrderBy(p=>p.PartnerId).Take(10).Skip((page - 1) * 10));
        }

        public ObservableCollection<PartnerType> GetTypes()
        {
            return new ObservableCollection<PartnerType>(ie.PartnerTypes);
        }

        public ObservableCollection<Partner> SearchPartner(int type, string search)
        {
            return new ObservableCollection<Partner>(ie.Partners.Where(p => p.PartnerTypeId == type && p.DisplayName.Contains(search)));
        }

        public bool CheckCardPartner(int partnerId, String cardId)
        {
            bool check = true;
            if(cardId.Length != 0)
            {
                //check card is exist in system or not
                var card = ie.IdentityCards.Find(cardId);
                if (card != null) check = false;
            }
            return check;
        }

        public Partner CreatePartner(Partner partner)
        {
            var newpartner = ie.Partners.Add(partner);
            ie.SaveChanges();
            return newpartner;
        }
        public void Save()
        {
            ie.SaveChanges();
        }
    }
}
