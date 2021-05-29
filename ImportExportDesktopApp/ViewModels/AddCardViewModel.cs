using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ImportExportDesktopApp.ViewModels
{
    class AddCardViewModel : BaseNotifyPropertyChanged
    {
        public int PartnerId { get; set; }
        public ObservableCollection<IdentityCard> _identityCards;
        private CardDataTransfer _cardDataTransfer;
        public ICommand ClearCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand AddCommand { get; set; }

        private String _txtCardId;

        SerialPort port1;
        int BaudRate = 9600;

        public AddCardViewModel()

        {
            _identityCards = new ObservableCollection<IdentityCard>();
            _cardDataTransfer = new CardDataTransfer();
            ClearCommand = new RelayCommand<object>((p) => { return true; }, (p) => { Clear(); });
            SaveCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AddCard(); });
            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AddCardManual(); });
            ReadSerial();
        }

        private void Clear()
        {
            IdentityCards.Clear();
        }

        private void AddCard()
        {
            try
            {
                _cardDataTransfer.InsertCard(IdentityCards);
                MessageBox.Show("Success!!");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error!! " + e.Message);
            }

        }

        private void AddCardManual()
        {
            IdentityCard identityCard = new IdentityCard();
            identityCard.IdentityCardId = TxtCardId.Trim();
            identityCard.IdentityCardStatus = 0;
            identityCard.CreatedDate = DateTime.Now;
            identityCard.PartnerId = PartnerId;

            IdentityCard identityCardCheck = IdentityCards.Where(i => i.IdentityCardId == TxtCardId.Trim()).SingleOrDefault();
            if (identityCardCheck == null)
            {
                if (!_cardDataTransfer.IsExist(TxtCardId.Trim()))
                {
                    IdentityCards.Add(identityCard);
                }
                else
                {
                    MessageBox.Show("Card is existed in system!!!");
                }
            }
            else
            {
                MessageBox.Show("Duplicated card!!!");
            }

        }
        public void ReadSerial()
        {
            port1 = new SerialPort();
            port1.DataReceived += new SerialDataReceivedEventHandler(DataRecive1);
            if (!port1.IsOpen)
            {
                try
                {
                    port1.PortName = "COM6";
                    port1.BaudRate = BaudRate;
                    port1.Open();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public void DataRecive1(object obj, SerialDataReceivedEventArgs e)
        {
            String value = port1.ReadLine();
            if (value != String.Empty)
            {
                if (value.Trim().StartsWith("uid"))
                {
                    String[] values = value.Split(':');
                    if (values.Length == 2)
                    {
                        String cardId = values[1];
                        IdentityCard identityCard = new IdentityCard();
                        identityCard.IdentityCardId = cardId.Trim();
                        identityCard.IdentityCardStatus = 0;
                        identityCard.CreatedDate = DateTime.Now;
                        identityCard.PartnerId = PartnerId;

                        App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                        {
                            IdentityCard identityCardCheck = IdentityCards.Where(i => i.IdentityCardId == cardId.Trim()).SingleOrDefault();
                            if (identityCardCheck == null)
                            {
                                if (!_cardDataTransfer.IsExist(cardId.Trim()))
                                {
                                    IdentityCards.Add(identityCard);
                                }
                            }
                        });
                    }
                }
            }
        }

        private void CloseSerialOnExit()
        {
            try
            {
                if (port1 != null)
                {
                    port1.DiscardInBuffer();
                    port1.DiscardOutBuffer();
                    port1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<IdentityCard> IdentityCards
        {
            get { return _identityCards; }
            set
            {
                _identityCards = value;
                NotifyPropertyChanged();
            }
        }

        public String TxtCardId
        {
            get { return _txtCardId; }
            set
            {
                _txtCardId = value;
                NotifyPropertyChanged();
            }
        }
    }
}
