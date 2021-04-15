using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Enums;
using ImportExportDesktopApp.Events;
using ImportExportDesktopApp.HttpServices;
using ImportExportDesktopApp.ScaleModels;
using ImportExportDesktopApp.Utils;
using ImportExportDesktopApp.Windows;
using Notifications.Wpf;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

/**
* @author Loi Nguyen
*
* @date - 4/11/2021 4:41:51 PM 
*/

namespace ImportExportDesktopApp.ViewModels
{

    class ScaleExceptionViewModel : BaseNotifyPropertyChanged
    {

        //Gate Exception handle
        private TransactionScale _transactionScaleGate;
        private Partner _partnerGate;
        private Schedule _scheduleGate;
        private EScaleExceptionType _exceptionType;

        private String _messageGate;

        private IEventAggregator _eventAggregator;

        private NotificationManager _notificationManager;
        private TransactionDataTransfer _transactionDataTransfer;
        private CardDataTransfer _cardDataTransfer;
        private SystemConfigDataTransfer _systemCongifDataTransfer;
        private GoodDataTransfer _goodDataTransfer;
        private InventoryDataTransfer _inventoryDataTransfer;
        private InventoryDetailDataTransfer _inventoryDetailDataTransfer;
        private ScheduleDataTransfer _scheduleDataTransfer;
        private TimeTemplateItemDataTransfer _timeTemplateItemDataTransfer;
        private PartnerDataTransfer _partnerDataTransfer;

        private NotifyService _notifyService;
        private NotificationApiService _notificationApiService;

        int _storageCapacity = 0;

        public ICommand CreateTransactionGateCommand { get; set; }
        public ICommand CancelGateCommand { get; set; }

        public ICommand DisableGateCommand { get; set; }

        private String _handleButtonContent;

        private String _disableButtonVisibilityGate;

        private String _gate;
        public ScaleExceptionViewModel()
        {
            _notificationManager = new NotificationManager();
            _eventAggregator = AppService.Instance.EventAggregator;

            _transactionDataTransfer = new TransactionDataTransfer();
            _cardDataTransfer = new CardDataTransfer();
            _systemCongifDataTransfer = new SystemConfigDataTransfer();
            _goodDataTransfer = new GoodDataTransfer();
            _inventoryDataTransfer = new InventoryDataTransfer();
            _inventoryDetailDataTransfer = new InventoryDetailDataTransfer();
            _scheduleDataTransfer = new ScheduleDataTransfer();
            _timeTemplateItemDataTransfer = new TimeTemplateItemDataTransfer();
            _partnerDataTransfer = new PartnerDataTransfer();
            _notifyService = new NotifyService();
            _notificationApiService = new NotificationApiService();

            CreateTransactionGateCommand = new RelayCommand<Window>(p => { return true; }, p =>
            {
                Accept(p);
            });

            CancelGateCommand = new RelayCommand<Window>(p => { return true; }, p =>
            {
                Cancel(p);
            });

            DisableGateCommand = new RelayCommand<Window>(p => { return true; }, p =>
            {
                Disable(p);
            });

            _storageCapacity = _systemCongifDataTransfer.GetStorageCappacity();
        }

