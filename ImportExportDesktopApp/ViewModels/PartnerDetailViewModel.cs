using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Enums;
using ImportExportDesktopApp.Utils;
using ImportExportDesktopApp.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class PartnerDetailViewModel : BaseNotifyPropertyChanged
    {
        private int partnerId;
        private Partner partner;
        private String selectedStatus;
        private List<String> _partnerStatuses;
        private List<String> _partnerTypes;
        private String _selectedType;
        private bool _isReadOnly;
        private bool _isEnable;
        private string _visibilityBlockItem;
        private ObservableCollection<Transaction> _listTranHistoryByPartner;
        private ObservableCollection<IdentityCard> _listCardByPartner;
        private String _unlockPartnerVisibility;
        private Account _account;
        private SystemConfig _maxSlots;
        private String _partnerBg;

        public IdentityCard SelectedCard { get; set; }
        public ICommand GetNextPageCommand { get; set; }
        public ICommand GetBeforePageCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand TurnOnEditCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public ICommand CreateCardCommand { get; set; }
        public ICommand DeleteCardCommand { get; set; }
        public ICommand ActiveCardCommand { get; set; }
        public ICommand UnlockPartnerCommand { get; set; }

        private String _txtPageInfo;
        private String _cancelButtonVisibility;
        private EditButtonContentValue _editButtonContent;
        private bool _isMaxPage;
        private bool _isFirstPage;
        private readonly PartnerDataTransfer _partnerDataTransfer;
        private readonly TransactionDataTransfer _transactionDataTransfer;
        private readonly CardDataTransfer _cardDataTransfer;
        private readonly AccountDataTransfer _accountDataTransfer;
        private readonly SystemConfigDataTransfer _systemConfigDataTransfer;
        private int _transactionCurrentPage;
        private int _transactionMaxPage;

        public PartnerDetailViewModel()
        {
            _partnerDataTransfer = new PartnerDataTransfer();
            _transactionDataTransfer = new TransactionDataTransfer();
            _cardDataTransfer = new CardDataTransfer();
            _accountDataTransfer = new AccountDataTransfer();
            _systemConfigDataTransfer = new SystemConfigDataTransfer();
            _maxSlots = _systemConfigDataTransfer.GetMaximumCanceledSchechule();
            PartnerBg = "White";
            IsReadOnly = true;
            IsEnable = false;
            EditButtonContent = EditButtonContentValue.Edit;
            CreateCardCommand = new RelayCommand<Object>((p) => { return true; }, (p) => { OpenCardDialog(); });
            TurnOnEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => { TurnOnEditTextBox(); });
            CancelEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CancelEditTextBox(); });
            GetNextPageCommand = new RelayCommand<object>((p) => { return true; }, p => { GetNextPage(); });
            GetBeforePageCommand = new RelayCommand<object>((p) => { return true; }, p => { GetBeforePage(); });
            DeleteCardCommand = new RelayCommand<IdentityCard>((p) => { return true; }, DeleteCard);
            ActiveCardCommand = new RelayCommand<IdentityCard>((p) => { return true; }, ActiveCard);
            UnlockPartnerCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                Unlockpartner();
            });
            EditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                UpdatePartner();
            });
            CancelButtonVisibility = "Hidden";

        }
        public void SetInit(int id)
        {
            TransactionCurrentPage = 1;
            partnerId = id;
            Partner = _partnerDataTransfer.GetByID(this.partnerId);
            ListCardByPartner = _cardDataTransfer.GetAllCardsByPartnerId(this.partnerId);
            ListTranHistoryByPartner = _transactionDataTransfer.GetAllTransactionByPartner(1, partnerId);
            _account = _accountDataTransfer.GetByUsername(Partner.Username);
            TransactionMaxPage = _transactionDataTransfer.GetMaxPageByPartner(10, id);
            PartnerStatuses = new List<string>();
            PartnerStatuses.Add("Active");
            PartnerStatuses.Add("Block");

            PartnerTypes = new List<string>();
            PartnerTypes.Add("Customer");
            PartnerTypes.Add("Provider");

            if (_account.NumberCanceled >= int.Parse(_maxSlots.AttributeValue))
            {
                UnlockPartnerVisibility = EVisibility.Visible.ToString();
                PartnerBg = "OrangeRed";
            }
            else
            {
                UnlockPartnerVisibility = EVisibility.Hidden.ToString();
                PartnerBg = "White";
            }

            SelectedType = Partner.PartnerTypeId == 1 ? "Customer" : "Provider";
            SelectedStatus = Partner.PartnerStatus == 0 ? "Active" : "Block";
            TxtPageInfo = String.Format("Page {0} of {1}", TransactionCurrentPage, TransactionMaxPage);
            CheckPage();

        }

        public void OpenCardDialog()
        {
            AddCardWindow addCardWindow = new AddCardWindow(partnerId);
            addCardWindow.ShowDialog();
        }

        private void Unlockpartner()
        {
            _account.NumberCanceled = 0;
            _accountDataTransfer.UpdateAccount(_account);
            UnlockPartnerVisibility = EVisibility.Hidden.ToString();
            PartnerBg = "White";
        }

        public void GetNextPage()
        {
            TransactionCurrentPage++;
            ListTranHistoryByPartner = _transactionDataTransfer.GetAllTransactionByPartner(TransactionCurrentPage, partnerId);

            TxtPageInfo = String.Format("Page {0} of {1}", TransactionCurrentPage, TransactionMaxPage);
            CheckPage();
        }

        public void GetBeforePage()
        {
            TransactionCurrentPage--;
            ListTranHistoryByPartner = _transactionDataTransfer.GetAllTransactionByPartner(TransactionCurrentPage, partnerId);

            TxtPageInfo = String.Format("Page {0} of {1}", TransactionCurrentPage, TransactionMaxPage);
            CheckPage();
        }

        private void TurnOnEditTextBox()
        {
            if (IsReadOnly == true)
            {
                IsReadOnly = false;
                IsEnable = true;
                EditButtonContent = EditButtonContentValue.Save;
                CancelButtonVisibility = "Visible";
            }
            else if (IsReadOnly == false)
            {
                IsReadOnly = true;
                IsEnable = false;
                EditButtonContent = EditButtonContentValue.Edit;
                CancelButtonVisibility = "Hidden";
                UpdatePartner();
            }
        }

        private void UpdatePartner()
        {
            if (SelectedType == "Customer")
            {
                Partner.PartnerTypeId = 1;
            }
            else if (SelectedType == "Provider")
            {
                Partner.PartnerTypeId = 2;
            }
            else
            {
                MessageBox.Show("Plesase choose partner type!");
            }

            if (SelectedStatus == "Active")
            {
                Partner.PartnerStatus = 0;
            }
            else if (SelectedStatus == "Block")
            {
                Partner.PartnerStatus = 1;
            }
            else
            {
                MessageBox.Show("Plesase choose partner status!");
            }

            try
            {
                _partnerDataTransfer.UpdatePartner(Partner);

                MessageBox.Show("Update successfully!");
            }
            catch (Exception e)
            {
                MessageBox.Show("Fail to update!" + e.Message);
            }

        }

        private void CancelEditTextBox()
        {
            if (IsReadOnly == true)
            {
                IsReadOnly = false;
                IsEnable = true;
                EditButtonContent = EditButtonContentValue.Save;
                CancelButtonVisibility = "Visible";
            }
            else if (IsReadOnly == false)
            {
                IsReadOnly = true;
                IsEnable = false;
                EditButtonContent = EditButtonContentValue.Edit;
                CancelButtonVisibility = "Hidden";
            }
        }

        private void DeleteCard(IdentityCard identityCard)
        {
            _cardDataTransfer.BlockCard(SelectedCard);
            ClearCard();
        }

        private void ActiveCard(IdentityCard identityCard)
        {
            _cardDataTransfer.ActiveCard(SelectedCard);
            ClearCard();
        }

        public void CheckPage()
        {
            if (TransactionCurrentPage >= TransactionMaxPage)
            {
                IsMaxPage = true;
            }
            else
            {
                IsMaxPage = false;
            }

            if (TransactionCurrentPage <= 1)
            {
                IsFirstPage = true;
            }
            else
            {
                IsFirstPage = false;
            }
        }

        public void ClearCard()
        {
            ListCardByPartner = _cardDataTransfer.GetAllCardsByPartnerId(partnerId);
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

        public List<String> PartnerStatuses
        {
            get { return _partnerStatuses; }
            set
            {
                _partnerStatuses = value;
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

        public EditButtonContentValue EditButtonContent
        {
            get { return _editButtonContent; }
            set
            {
                _editButtonContent = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                NotifyPropertyChanged();
            }
        }

        public String CancelButtonVisibility
        {
            get { return _cancelButtonVisibility; }
            set
            {
                _cancelButtonVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                NotifyPropertyChanged();
            }
        }

        public String SelectedStatus
        {
            get { return this.selectedStatus; }
            set
            {
                this.selectedStatus = value;
                NotifyPropertyChanged();
            }
        }
        public Partner Partner
        {
            get { return this.partner; }
            set
            {
                this.partner = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<IdentityCard> ListCardByPartner
        {
            get { return this._listCardByPartner; }
            set
            {
                this._listCardByPartner = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<Transaction> ListTranHistoryByPartner
        {
            get { return _listTranHistoryByPartner; }
            set
            {
                _listTranHistoryByPartner = value;
                NotifyPropertyChanged();
            }
        }

        public String VisibilityBlockItem
        {
            get { return _visibilityBlockItem; }
            set
            {
                _visibilityBlockItem = value;
                NotifyPropertyChanged();
            }
        }

        public int TransactionCurrentPage
        {
            get { return _transactionCurrentPage; }
            set
            {
                _transactionCurrentPage = value;
                NotifyPropertyChanged();
            }
        }

        public int TransactionMaxPage
        {
            get { return _transactionMaxPage; }
            set
            {
                _transactionMaxPage = value;
                NotifyPropertyChanged();
            }
        }

        public List<String> PartnerTypes
        {
            get { return _partnerTypes; }
            set
            {
                _partnerTypes = value;
                NotifyPropertyChanged();
            }
        }

        public String SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                NotifyPropertyChanged();
            }
        }

        public String UnlockPartnerVisibility
        {
            get { return _unlockPartnerVisibility; }
            set
            {
                _unlockPartnerVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public String PartnerBg
        {
            get { return _partnerBg; }
            set
            {
                _partnerBg = value;
                NotifyPropertyChanged();
            }
        }
    }

    public enum EditButtonContentValue
    {
        Edit, Save
    }
}
