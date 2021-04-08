using ImportExportDesktopApp.Enums;
using ImportExportDesktopApp.ScaleModels;
using System;
using System.Collections.Generic;
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
            ie = new IEEntities();
        }

        public Partner CheckCard(TransactionScale transactionScale)
        {
            if (transactionScale.Device == EDeviceType.Card)
            {
                Partner partner = ie.IdentityCards
                    .Where(c => c.IdentityCardId.Equals(transactionScale.Indentify) && c.IdentityCardStatus == 0)
                    .Where(c => c.Partner.PartnerStatus == 0).Select(c => c.Partner)
                    .SingleOrDefault();
                return partner;
            }
            else if (transactionScale.Device == EDeviceType.Android)
            {
                Partner partner = ie.Partners.Where(c => c.PartnerId == transactionScale.PartnerId && c.PartnerStatus == 0).SingleOrDefault();
                return partner;
            }
            return null;
        }
    }
}
