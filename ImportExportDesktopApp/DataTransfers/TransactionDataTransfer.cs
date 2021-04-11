using ImportExportDesktopApp.Enums;
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
            return new ObservableCollection<Transaction>(ie.Transactions.Include(t => t.Partner).Where(t => t.TransactionStatus == 0).OrderByDescending(t => t.TimeIn).Take(5));
        }

        public ObservableCollection<Transaction> GetSuccessTransaction()
        {
            return new ObservableCollection<Transaction>(ie.Transactions.Include(t => t.Partner).Where(t => t.TransactionStatus == 1).OrderByDescending(t => t.TimeOut).Take(5));
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
                return new ObservableCollection<Transaction>(ie.Transactions.OrderByDescending(t => t.CreatedDate).Take(10).Skip((page - 1) * 10));
            }
            else
            {
                return new ObservableCollection<Transaction>(ie.Transactions.OrderByDescending(t => t.CreatedDate).Where(t => t.TransactionType == type).Take(10).Skip((page - 1) * 10));
            }
        }

        public void DisableAll(String identificationCode)
        {
            List<Transaction> transactions = ie.Transactions.Where(t => t.IdentificationCode == identificationCode && t.TransactionStatus == 0).ToList();
            foreach (var item in transactions)
            {
                item.TransactionStatus = 2;
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
    }
}
