using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private string _pagingInfo;
        public ICommand NextPageCommand { get; set; }
        public ICommand BeforePageCommand { get; set; }
        public TransactionListViewModel()
        {
            _transactionDataTransfer = new TransactionDataTransfer();
            Transactions = _transactionDataTransfer.GetTransactions(_currentPage, -1);
            MaxPage = _transactionDataTransfer.GetMaxPage(10);
            SetPagingInfo();
            NextPageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                NextPage();
            });
            BeforePageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                BeforePage();
            });
        }

        public void NextPage()
        {
            CurrentPage++;
            Transactions = _transactionDataTransfer.GetTransactions(CurrentPage, -1);
        }
        public void BeforePage()
        {
            CurrentPage--;
            Transactions = _transactionDataTransfer.GetTransactions(CurrentPage, -1);
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

        private void SetPagingInfo()
        {
            PagingInfo = String.Format("Page {0} of {1}", CurrentPage, MaxPage);
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

        public String PagingInfo
        {
            get { return _pagingInfo; }
            set
            {
                _pagingInfo = value;
                NotifyPropertyChanged();
            }
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                NotifyPropertyChanged();
            }
        }
    }
}
