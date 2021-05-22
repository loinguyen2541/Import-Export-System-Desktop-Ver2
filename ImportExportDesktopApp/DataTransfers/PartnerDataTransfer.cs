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
            return new ObservableCollection<Partner>(ie.Partners.AsNoTracking().OrderByDescending(p => p.PartnerId).Take(10).Skip((page - 1) * 10));
        }

        public ObservableCollection<PartnerType> GetTypes()
        {
            return new ObservableCollection<PartnerType>(ie.PartnerTypes);
        }

        public ObservableCollection<Partner> SearchPartner(int type, string search)
        {
            List<Partner> partners = ie.Partners.AsNoTracking().Where(p => p.PartnerTypeId == type && p.DisplayName.Contains(search)).ToList();
            return new ObservableCollection<Partner>(partners);
        }

        public bool CheckCardPartner(int partnerId, String cardId)
        {
            bool check = true;
            if (cardId.Length != 0)
            {
                //check card is exist in system or not
                var card = ie.IdentityCards.Find(cardId);
                if (card != null) check = false;
            }
            return check;
        }

        public Partner GetByID(int id)
        {
            return ie.Partners.AsNoTracking().Where(t => t.PartnerId == id).SingleOrDefault();
        }
        public void CreatePartner(Partner partner)
        {
            ie.Partners.Add(partner);
            ie.SaveChanges();
        }
        public int GetMaxPage(int pageSize)
        {
            int count = ie.Partners.Count();
            double totalPage = count * (1.0) / pageSize * (1.0);
            return (int)Math.Ceiling(totalPage);
        }

    }
}
