using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Enums;
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
        private int _currentPage;
        private string _pagingInfo;
        private string _searchType;
        private string _searchPartner;
        private string _searchDate;
        private string _btnSearchVisibility;
        private bool _isMaxPage;
        private bool _isFirstPage;

        public List<String> TransactionTypes { get; set; }

        public ICommand NextPageCommand { get; set; }
        public ICommand BeforePageCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand CancelSearchCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public TransactionListViewModel()
        {

            TransactionTypes = new List<string>();
            TransactionTypes.Add(ETransactionType.Import.ToString());
            TransactionTypes.Add(ETransactionType.Export.ToString());

            BtnSearchVisibility = EVisibility.Hidden.ToString();

            CurrentPage = 1;
            _transactionDataTransfer = new TransactionDataTransfer();
            Transactions = _transactionDataTransfer.GetTransactions(_currentPage, -1);
            MaxPage = _transactionDataTransfer.GetMaxPage(10);
            SearchPartner = "";
            SetPagingInfo();


            IsFirstPage = true;
            if (CurrentPage == MaxPage)
            {
                IsMaxPage = true;
            }
            else
            {
                IsMaxPage = false;
            }

            NextPageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                NextPage();
            });
            BeforePageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                BeforePage();
            });
            SearchCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                SearchTransaction();
            });
            CancelSearchCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                CancelSearch();
            });
            RefreshCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                Refresh();
            });
        }

        public void Refresh()
        {
            CurrentPage = 1;
            ObservableCollection<Transaction> transactions = _transactionDataTransfer.GetTransactions(CurrentPage, -1);
            Transactions.Clear();
            foreach (var item in transactions)
            {
                Transactions.Add(item);
            }
            SetPagingInfo();
        }

        public void SearchTransaction()
        {
            BtnSearchVisibility = EVisibility.Visible.ToString();

            int type = -1;
            if (SearchType != null)
            {
                if (SearchType.Equals(ETransactionType.Import.ToString()))
                {
                    type = 0;
                }
                if (SearchType.Equals(ETransactionType.Export.ToString()))
                {
                    type = 1;
                }
            }

            Transactions = _transactionDataTransfer.SearchTransaction(type, SearchPartner, SearchDate);
            DisablePagingInfo();

        }

        private void DisablePagingInfo()
        {
            CurrentPage = 1;
            MaxPage = 1;
            PagingInfo = String.Format("Page {0} of {1}", 1, 1);
            IsMaxPage = true;
        }
        private void CancelSearch()
        {
            CurrentPage = 1;
            Transactions = _transactionDataTransfer.GetTransactions(_currentPage, -1);
            SearchDate = "";
            SearchPartner = "";
            SearchType = "";
            MaxPage = _transactionDataTransfer.GetMaxPage(10);
            IsMaxPage = false;
            BtnSearchVisibility = EVisibility.Hidden.ToString();
            SetPagingInfo();
        }
        public void NextPage()
        {
            if (CurrentPage < MaxPage)
            {
                CurrentPage++;
                IsFirstPage = false;
            }
            else
            {
                IsMaxPage = true;
            }
            Transactions = _transactionDataTransfer.GetTransactions(CurrentPage, -1);
            SetPagingInfo();
        }
        public void BeforePage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                IsMaxPage = false;
            }
            else
            {
                IsFirstPage = true;
            }
            Transactions = _transactionDataTransfer.GetTransactions(CurrentPage, -1);
            SetPagingInfo();
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
        public String SearchType
        {
            get { return _searchType; }
            set
            {
                _searchType = value;
                NotifyPropertyChanged();
            }
        }
        public String SearchDate
        {
            get { return _searchDate; }
            set
            {
                _searchDate = value;
                NotifyPropertyChanged();
            }
        }
        public String SearchPartner
        {
            get { return _searchPartner; }
            set
            {
                _searchPartner = value;
                NotifyPropertyChanged();
            }
        }

        public String BtnSearchVisibility
        {
            get { return _btnSearchVisibility; }
            set
            {
                _btnSearchVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsMaxPage
        {
            get { return _isMaxPage; }
            set
            {
                _isMaxPage = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsFirstPage
        {
            get { return _isFirstPage; }
            set
            {
                _isFirstPage = value;
                NotifyPropertyChanged();
            }
        }
    }
}
