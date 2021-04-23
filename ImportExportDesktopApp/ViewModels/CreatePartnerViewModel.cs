using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Utils;
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
    class CreatePartnerViewModel : BaseNotifyPropertyChanged
    {
        private Partner _partner;
        public String _identityCardId { get; set; }
        private PartnerType _selectedType;
        private ObservableCollection<PartnerType> _types;
        private ObservableCollection<IdentityCard> _identitycards;

        public ICommand AddIdentityCardCommand { get; set; }
        public ICommand AddPartnerCommnand { get; set; }

        private PartnerDataTransfer _partnerDataTransfer;

        public CreatePartnerViewModel()
        {
            Partner = new Partner();
            Partner.IdentityCards = new List<IdentityCard>();
            IdentityCards = new ObservableCollection<IdentityCard>();
            AddIdentityCardCommand = new RelayCommand<String>(p => { return true; }, AddIdentityCard);
            AddPartnerCommnand = new RelayCommand<Partner>((p) => { return true; }, AddPartner);
            _partnerDataTransfer = new PartnerDataTransfer();
            Types = _partnerDataTransfer.GetTypes();
        }

        private void AddPartner(Partner partner)
        {

            if (IsSelectedType())
            {
                if (CardListIsEmpty())
                {
                    MessageBox.Show("Please add at least one identity card!");
                }
                else
                {
                    Partner.PartnerTypeId = SelectedType.PartnerTypeId;
                    try
                    {
                        _partnerDataTransfer.CreatePartner(partner);
                        MessageBox.Show("Add successfuly!");
                        ResetPartner();
                    }
                    catch
                    {
                        MessageBox.Show("Add Error!");
                    }
                }

            }
            else
            {
                MessageBox.Show("Please choose at least one type!");
            }
        }

        private void AddIdentityCard(String identityCard)
        {
            IdentityCard newIdentityCard = new IdentityCard();
            newIdentityCard.IdentityCardId = identityCard;
            newIdentityCard.IdentityCardStatus = 0;
            newIdentityCard.CreatedDate = DateTime.Now;
            bool isExisted = false;
            foreach (var item in IdentityCards)
            {
                if (item.IdentityCardId.Equals(identityCard))
                {
                    isExisted = true;
                    break;
                }
            }
            if (!isExisted)
            {
                IdentityCards.Add(newIdentityCard);
                Partner.IdentityCards.Add(newIdentityCard);
            }
            else
            {
                MessageBox.Show("Card existed!");
            }
            IdentityCardId = "";
        }

        private void RemoveCard(IdentityCard identityCard)
        {
            Partner.IdentityCards.Remove(identityCard);
        }

        private bool IsSelectedType()
        {
            if (SelectedType == null)
            {

                return false;
            }
            return true;
        }

        private void ResetPartner()
        {
            Partner.Address = "";
            Partner.DisplayName = "";
            Partner.Email = "";
            Partner.PhoneNumber = "";
            Partner.IdentityCards.Clear();
            Partner.PartnerType = null;
            SelectedType = null;
        }

        private bool CardListIsEmpty()
        {
            if (Partner.IdentityCards == null)
            {
                return true;
            }

            if (Partner.IdentityCards.Count == 0)
            {
                return true;
            }

            return false;

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

        public String IdentityCardId
        {
            get { return _identityCardId; }
            set
            {
                _identityCardId = value;
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

        public ObservableCollection<PartnerType> Types
        {
            get { return _types; }
            set
            {
                _types = value;
                NotifyPropertyChanged();
            }
        }

        public String DisplayName
        {
            get { return Partner.DisplayName; }
            set
            {
                Partner.DisplayName = value;
                NotifyPropertyChanged();
            }
        }

        public String PhoneNumber
        {
            get { return Partner.PhoneNumber; }
            set
            {
                Partner.PhoneNumber = value;
                NotifyPropertyChanged();
            }
        }

        public String Address
        {
            get { return Partner.Address; }
            set
            {
                Partner.Address = value;
                NotifyPropertyChanged();
            }
        }

        public String Email
        {
            get { return Partner.Email; }
            set
            {
                Partner.Email = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<IdentityCard> IdentityCards
        {
            get { return _identitycards; }
            set
            {
                _identitycards = value;
                NotifyPropertyChanged();
            }
        }
    }
}