        public void Init(ScaleExeption scaleExeption)
        {
            TransactionScaleGate = scaleExeption.TransactionScale;
            PartnerGate = scaleExeption.Partner;
            ScheduleGate = scaleExeption.Schedule;
            ExceptionType = scaleExeption.ExceptionType;
            MessageGate = scaleExeption.Message;
            if (ExceptionType == EScaleExceptionType.WrongTransactionType)
            {
                HandleButtonContent = EBtnHandleContent.Edit.ToString();
            }
            else
            {
                HandleButtonContent = EBtnHandleContent.Accept.ToString();
            }

            if (ExceptionType == EScaleExceptionType.WrongTransactionType)
            {
                DisableButtonVisibilityGate = EVisibility.Visible.ToString();
            }
            else
            {
                DisableButtonVisibilityGate = EVisibility.Hidden.ToString();
            }
        }
        public void Accept(Window window)
        {
            if (ExceptionType == EScaleExceptionType.WrongTransactionType)
            {
                Transaction transaction = _transactionDataTransfer.IsProcessing(TransactionScaleGate.Indentify);
                EditExceptionTransactionWindow editExceptionTransactionWindow = new EditExceptionTransactionWindow(transaction, TransactionScaleGate);
                var point = Mouse.GetPosition(Application.Current.MainWindow);
                //Application curApp = Application.Current;
                //Window mainWindow = curApp.MainWindow;
                editExceptionTransactionWindow.Left = point.X;
                editExceptionTransactionWindow.Top = point.Y - 150;
                editExceptionTransactionWindow.Topmost = true;
                editExceptionTransactionWindow.ShowDialog();
                bool isCancel = editExceptionTransactionWindow.IsCancel;
                if (isCancel)
                {
                    return;
                }
                ChangePartner(transaction, TransactionScaleGate, editExceptionTransactionWindow.SelectedPartner, ScheduleGate);
                _notificationManager.Show(new NotificationContent
                {
                    Title = "Notification",
                    Message = "Transaction has been updated for " + PartnerGate.DisplayName,
                    Type = NotificationType.Information
                });
            }
            else if (ExceptionType == EScaleExceptionType.Overload)
            {
                Transaction transaction = _transactionDataTransfer.IsProcessing(TransactionScaleGate.Indentify);
                if (transaction != null)
                {
                    if (transaction.Gate.Contains(TransactionScaleGate.Gate.ToString()))
                    {
                        ExceptionType = EScaleExceptionType.Duplicate;
                        MessageGate = "Partner is requesting to cancel the old transaction and create a new one !!!";
                        return;
                    }
                    transaction = UpdateTransaction(transaction, TransactionScaleGate, PartnerGate, ScheduleGate, true);
                    if (transaction == null)
                    {
                        return;
                    }
                    UpdateGood(PartnerGate, transaction);
                    Task.Run(new Action(() =>
                    {
                        NotifyHttpAll(transaction);
                    }));
                }
                else
                {
                    if (TransactionScaleGate.Gate == EGate.Gate2)
                    {
                        ExceptionType = EScaleExceptionType.WrongProcess;
                        MessageGate = "Wrong process!!This partner has not passed through gate 1!!";
                        return;
                    }
                    else
                    {
                        CreateTransaction(TransactionScaleGate, PartnerGate, ScheduleGate, false);
                    }
                }
            }
            else
            {
                if (ExceptionType == EScaleExceptionType.OverWeightImport || ExceptionType == EScaleExceptionType.OverWeightExport)
                {
                    Transaction transaction = _transactionDataTransfer.IsProcessing(TransactionScaleGate.Indentify);
                    transaction = UpdateTransaction(transaction, TransactionScaleGate, PartnerGate, ScheduleGate, false);
                    UpdateGood(PartnerGate, transaction);
                }
                else if (CreateTransaction(TransactionScaleGate, PartnerGate, ScheduleGate, false))
                {
                    _notificationManager.Show(new NotificationContent
                    {
                        Title = "Notification",
                        Message = "Transaction has been created for " + PartnerGate.DisplayName,
                        Type = NotificationType.Information
                    });


                }
            }
            _eventAggregator.GetEvent<ReslovedScaleExceptionEvent>().Publish(new ScaleExeptionAction(TransactionScaleGate.Gate, EScaleExceptionAction.Accept));
            if (window != null)
            {
                window.Close();
            }
        }

        public void Disable(Window window)
        {
            if (TransactionScaleGate.Gate == EGate.Gate1)
            {
                _transactionDataTransfer.DisableAll(TransactionScaleGate.Indentify);
            }
            else if (TransactionScaleGate.Gate == EGate.Gate2)
            {
                _transactionDataTransfer.DisableAll(TransactionScaleGate.Indentify);
            }
            _transactionDataTransfer.Save();
            _notificationManager.Show(new NotificationContent
            {
                Title = "Notification",
                Message = "Transaction has been disabled for " + PartnerGate.DisplayName,
                Type = NotificationType.Information
            });
            _eventAggregator.GetEvent<ReslovedScaleExceptionEvent>().Publish(new ScaleExeptionAction(TransactionScaleGate.Gate, EScaleExceptionAction.Accept));

            if (window != null)
            {
                window.Close();
            }
        }

        public bool ChangePartner(Transaction transaction, TransactionScale transactionScale, Partner partner, Schedule schedule)
        {
            transaction.PartnerId = partner.PartnerId;
            transaction.TransactionType = transaction.TransactionType = partner.PartnerTypeId == 1 ? 1 : 0;
            transaction = UpdateTransaction(transaction, transactionScale, partner, schedule, false);
            if (transaction == null)
            {
                return false;
            }
            else
            {
                UpdateGood(partner, transaction);
            }
            NotifyHttpAll(transaction);
            return true;
        }

