using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.DataTransfers
{
    class AccountDataTransfer
    {
        private readonly IEEntities ie;
        public AccountDataTransfer()
        {
            ie = DataContext.GetInstance().DB;
        }

        public ObservableCollection<Account> GetAll()
        {
            return new ObservableCollection<Account>(ie.Accounts);
        }

        public bool CheckExisted(string username)
        {
            bool check = true;
            ObservableCollection<Account> listAccount = GetAll();
            if (listAccount.Count != 0)
            {
                foreach (var item in listAccount)
                {
                    if(item.Username.Equals(username))
                    {
                        check = false;
                        break;
                    }
                }
            }
            return check;
        }

        public Account InsertAccount(Account account)
        {
            bool checkExisted = CheckExisted(account.Username);
            Account newAccount = null;
            if (checkExisted)
            {
                newAccount = ie.Accounts.Add(account);
                ie.SaveChanges();
            }
            return newAccount;
        }
    }
}
