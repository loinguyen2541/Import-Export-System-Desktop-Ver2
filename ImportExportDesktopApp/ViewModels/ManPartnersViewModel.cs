using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.HttpServices;
using ImportExportDesktopApp.Utils;
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
    class ManPartnersViewModel : BaseNotifyPropertyChanged
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

        private PartnerDataTransfer _partnerDataTransfer;
        private CardDataTransfer _cardDataTransfer;
        private AccountDataTransfer _accountDataTransfer;
        public Partner TableSelectedItem { get; set; }
        private bool _isLoading;
        public ICommand SearchCommand { get; set; }
        public ICommand AddPartnerCommnand { get; set; }
        public ICommand TableDoubleClickCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand BeforePageCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand CancelSearchCommand { get; set; }
        public ICommand AddIdentityCardComment { get; set; }
        public ManPartnersViewModel()
        {

            _partnerDataTransfer = new PartnerDataTransfer();
            _cardDataTransfer = new CardDataTransfer();
            _accountDataTransfer = new AccountDataTransfer();

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
            AddPartnerCommnand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                AddPartner();
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
            AddIdentityCardComment = new RelayCommand<object>(p => { return true; }, p =>
            {
                AddIdentityCard();
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
        }

        private void OpenDialog()
        {
            //Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //Partner newPartner = HttpService.GetPartnerById(TableSelectedItem.PartnerId);
            //Pagination<Transaction> transactions = HttpService.GetTransactionByPartxnerId(1, 3, TableSelectedItem.PartnerId);
            //PartnerDetailWindow window = new PartnerDetailWindow(newPartner, transactions);
            //window.ShowDialog();
        }

        private void AddPartner()
        {
            bool check = CheckValid();
            if (check)
            {
                if (ListIdentityCards.Count == 0)
                {
                    Visibility = "Visible";
                    ErrorMessage = "Partner must have at least one identity card";
                }
                else
                {
                    //add account
                    bool checkAccount = AddAccount();
                    if (checkAccount)
                    {
                        //add partner
                        Partner.PartnerType = SelectedType;
                        Partner.Username = _account.Username;
                        var partner = _partnerDataTransfer.CreatePartner(Partner);
                        if (partner != null)
                        {
                            //add card
                            foreach (var item in ListIdentityCards)
                            {
                                item.IdentityCardStatus = 0;
                                item.PartnerId = Partner.PartnerId;
                            }
                            Partner.IdentityCards = _cardDataTransfer.InsertCard(ListIdentityCards);
                        }
                        MessageBoxResult result = MessageBox.Show("Add Partner Success", "Confirmation");
                        Partner = null;
                    }
                    else
                    {
                        Visibility = "Visible";
                        ErrorMessage = "This name is existed";
                    }
                }
            }
        }
        private bool AddAccount()
        {
            string username = Regex.Replace(Partner.DisplayName, @"\s+", "");
            string password = RandomPassword();
            Account account = new Account() { Password = password, Username = username, RoleId = 3, Status = 0};
            _account = _accountDataTransfer.InsertAccount(account);
            return _account == null ? false : true;
        }

        private string RandomPassword()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void AddIdentityCard()
        {
            IdentityCard card = new IdentityCard();
            card.IdentityCardStatus = 0;
            card.IdentityCardId = CardId;
            card.CreatedDate = DateTime.Now;
            card.PartnerId = 0;

            bool check = true;
            if(ListIdentityCards.Count != 0)
            {
                foreach (var item in ListIdentityCards)
                {
                    if (item.IdentityCardId.Equals(CardId))
                    {
                        check = false;
                        break;
                    }
                }
            }

            if (check)
            {
                ListIdentityCards.Add(card);
            }
            else
            {
                Visibility = "Visible";
                ErrorMessage = "Identity card is existed";
            }

            CardId = "";
        }

        private bool CheckValid()
        {
            if (Partner != null)
            {
                if (Partner.DisplayName == null || Partner.Email == null || Partner.PhoneNumber == null)
                {
                    Visibility = "Visible";
                    ErrorMessage = "Some fields can not be null";
                    return false;
                }
                if (Partner.DisplayName.Length == 0 || Partner.Email.Length == 0 || Partner.PhoneNumber.Length == 0)
                {
                    Visibility = "Visible";
                    ErrorMessage = "Some fields can not be null";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            Visibility = "Visible";
            ErrorMessage = "Some fields can not be null";
            return false;
        }
        public void SearchSchedules()
        {
            IsSearch = true;
            Partners = _partnerDataTransfer.SearchPartner(SelectedType.PartnerTypeId, Search);
        }

        public void NextPage()
        {
            CurrentPage++;
            Partners = _partnerDataTransfer.GetAllWithPaging(CurrentPage);
        }
        public void BeforePage()
        {
            CurrentPage--;
            Partners = _partnerDataTransfer.GetAllWithPaging(CurrentPage);
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
    }
}
