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

        public void Save()
        {
            ie.SaveChanges();
        }
    }
}
