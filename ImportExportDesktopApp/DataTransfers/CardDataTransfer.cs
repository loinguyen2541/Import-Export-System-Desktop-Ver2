using ImportExportDesktopApp.Enums;
using ImportExportDesktopApp.ScaleModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/8/2021 12:10:15 AM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class CardDataTransfer
    {
        private readonly IEEntities ie;
        public CardDataTransfer()
        {
            ie = DataContext.GetInstance().DB;
        }

        public Partner CheckCard(TransactionScale transactionScale)
        {
            if (transactionScale.Device == EDeviceType.Card)
            {
                Partner partner = ie.IdentityCards.Include(i => i.Partner.PartnerType)
                    .Where(c => c.IdentityCardId.Equals(transactionScale.Indentify) && c.IdentityCardStatus == 0)
                    .Where(c => c.Partner.PartnerStatus == 0).Select(c => c.Partner)
                    .SingleOrDefault();
                ie.Entry(partner).Reload();
                return partner;
            }
            else if (transactionScale.Device == EDeviceType.Android)
            {
                Partner partner = ie.Partners.Include(p => p.PartnerType).Where(c => c.PartnerId == transactionScale.PartnerId && c.PartnerStatus == 0).SingleOrDefault();
                ie.Entry(partner).Reload();
                return partner;
            }
            return null;
        }

        public ICollection<IdentityCard> InsertCard(ICollection<IdentityCard> cards)
        {
            List<IdentityCard> tempList = new List<IdentityCard>();
            IdentityCard tempCard = null;
            bool check = true;
            foreach (var item in cards)
            {
                tempCard = ie.IdentityCards.Add(item);
                if (tempCard != null)
                {
                    tempList.Add(tempCard);
                }
                else
                {
                    check = false;
                    break;
                }
                tempCard = null;
            }
            if (check)
            {
                ie.SaveChanges();
            }
            return tempList;
        }

        public ObservableCollection<IdentityCard> GetAllCardsByPartnerId(int partnerId)
        {
            return new ObservableCollection<IdentityCard>(ie.IdentityCards.Where(i => i.PartnerId == partnerId));
        }
    }
}
