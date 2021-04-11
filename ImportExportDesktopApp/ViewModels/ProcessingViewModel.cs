using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Enums;
using ImportExportDesktopApp.Events;
using ImportExportDesktopApp.ScaleModels;
using ImportExportDesktopApp.Utils;
using Notifications.Wpf;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

/**
* @author Loi Nguyen
*
* @date - 4/8/2021 1:24:03 PM 
*/

namespace ImportExportDesktopApp.ViewModels
{
    class ProcessingViewModel : BaseNotifyPropertyChanged
    {
        private IEventAggregator _eventAggregator;

        private TransactionDataTransfer _transactionDataTransfer;
        private CardDataTransfer _cardDataTransfer;
        private SystemConfigDataTransfer _systemCongifDataTransfer;
        private GoodDataTransfer _goodDataTransfer;
        private InventoryDataTransfer _inventoryDataTransfer;
        private InventoryDetailDataTransfer _inventoryDetailDataTransfer;
        private ScheduleDataTransfer _scheduleDataTransfer;
        private TimeTemplateItemDataTransfer _timeTemplateItemDataTransfer;

        private ObservableCollection<Transaction> _processingTransaction;
        private ObservableCollection<Transaction> _successTransaction;
        private int _storageCapacity = 0;

        //gate field
        private String _partnerNameGate1;
        private String _partnerTypeNameGate1;
        private String _partnerNameGate2;
        private String _partnerTypeNameGate2;
        private String _weightGate1;
        private String _weightGate2;
        private String _messageGate1;
        private String _messageGate2;

        //Command
        public ICommand CreateTransactionGate2Command { get; set; }

        //Gate Exception handle
        private String _gate1ButtonVisibility;
        private String _gate2ButtonVisibility;
        private TransactionScale _transactionScaleGate1;
        private Partner _partnerGate1;
        private Schedule _scheduleGate1;
        private TransactionScale _transactionScaleGate2;
        private Partner _partnerGate2;
        private Schedule _scheduleGate2;
        private bool _isSlovingExeptionGate1;
        private bool _isSlovingExeptionGate2;

        public ProcessingViewModel(IEventAggregator ea)
        {

            _isSlovingExeptionGate1 = false;
            _isSlovingExeptionGate2 = false;
            Gate1ButtonVisibility = EVisibility.Hidden.ToString();
            Gate2ButtonVisibility = EVisibility.Hidden.ToString();

            _transactionDataTransfer = new TransactionDataTransfer();
            _cardDataTransfer = new CardDataTransfer();
            _systemCongifDataTransfer = new SystemConfigDataTransfer();
            _goodDataTransfer = new GoodDataTransfer();
            _inventoryDataTransfer = new InventoryDataTransfer();
            _inventoryDetailDataTransfer = new InventoryDetailDataTransfer();
            _scheduleDataTransfer = new ScheduleDataTransfer();
            _timeTemplateItemDataTransfer = new TimeTemplateItemDataTransfer();

            _storageCapacity = _systemCongifDataTransfer.GetStorageCappacity();
            ProcessingTransaction = _transactionDataTransfer.GetProcessingTransaction();
            SuccessTransaction = _transactionDataTransfer.GetSuccessTransaction();

            //Command 
            CreateTransactionGate2Command = new RelayCommand<object>(p => { return true; }, p =>
            {
                CreateTransactionGate2();
            });


            //Set event
            _eventAggregator = ea;

        }

