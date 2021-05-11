using ImportExportDesktopApp.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/7/2021 11:10:00 PM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class TransactionDataTransfer
    {

        IEEntities ie;
        public TransactionDataTransfer()
        {
            ie = DataContext.GetInstance().DB;
        }

        public ObservableCollection<Transaction> GetProcessingTransaction()
        {
            return new ObservableCollection<Transaction>(ie.Transactions.Include(t => t.Partner).Where(t => t.TransactionStatus == 0).OrderByDescending(t => t.TimeIn).Take(10));
        }

        public ObservableCollection<Transaction> GetSuccessTransaction()
        {
            DateTime dateTime = DateTime.Now;
            return new ObservableCollection<Transaction>(ie.Transactions.Include(t => t.Partner).Where(t => t.TransactionStatus == 1 && DbFunctions.TruncateTime(t.TimeOut) == dateTime.Date).OrderByDescending(t => t.TimeOut).Take(10));
        }

        public ObservableCollection<Transaction> GetProcessingTransactionByPartnerToday(int partnerId)
        {
            var today = DateTime.Now;

            return new ObservableCollection<Transaction>(ie.Transactions.Include(t => t.Partner).Where(t => t.TransactionStatus == 0 && t.PartnerId == partnerId && DbFunctions.TruncateTime(t.CreatedDate) == today.Date).OrderByDescending(t => t.TimeIn).ToList());
        }

        public ObservableCollection<Transaction> GetSuccessTransactionByPartnerToday(int partnerId)
        {
            var today = DateTime.Now;
            return new ObservableCollection<Transaction>(ie.Transactions.Include(t => t.Partner).Where(t => t.TransactionStatus == 1 && t.PartnerId == partnerId && DbFunctions.TruncateTime(t.CreatedDate) == today.Date).OrderByDescending(t => t.TimeOut).ToList());
        }

        public ObservableCollection<Transaction> GetFailTransactionByPartnerToday(int partnerId)
        {
            var today = DateTime.Now;
            return new ObservableCollection<Transaction>(ie.Transactions.Include(t => t.Partner).Where(t => t.TransactionStatus == 2 && t.PartnerId == partnerId && DbFunctions.TruncateTime(t.CreatedDate) == today.Date).OrderByDescending(t => t.TimeOut).ToList());
        }

        public Transaction InsertTransaction(Transaction transaction)
        {
            var newTransaction = ie.Transactions.Add(transaction);
            ie.SaveChanges();
            return newTransaction;
        }

        public Transaction UpdateTransactionNotSaveChanges(Transaction transaction)
        {
            ie.Entry(transaction).State = EntityState.Modified;
            return transaction;
        }


        public Transaction IsProcessing(String identify)
        {
            Transaction transaction = ie.Transactions
                .Where(t => t.IdentificationCode.Contains(identify) && t.TransactionStatus == 0).SingleOrDefault();
            return transaction;
        }

        /// <summary>
        ///     -1 is get all type
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ObservableCollection<Transaction> GetTransactions(int page, int type)
        {
            if (type == -1)
            {
                List<Transaction> transactions = ie.Transactions.OrderByDescending(t => t.CreatedDate).Skip((page - 1) * 20).Take(20).ToList();
                return new ObservableCollection<Transaction>(transactions);
            }
            else
            {
                return new ObservableCollection<Transaction>(ie.Transactions.OrderByDescending(t => t.CreatedDate).Where(t => t.TransactionType == type).Skip((page - 1) * 10).Take(10));
            }
        }

        public void DisableAll(String identificationCode)
        {
            List<Transaction> transactions = ie.Transactions.Where(t => t.IdentificationCode == identificationCode && t.TransactionStatus == 0).ToList();
            foreach (var item in transactions)
            {
                item.TransactionStatus = 2;
                item.TimeOut = DateTime.Now;
                ie.Entry(item).State = EntityState.Modified;
            }
        }

        public int GetMaxPage(int pageSize)
        {
            int count = ie.Transactions.Count();
            double totalPage = count * (1.0) / pageSize * (1.0);
            return (int)Math.Ceiling(totalPage);
        }

        public void Save()
        {
            ie.SaveChanges();
        }

        public ObservableCollection<Transaction> SearchTransaction(int type, string searchPartner, String searchDate)
        {
            IQueryable<Transaction> queryable = ie.Transactions;
            if (type > -1)
            {
                queryable = queryable.Where(t => t.TransactionType == type);
            }
            if (searchPartner != null && searchPartner.Length > 0)
            {
                queryable = queryable.Where(t => t.Partner.DisplayName.ToLower().Contains(searchPartner.ToLower()));
            }
            DateTime dateTime;
            if (DateTime.TryParse(searchDate, out dateTime))
            {
                queryable = queryable.Where(t => DbFunctions.TruncateTime(t.CreatedDate) == dateTime.Date);
            }
            queryable = queryable.OrderByDescending(t => t.CreatedDate);
            return new ObservableCollection<Transaction>(queryable);
        }
    }
}
