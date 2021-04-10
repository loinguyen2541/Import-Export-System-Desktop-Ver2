using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/10/2021 9:59:48 AM 
*/

namespace ImportExportDesktopApp.ViewModels
{
    class TransactionListViewModel : BaseNotifyPropertyChanged
    {
        private ObservableCollection<Transaction> _transactions;
        private TransactionDataTransfer _transactionDataTransfer;
        private int _maxPage;

        private int _currentPage = 1;
        public TransactionListViewModel()
        {
            _transactionDataTransfer = new TransactionDataTransfer();
            Transactions = _transactionDataTransfer.GetTransactions(_currentPage, -1);
            MaxPage = _transactionDataTransfer.GetMaxPage(10);
        }

        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            set
            {
                _transactions = value;
                NotifyPropertyChanged();
            }
        }

        public int MaxPage
        {
            get { return _maxPage; }
            set
            {
                _maxPage = value;
                NotifyPropertyChanged();
            }
        }
    }
}