        public bool CheckCard(TransactionScale transactionScale)
        {
            //_eventAggregator.GetEvent<ScaleExceptionEvent>().Publish("Check");

            if (transactionScale.Weight < 10)
            {
                return false;
            }

            Partner partner = _cardDataTransfer.CheckCard(transactionScale);
            UpdateGatePanel(partner, transactionScale);

            if (partner != null)
            {
                Schedule schedule = _scheduleDataTransfer.CheckSchedule(partner.PartnerId);
                Transaction transaction = _transactionDataTransfer.IsProcessing(transactionScale.Indentify);
                if (transaction == null)
                {
                    CreateTransaction(transactionScale, partner, schedule);
                    return true;
                }
                else
                {
                    if (transaction.Gate.Contains(transactionScale.Gate.ToString()))
                    {
                        return false;
                    }

                    Transaction newTransaction = UpdateTransaction(transaction, transactionScale, partner, schedule);
                    if (newTransaction == null)
                    {
                        return false;
                    }

                    if (partner.PartnerTypeId == 1)
                    {
                        float weight = newTransaction.WeightOut - newTransaction.WeightIn;
                        Good good = _goodDataTransfer.UpdateInventory(partner.PartnerTypeId, weight, _storageCapacity);
                        double storageCapacity20 = _storageCapacity * 0.2;

                        SendNotify(good.QuantityOfInventory, storageCapacity20);

                        // send good inventoey to main view model
                        _eventAggregator.GetEvent<UpdateInventoryEvent>().Publish(good.QuantityOfInventory + "");
                    }
                    else if (partner.PartnerTypeId == 2)
                    {
                        float weight = newTransaction.WeightIn - newTransaction.WeightOut;
                        Good good = _goodDataTransfer.UpdateInventory(partner.PartnerTypeId, weight, _storageCapacity);
                        double storageCapacity20 = _storageCapacity * 0.2;

                        SendNotify(good.QuantityOfInventory, storageCapacity20);

                        // send good inventoey to main view model
                        _eventAggregator.GetEvent<UpdateInventoryEvent>().Publish(good.QuantityOfInventory + "");
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
            }
            return false;
        }


        public bool CheckCardGate2(TransactionScale transactionScale)
        {
            if (transactionScale.Gate == EGate.Gate1 && _isSlovingExeptionGate1)
            {
                return false;
            }
            else if (transactionScale.Gate == EGate.Gate2 && _isSlovingExeptionGate2)
            {
                return false;
            }

            if (transactionScale.Weight < 10)
            {
                return false;
            }

            Partner partner = _cardDataTransfer.CheckCard(transactionScale);
            UpdateGatePanel(partner, transactionScale);

            if (partner != null)
            {
                Schedule schedule = _scheduleDataTransfer.CheckSchedule(partner.PartnerId);
                Transaction transaction = _transactionDataTransfer.IsProcessing(transactionScale.Indentify);
                if (transaction == null)
                {
                    if (transactionScale.Gate == EGate.Gate2)
                    {
                        Gate2ButtonVisibility = EVisibility.Visible.ToString();
                        TransactionScaleGate2 = transactionScale;
                        PartnerGate2 = partner;
                        ScheduleGate2 = schedule;
                        _isSlovingExeptionGate2 = true;
                        ScaleExeption scaleExeption = new ScaleExeption(transactionScale, partner, schedule);
                        _eventAggregator.GetEvent<ScaleExceptionEvent>().Publish(scaleExeption);
                        return false;
                    }
                    CreateTransaction(transactionScale, partner, schedule);
                    return true;
                }
                else
                {
                    if (transaction.Gate.Contains(transactionScale.Gate.ToString()))
                    {
                        return false;
                    }

                    Transaction newTransaction = UpdateTransaction(transaction, transactionScale, partner, schedule);
                    if (newTransaction == null)
                    {
                        return false;
                    }

                    if (partner.PartnerTypeId == 1)
                    {
                        float weight = newTransaction.WeightOut - newTransaction.WeightIn;
                        Good good = _goodDataTransfer.UpdateInventory(partner.PartnerTypeId, weight, _storageCapacity);
                        double storageCapacity20 = _storageCapacity * 0.2;

                        SendNotify(good.QuantityOfInventory, storageCapacity20);

                        // send good inventoey to main view model
                        _eventAggregator.GetEvent<UpdateInventoryEvent>().Publish(good.QuantityOfInventory + "");
                    }
                    else if (partner.PartnerTypeId == 2)
                    {
                        float weight = newTransaction.WeightIn - newTransaction.WeightOut;
                        Good good = _goodDataTransfer.UpdateInventory(partner.PartnerTypeId, weight, _storageCapacity);
                        double storageCapacity20 = _storageCapacity * 0.2;

                        SendNotify(good.QuantityOfInventory, storageCapacity20);

                        // send good inventoey to main view model
                        _eventAggregator.GetEvent<UpdateInventoryEvent>().Publish(good.QuantityOfInventory + "");
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
            }
            return false;
        }

        void SendNotify(float inventory, double storageCapacity20)
        {
            if (_storageCapacity - inventory <= storageCapacity20)
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Warning",
                    Message = "Storge capacity is almost full!!!!",
                    Type = NotificationType.Warning
                });
            }

            if (inventory <= storageCapacity20)
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Warning",
                    Message = "The warehouse is comming out of stock!!!!",
                    Type = NotificationType.Warning
                });
            }
        }


        public void CreateTransactionGate1()
        {
            CreateTransaction(TransactionScaleGate1, PartnerGate1, ScheduleGate1);
            UpdateTable();
        }

        public void CreateTransactionGate2()
        {
            CreateTransaction(TransactionScaleGate2, PartnerGate2, ScheduleGate2);
            UpdateTable();
            Gate2ButtonVisibility = EVisibility.Hidden.ToString();
            _isSlovingExeptionGate2 = false;
        }

        public void CreateTransaction(TransactionScale transactionScale, Partner partner, Schedule schedule)
        {
            Transaction transaction = new Transaction();
            transaction.PartnerId = partner.PartnerId;

            if (schedule != null)
            {
                transaction.IsScheduled = true;
            }
            else
            {
                transaction.IsScheduled = false;
            }

            transaction.CreatedDate = DateTime.Now;
            transaction.GoodsId = 1;
            transaction.TimeIn = DateTime.Now;
            transaction.WeightIn = transactionScale.Weight;
            transaction.WeightOut = 0;
            transaction.IdentificationCode = transactionScale.Indentify;
            transaction.TransactionStatus = 0;
            transaction.TransactionType = partner.PartnerTypeId == 1 ? 1 : 0;
            transaction.Gate = transactionScale.Gate.ToString();
            _transactionDataTransfer.InsertTransaction(transaction);
        }

        public Transaction UpdateTransaction(Transaction transaction, TransactionScale transactionScale, Partner partner, Schedule schedule)
        {
            float goodInventory = _goodDataTransfer.getInventory();
            if (!CheckWeight(partner.PartnerTypeId, transaction.WeightIn, transactionScale.Weight, goodInventory))
            {
                return null;
            }
            transaction.WeightOut = transactionScale.Weight;
            transaction.TimeOut = DateTime.Now;
            transaction.TransactionStatus = 1;
            transaction.Gate = transactionScale.Gate.ToString();
            Inventory inventory = _inventoryDataTransfer.CheckExist();
            if (inventory == null)
            {
                inventory = _inventoryDataTransfer.CreateInventoryToday(goodInventory);
            }
            transaction = _transactionDataTransfer.UpdateTransactionNotSaveChanges(transaction);
            float totalWeight = getTotalWeight(partner.PartnerTypeId, transaction.WeightIn, transaction.WeightOut);
            _inventoryDetailDataTransfer.InsertInventoryDetailNotSaveChanges(1, partner.PartnerId, partner.PartnerTypeId, totalWeight, inventory.InventoryId);
            if (schedule != null)
            {
                schedule.ActualWeight = totalWeight;
                schedule.ScheduleStatus = 1;
                _scheduleDataTransfer.UpdateSchedule(schedule);
            }
            else
            {
                _timeTemplateItemDataTransfer.updateTimetemplateItemWeight(transaction.TimeOut, totalWeight, partner.PartnerTypeId);
            }
            _transactionDataTransfer.Save();
            return transaction;
        }

        private void UpdateGatePanel(Partner partner, TransactionScale transactionScale)
        {
            if (transactionScale.Gate == EGate.Gate1)
            {
                if (partner != null)
                {
                    PartnerNameGate1 = "Partner: " + partner.DisplayName;
                    PartnerTypeNameGate1 = "Type: " + partner.PartnerType.PartnerTypeName;
                }
                else
                {
                    PartnerNameGate1 = "No partner found!!";
                }
                WeightGate1 = transactionScale.Weight + " kg";
            }
            else
            {
                if (partner != null)
                {
                    PartnerNameGate2 = "Partner: " + partner.DisplayName;
                    PartnerTypeNameGate2 = "Type:" + partner.PartnerType.PartnerTypeName;
                }
                else
                {
                    PartnerNameGate2 = "No partner found!!";
                }
                WeightGate2 = transactionScale.Weight + " kg";
            }
        }



        /// <summary>
        ///   Check the total weight is valid or not!!!
        /// </summary>
        /// <param name="partnerTypeId"></param>
        /// <param name="weightIn"></param>
        /// <param name="weightOut"></param>
        /// <param name="goodInventory"></param>
        /// <returns>
        /// return false if <br/>
        ///     customer: <br/>
        ///         -- WeightIn > WeighOut <br/>
        ///         -- TotalWeight > Inventory <br/>
        ///     provider: <br/>
        ///         -- WeightIn < WeightOut <br/>
        ///         -- TotalWeight > Current Capacity <br/>
        ///  else 
        ///     return true
        /// </returns>
        public bool CheckWeight(int partnerTypeId, float weightIn, float weightOut, float goodInventory)
        {
            // Export
            if (partnerTypeId == 1)
            {
                if (weightIn > weightOut)
                {
                    return false;
                }
                else if ((weightOut - weightIn) > goodInventory)
                {
                    return false;
                }
            }

            //Import
            else if (partnerTypeId == 2)
            {
                if (weightIn < weightOut)
                {
                    return false;
                }
                else if ((weightIn - weightOut) > (_storageCapacity - goodInventory))
                {
                    return false;
                }
            }

            // Other
            else
            {
                return false;
            }

            return true;
        }

        public float getTotalWeight(int partnerTypeId, float weightIn, float weightOut)
        {
            if (partnerTypeId == 1)
            {
                return weightOut - weightIn;
            }
            else if (partnerTypeId == 2)
            {
                return weightIn - weightOut;
            }
            else return 0;
        }
        public void UpdateTable()
        {
            ProcessingTransaction = _transactionDataTransfer.GetProcessingTransaction();
            SuccessTransaction = _transactionDataTransfer.GetSuccessTransaction();
        }

        public ObservableCollection<Transaction> ProcessingTransaction
        {
            get { return _processingTransaction; }
            set
            {
                _processingTransaction = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Transaction> SuccessTransaction
        {
            get { return _successTransaction; }
            set
            {
                _successTransaction = value;
                NotifyPropertyChanged();
            }
        }

        public String PartnerNameGate1
        {
            get { return _partnerNameGate1; }
            set
            {
                _partnerNameGate1 = value;
                NotifyPropertyChanged();
            }
        }
        public String PartnerNameGate2
        {
            get { return _partnerNameGate2; }
            set
            {
                _partnerNameGate2 = value;
                NotifyPropertyChanged();
            }
        }
        public String WeightGate1
        {
            get { return _weightGate1; }
            set
            {
                _weightGate1 = value;
                NotifyPropertyChanged();
            }
        }

        public String WeightGate2
        {
            get { return _weightGate2; }
            set
            {
                _weightGate2 = value;
                NotifyPropertyChanged();
            }
        }

        public String PartnerTypeNameGate1
        {
            get { return _partnerTypeNameGate1; }
            set
            {
                _partnerTypeNameGate1 = value;
                NotifyPropertyChanged();
            }
        }
        public String PartnerTypeNameGate2
        {
            get { return _partnerTypeNameGate2; }
            set
            {
                _partnerTypeNameGate2 = value;
                NotifyPropertyChanged();
            }
        }

        public String Gate1ButtonVisibility
        {
            get { return _gate1ButtonVisibility; }
            set
            {
                _gate1ButtonVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public String Gate2ButtonVisibility
        {
            get { return _gate2ButtonVisibility; }
            set
            {
                _gate2ButtonVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public TransactionScale TransactionScaleGate1
        {
            get { return _transactionScaleGate1; }
            set
            {
                _transactionScaleGate1 = value;
                NotifyPropertyChanged();
            }
        }

        public Partner PartnerGate1
        {
            get { return _partnerGate1; }
            set
            {
                _partnerGate1 = value;
                NotifyPropertyChanged();
            }
        }

        public Schedule ScheduleGate1
        {
            get { return _scheduleGate1; }
            set
            {
                _scheduleGate1 = value;
                NotifyPropertyChanged();
            }
        }

        public TransactionScale TransactionScaleGate2
        {
            get { return _transactionScaleGate2; }
            set
            {
                _transactionScaleGate2 = value;
                NotifyPropertyChanged();
            }
        }

        public Partner PartnerGate2
        {
            get { return _partnerGate2; }
            set
            {
                _partnerGate2 = value;
                NotifyPropertyChanged();
            }
        }
        public Schedule ScheduleGate2
        {
            get { return _scheduleGate2; }
            set
            {
                _scheduleGate2 = value;
                NotifyPropertyChanged();
            }
        }

        public String MessageGate1
        {
            get { return _messageGate1; }
            set
            {
                _messageGate1 = value;
                NotifyPropertyChanged();
            }
        }

        public String MessageGate2
        {
            get { return _messageGate2; }
            set
            {
                _messageGate2 = value;
                NotifyPropertyChanged();
            }
        }

    }
}
