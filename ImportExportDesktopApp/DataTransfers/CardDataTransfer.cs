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

        public Partner CheckCard(string identificationCode)
        {
            Partner partner = ie.IdentityCards
                .Where(c => c.IdentityCardId.Equals(identificationCode) && c.IdentityCardStatus == 0)
                .Where(c => c.Partner.PartnerStatus == "Block").Select(c => c.Partner)
                .SingleOrDefault();
            return partner;
        }
    }
}