        public Transaction UpdateTransaction(Transaction transaction, TransactionScale transactionScale, Partner partner, Schedule schedule, bool isCheckWeight)
        {
            float goodInventory = _goodDataTransfer.getInventory();
            if (isCheckWeight && !CheckWeight(partner.PartnerTypeId, transaction.WeightIn, transactionScale.Weight, goodInventory))
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
        public bool CheckWeight(int partnerTypeId, float weightIn, float weightOut, float goodInventory)
        {
            // Export
            if (partnerTypeId == 1)
            {
                if (weightIn > weightOut)
                {

                    ExceptionType = EScaleExceptionType.WrongTransactionType;
                    HandleButtonContent = EBtnHandleContent.Edit.ToString();
                    DisableButtonVisibilityGate = EVisibility.Visible.ToString();
                    MessageGate = "Wrong partner type";
                    return false;
                }
                else if ((weightOut - weightIn) > goodInventory)
                {

                    ExceptionType = EScaleExceptionType.OverWeightExport;
                    MessageGate = "Storage is out of stock!!!";
                    return false;
                }
            }

            //Import
            else if (partnerTypeId == 2)
            {
                if (weightIn < weightOut)
                {
                    ExceptionType = EScaleExceptionType.WrongTransactionType;
                    HandleButtonContent = EBtnHandleContent.Edit.ToString();
                    DisableButtonVisibilityGate = EVisibility.Visible.ToString();
                    MessageGate = "Wrong partner type";
                    return false;
                }
                else if ((weightIn - weightOut) > (_storageCapacity - goodInventory))
                {
                    ExceptionType = EScaleExceptionType.OverWeightImport;
                    MessageGate = "Storage capacity is full!!";
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
        void UpdateGood(Partner partner, Transaction newTransaction)
        {
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

        public void Cancel(Window window)
        {
            _eventAggregator.GetEvent<ReslovedScaleExceptionEvent>().Publish(new ScaleExeptionAction(TransactionScaleGate.Gate, EScaleExceptionAction.Cancel));
            if (ExceptionType != EScaleExceptionType.WrongTransactionType)
            {
                CreateTransaction(TransactionScaleGate, PartnerGate, ScheduleGate, true);
            }
            if (window != null)
            {
                window.Close();
            }
        }

        void NotifyHttpAll(Transaction transaction)
        {
            if (TransactionScaleGate.Device == EDeviceType.Android)
            {
                _notifyService.NotifyAndroid();
            }
            Notification notification = new Notification();
            notification.ContentForAdmin = "the partner " + PartnerGate.DisplayName + " has just completed a transaction!";
            notification.ContentForPartner = "You have just completed a transaction!";
            notification.CreatedDate = transaction.CreatedDate;
            notification.TransactionId = transaction.TransactionId;
            notification.PartnerId = PartnerGate.PartnerId;
            notification.NotificationType = PartnerGate.PartnerTypeId == 1 ? 1 : 0;
            bool noResult = _notificationApiService.PostNotification(notification);
            if (noResult)
            {
                _notifyService.NotifyWeb();
            }
        }

        public bool CreateTransaction(TransactionScale transactionScale, Partner partner, Schedule schedule, bool isCancel)
        {
            _transactionDataTransfer.DisableAll(transactionScale.Indentify);

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
            transaction.TransactionStatus = isCancel ? 2 : 0;
            transaction.TransactionType = partner.PartnerTypeId == 1 ? 1 : 0;
            transaction.Gate = transactionScale.Gate.ToString();
            try
            {
                _transactionDataTransfer.InsertTransaction(transaction);
            }
            catch
            {
                MessageBox.Show("Insert Error!!!");
                return false;
            }
            if (transactionScale.Device == EDeviceType.Android)
            {
                _notifyService.NotifyAndroid();
            }
            return true;
        }
        public TransactionScale TransactionScaleGate
        {
            get { return _transactionScaleGate; }
            set
            {
                _transactionScaleGate = value;
                NotifyPropertyChanged();
            }
        }

        public Partner PartnerGate
        {
            get { return _partnerGate; }
            set
            {
                _partnerGate = value;
                NotifyPropertyChanged();
            }
        }

        public Schedule ScheduleGate
        {
            get { return _scheduleGate; }
            set
            {
                _scheduleGate = value;
                NotifyPropertyChanged();
            }
        }

        public String MessageGate
        {
            get { return _messageGate; }
            set
            {
                _messageGate = value;
                NotifyPropertyChanged();
            }
        }

        public EScaleExceptionType ExceptionType
        {
            get { return _exceptionType; }
            set
            {
                _exceptionType = value;
                NotifyPropertyChanged();
            }
        }

        public String HandleButtonContent
        {
            get { return _handleButtonContent; }
            set
            {
                _handleButtonContent = value;
                NotifyPropertyChanged();
            }
        }

        public String DisableButtonVisibilityGate
        {
            get { return _disableButtonVisibilityGate; }
            set
            {
                _disableButtonVisibilityGate = value;
                NotifyPropertyChanged();
            }
        }

        public String Gate
        {
            get { return _gate; }
            set
            {
                _gate = value;
                NotifyPropertyChanged();
            }
        }
    }
}
