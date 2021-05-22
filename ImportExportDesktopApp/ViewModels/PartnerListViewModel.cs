using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.HttpServices;
using ImportExportDesktopApp.Utils;
using ImportExportDesktopApp.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class PartnerListViewModel : BaseNotifyPropertyChanged
    {
        private ObservableCollection<Partner> _partners;
        //private QueryParams _paging;
        private ObservableCollection<PartnerType> _types;
        private Partner _partner;

        private bool _isSearch;
        private string _search;
        private PartnerType _selectedType;
        private String _visibility;
        private String _errorMessage;
        private int _currentPage = 1;
        private String _cardId;
        private List<IdentityCard> _listCard;
        private Account _account = null;
        private string _pagingInfo;
        private int _maxPage;
        private bool _isMaxPage;
        private bool _isFirstPage;

        private PartnerDataTransfer _partnerDataTransfer;
        private CardDataTransfer _cardDataTransfer;
        private AccountDataTransfer _accountDataTransfer;
        private AccountApiService _accountService;
        public Partner TableSelectedItem { get; set; }
        private bool _isLoading;
        public ICommand SearchCommand { get; set; }
        public ICommand TableDoubleClickCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand BeforePageCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand CancelSearchCommand { get; set; }

        public PartnerListViewModel()
        {

            _partnerDataTransfer = new PartnerDataTransfer();
            _cardDataTransfer = new CardDataTransfer();
            _accountDataTransfer = new AccountDataTransfer();
            _accountService = new AccountApiService();
            MaxPage = _partnerDataTransfer.GetMaxPage(10);

            IsFirstPage = true;
            if (CurrentPage == MaxPage)
            {
                IsMaxPage = true;
            }
            else
            {
                IsMaxPage = false;
            }

            Task.Run(() =>
            {
                Init();
            });
            _visibility = "Hidden";
            _listCard = new List<IdentityCard>();

            Partner = new Partner();
            SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                SearchSchedules();
            });
            RefreshCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                Refresh();
            });
            CancelSearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                CancelSearch();
            });
            NextPageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                NextPage();
            });
            BeforePageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                BeforePage();
            });
            TableDoubleClickCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                OpenDialog();
            });
        }

        public void Init()
        {
            try
            {
                Partners = _partnerDataTransfer.GetAllWithPaging(_currentPage);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            Types = _partnerDataTransfer.GetTypes();

            IsLoading = false;
            IsSearch = false;
            Search = "";
            SelectedType = Types[0];
            SetPagingInfo();
        }

        private void OpenDialog()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            PartnerDetailWindow window = new PartnerDetailWindow(TableSelectedItem.PartnerId);
            window.ShowDialog();
        }

        public void SearchSchedules()
        {
            IsSearch = true;
            Partners = _partnerDataTransfer.SearchPartner(SelectedType.PartnerTypeId, Search);

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
            Partners = _partnerDataTransfer.GetAllWithPaging(CurrentPage);
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
            Partners = _partnerDataTransfer.GetAllWithPaging(CurrentPage);
            SetPagingInfo();
        }

        public void CancelSearch()
        {
            IsSearch = false;
            CurrentPage = 1;
            Partners = _partnerDataTransfer.GetAllWithPaging(CurrentPage);
            Search = "";
            SelectedType = Types[0];
        }

        public void Refresh()
        {
            IsLoading = true;
            CurrentPage = 1;
            ObservableCollection<Partner> partners = _partnerDataTransfer.GetAllWithPaging(CurrentPage);
            Partners.Clear();
            foreach (var item in partners)
            {
                Partners.Add(item);
            }
            IsLoading = false;

        }
        private void SetPagingInfo()
        {
            PagingInfo = String.Format("Page {0} of {1}", CurrentPage, MaxPage);
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged();
            }
        }

        public PartnerType SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSearch
        {
            get { return _isSearch; }
            set
            {
                _isSearch = value;
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

        public ObservableCollection<Partner> Partners
        {
            get { return _partners; }
            set
            {
                _partners = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<PartnerType> Types
        {
            get { return _types; }
            set
            {
                _types = value;
                NotifyPropertyChanged();
            }
        }

        public String Search
        {
            get { return _search; }
            set
            {
                _search = value;
                NotifyPropertyChanged();
            }
        }

        public String Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                NotifyPropertyChanged();
            }
        }

        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public String CardId
        {
            get { return _cardId; }
            set
            {
                _cardId = value;
                NotifyPropertyChanged();
            }
        }

        public Partner Partner
        {
            get { return _partner; }
            set
            {
                _partner = value;
                NotifyPropertyChanged();
            }
        }

        public List<IdentityCard> ListIdentityCards
        {
            get { return _listCard; }
            set
            {
                _listCard = value;
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
        public int MaxPage
        {
            get { return _maxPage; }
            set
            {
                _maxPage = value;
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
