using ImportExportDesktopApp.Models;
using ImportExportDesktopApp.Services;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class ManageTransactionModel : BaseNotifyPropertyChanged
    {
        private Transaction _trans;
        public List<String> ListStatus { get; }
        public List<Partner> ListPartner { get; }
        public String selectedItemCbxStatus { get; set; }
        public Partner selectedItemCbxPartner { get; set; }
        public String timeIn { get; set; }
        public String timeOut { get; set; }
        public float weightIn { get; set; }
        public float weightOut { get; set; }
        public ICommand AddTransactionComman { get; set; }
        private PartnerHttpService partnerService { get; set; }
        public float totalweight { get; set; }
        private String _closeSearchVisibility;

        public Pagination<Transaction> Transactions { get; set; }
        public QueryParams Paging { get; set; }
        public List<String> Types { get; }
        public Transaction SelectedTransaction { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand CloseSearchCommand { get; set; }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand GetNextPageCommand { get; set; }
        public ICommand GetBeforePageCommand { get; set; }
        private String _txtPageInfo;
        private bool _isMaxPage;
        private bool _isFirstPage;
        TransactionHttpService transactionHttpService;
        public int transactionID { get; set; }

        public ManageTransactionModel()
        {
            CloseSearchVisibility = "Hidden";
            transactionHttpService = new TransactionHttpService();
            partnerService = new PartnerHttpService();
            ListPartner = partnerService.GetPartnerActive();
            ListStatus = transactionHttpService.GetTransactionStatus();
            selectedItemCbxStatus = ListStatus.First();
            selectedItemCbxPartner = ListPartner.First();
            timeIn = DateTime.Now.ToString("hh:mm:ss tt");
            timeOut = DateTime.Now.ToString("hh:mm:ss tt");
            CreateNewTransaction();
            AddTransactionComman = new RelayCommand<Transaction>((p) => { return true; }, AddTransaction);

            Transactions = transactionHttpService.GetTransaction(1, 5, null, null, null).Result;
            Paging = new QueryParams();
            Paging.Size = Transactions.Size;
            Paging.Page = Transactions.Page;

            _txtPageInfo = String.Format("Page {0} of {1}", Transactions.Page, Transactions.TotalPage);
            CheckPage();

            DoubleClickCommand = new RelayCommand<Transaction>((p) => { return true; }, p =>
            {
                SearchTransactionByID();
            });
            SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                SearchTransaction();
            });
            GetNextPageCommand = new RelayCommand<QueryParams>((p) => { return true; }, GetNextPage);

            GetBeforePageCommand = new RelayCommand<QueryParams>((p) => { return true; }, GetBeforePage);
            CloseSearchCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CloseSearch();
            });
        }

        private void AddTransaction(Transaction trans)
        {
            trans.PartnerId = selectedItemCbxPartner.PartnerId;
            trans.TimeIn = _trans.TimeIn + convertTime(timeIn);
            trans.TimeOut = _trans.TimeOut + convertTime(timeOut);
            bool result = transactionHttpService.PostTransaction(trans);
            if (result)
            {
                MessageBox.Show("Add successfuly!");
            }
            else
            {
                MessageBox.Show("Please choose at least one type!");
            }
        }

        private void CreateNewTransaction()
        {
            Transaction = new Transaction();
            Transaction.CreatedDate = DateTime.Now;
            Transaction.TimeIn = DateTime.Now;
            Transaction.TimeOut = DateTime.Now;
            Transaction.GoodsId = 1;
        }

        private async void CloseSearch()
        {
            CloseSearchVisibility = "Hidden";
            Pagination<Transaction> newTransaction = await transactionHttpService.GetTransaction(1, 5, null, null, null);
            Clear(newTransaction);
            TxtPageInfo = String.Format("Page {0} of {1}", Transactions.Page, Transactions.TotalPage);
            CheckPage();
        }

        // Task will automatic update list afer 5 seconds
        private async void AutoRefreshTask()
        {
            Random rnd = new Random();

            while (true)
            {
                Pagination<Transaction> newTransaction;
                if (CloseSearchVisibility.Equals("Hidden"))
                {
                    newTransaction = await transactionHttpService.GetTransaction(Paging.Page, Paging.Size, null, null, null);
                }
                else
                {
                    newTransaction = await transactionHttpService.GetTransaction(Paging.Page, Paging.Size, Paging.Date, Paging.Search, Paging.Type);
                }
                Clear(newTransaction);
                CheckPage();
                TxtPageInfo = String.Format("Page {0} of {1}", Transactions.Page, Transactions.TotalPage);
                Thread.Sleep(5000);
            }
        }

        public void StartThread()
        {
            Thread trd = new Thread(new ThreadStart(this.AutoRefreshTask));
            trd.IsBackground = true;
            trd.Start();
        }


        public Transaction Transaction
        {
            get { return _trans; }
            set
            {
                _trans = value;
                NotifyPropertyChanged();
            }
        }
        private TimeSpan convertTime(String time)
        {
            String temp = "";
            if (time.Contains("/"))
            {
                temp = time.Substring(time.IndexOf(' ') + 1);
            }
            else
            {
                temp = time;
            }
            return DateTime.ParseExact(temp, "h:mm:ss tt", CultureInfo.InvariantCulture).TimeOfDay;
        }

        public async void SearchTransaction()
        {
            CloseSearchVisibility = "Visible";
            Pagination<Transaction> newTransaction = await transactionHttpService.GetTransaction(Paging.Page, Paging.Size, Paging.Date, Paging.Search, Paging.Type);
            Clear(newTransaction);
            CheckPage();
        }

        public void SearchTransactionByID()
        {
            //Transaction newTransaction = transactionHttpService.GetTransactionByID(SelectedTransaction.TransactionId);
            //DetailTransactinoWindow detailTransactinoWindow = new DetailTransactinoWindow(newTransaction);
            //detailTransactinoWindow.ShowDialog();
        }


        public async void GetNextPage(QueryParams query)
        {
            query.Page = query.Page + 1;
            Pagination<Transaction> transaction = await transactionHttpService.GetTransaction(query.Page, query.Size, query.Date, query.Search, query.Type);
            Clear(transaction);
            TxtPageInfo = String.Format("Page {0} of {1}", Transactions.Page, Transactions.TotalPage);
            CheckPage();
        }

        public async void GetBeforePage(QueryParams query)
        {
            query.Page = query.Page - 1;
            Pagination<Transaction> transaction = await transactionHttpService.GetTransaction(query.Page, query.Size, query.Date, query.Search, query.Type);
            Clear(transaction);
            TxtPageInfo = String.Format("Page {0} of {1}", Transactions.Page, Transactions.TotalPage);
            CheckPage();
        }

        public void CheckPage()
        {
            if (Transactions.Page >= Transactions.TotalPage)
            {
                IsMaxPage = true;
            }
            else
            {
                IsMaxPage = false;
            }

            if (Transactions.Page <= 1)
            {
                IsFirstPage = true;
            }
            else
            {
                IsFirstPage = false;
            }
        }

        public String TxtPageInfo
        {
            get { return _txtPageInfo; }
            set
            {
                _txtPageInfo = value;
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

        public String CloseSearchVisibility
        {
            get { return _closeSearchVisibility; }
            set
            {
                _closeSearchVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public void Clear(Pagination<Transaction> transations)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Transactions.Page = transations.Page;
                Transactions.Size = transations.Size;
                Transactions.TotalPage = transations.TotalPage;
                Transactions.Data.Clear();
                foreach (var item in transations.Data)
                {
                    Transactions.Data.Add(item);
                }
            });
        }
    }
}
