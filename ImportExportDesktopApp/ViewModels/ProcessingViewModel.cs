﻿using ImportExportDesktopApp.Commands;
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
    //
    //
    // Update transction chua update inventory
    //
    //
    //
    class ProcessingViewModel : BaseNotifyPropertyChanged
    {
        private String YELLOW_BG = "#FFC107";
        private String GREEN_BG = "#4CAF50";

        private IEventAggregator _eventAggregator;

        //search
        private ObservableCollection<Partner> _partners;
        private Partner _selectedPartner;
        private String _cancelSearchVisibility;

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
        public ICommand CancelGate2Command { get; set; }
        public ICommand CreateTransactionGate1Command { get; set; }
        public ICommand CancelGate1Command { get; set; }
        public ICommand SearchByPartnerCommand { get; set; }
        public ICommand CloseSearchCommand { get; set; }
        public ICommand DisableGate1Command { get; set; }
        public ICommand DisableGate2Command { get; set; }


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
        private String _btnHanldeContentGate1;
        private String _btnHanldeContentGate2;
        private String _gate1bg;
        private String _gate2bg;
        private EScaleExceptionType _exceptionTypeGate1;
        private EScaleExceptionType _exceptionTypeGate2;
        private String _disableButtonVisibilityGate1;
        private String _disableButtonVisibilityGate2;


        public ProcessingViewModel(IEventAggregator ea)
        {
            _cancelSearchVisibility = EVisibility.Hidden.ToString();

            Gate1Bg = GREEN_BG;
            Gate2Bg = GREEN_BG;

            BtnHanldeContentGate1 = EBtnHandleContent.Accept.ToString();
            BtnHanldeContentGate2 = EBtnHandleContent.Accept.ToString();

            DisableButtonVisibilityGate1 = EVisibility.Hidden.ToString();
            DisableButtonVisibilityGate2 = EVisibility.Hidden.ToString();

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
            _partnerDataTransfer = new PartnerDataTransfer();
            _notifyService = new NotifyService();

            Task.Run(new Action(() =>
            {
                GetAllPartners();
                _storageCapacity = _systemCongifDataTransfer.GetStorageCappacity();
                ProcessingTransaction = _transactionDataTransfer.GetProcessingTransaction();
                SuccessTransaction = _transactionDataTransfer.GetSuccessTransaction();
            }));

            //Command 
            CreateTransactionGate2Command = new RelayCommand<object>(p => { return true; }, p =>
            {
                CreateTransactionGate2();
            });
            CancelGate2Command = new RelayCommand<object>(p => { return true; }, p =>
            {
                CancelTransaction(EGate.Gate2, new ScaleExeptionAction(EGate.Gate2, EScaleExceptionAction.Cancel));
            });
            CreateTransactionGate1Command = new RelayCommand<object>(p => { return true; }, p =>
            {
                CreateTransactionGate1();
            });
            CancelGate1Command = new RelayCommand<object>(p => { return true; }, p =>
            {
                CancelTransaction(EGate.Gate1, new ScaleExeptionAction(EGate.Gate1, EScaleExceptionAction.Cancel));
            });
            SearchByPartnerCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                SearchByPartner();
            });
            CloseSearchCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                CloseSearch();
            });
            DisableGate1Command = new RelayCommand<object>(p => { return true; }, p =>
            {
                Disable(EGate.Gate1);
            });
            DisableGate2Command = new RelayCommand<object>(p => { return true; }, p =>
            {
                Disable(EGate.Gate2);
            });

            //Set event
            _eventAggregator = ea;
            _eventAggregator.GetEvent<ReslovedScaleExceptionEvent>().Subscribe(ResetAcceptContent);
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
                    CreateTransaction(transactionScale, partner, schedule, false);
                    return true;
                }
                else
                {
                    if (transaction.Gate.Contains(transactionScale.Gate.ToString()))
                    {
                        return false;
                    }

                    Transaction newTransaction = UpdateTransaction(transaction, transactionScale, partner, schedule, true);
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
                        AddException(transactionScale, partner, schedule, EScaleExceptionType.WrongProcess);
                        return false;
                    }
                    CreateTransaction(transactionScale, partner, schedule, false);
                    Task.Run(new Action(() =>
                    {
                        if (transactionScale.Device == EDeviceType.Android)
                        {
                            _notifyService.NotifyAndroid();
                        }
                    }));
                    return true;
                }
                else
                {
                    if (transaction.Gate.Contains(transactionScale.Gate.ToString()))
                    {
                        AddException(transactionScale, partner, schedule, EScaleExceptionType.Duplicate);
                        return false;
                    }

                    Transaction newTransaction = UpdateTransaction(transaction, transactionScale, partner, schedule, true);
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

                    Task.Run(new Action(() =>
                    {
                        if (transactionScale.Device == EDeviceType.Android)
                        {
                            _notifyService.NotifyAndroid();
                        }
                        _notifyService.NotifyWeb();
                    }));
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

        public void AddException(TransactionScale transactionScale, Partner partner, Schedule schedule, EScaleExceptionType exceptionType)
        {
            if (transactionScale.Gate == EGate.Gate2)
            {
                Gate2ButtonVisibility = EVisibility.Visible.ToString();
                TransactionScaleGate2 = transactionScale;
                PartnerGate2 = partner;
                ScheduleGate2 = schedule;
                _isSlovingExeptionGate2 = true;
                _exceptionTypeGate2 = exceptionType;
                MessageGate2 =
                                exceptionType == EScaleExceptionType.WrongProcess ? "Wrong process!! This partner has not passed through gate 1."
                              : exceptionType == EScaleExceptionType.Duplicate ? "Partner is requesting to cancel the old transaction and create a new one !!!"
                              : exceptionType == EScaleExceptionType.WrongTransactionType ? "Wrong partner type"
                              : exceptionType == EScaleExceptionType.OverWeightImport ? "Storage capacity is full!!"
                              : exceptionType == EScaleExceptionType.OverWeightExport ? "Storage is out of stock!!!"
                              : "Normal";
                if (exceptionType == EScaleExceptionType.WrongTransactionType)
                {
                    BtnHanldeContentGate2 = EBtnHandleContent.Edit.ToString();
                    DisableButtonVisibilityGate2 = EVisibility.Visible.ToString();
                }
                ScaleExeption scaleExeption = new ScaleExeption(transactionScale, partner, schedule, exceptionType, MessageGate2);
                _eventAggregator.GetEvent<ScaleExceptionEvent>().Publish(scaleExeption);
                Gate2Bg = YELLOW_BG;
            }
            else if (transactionScale.Gate == EGate.Gate1)
            {
                Gate1ButtonVisibility = EVisibility.Visible.ToString();
                TransactionScaleGate1 = transactionScale;
                PartnerGate1 = partner;
                ScheduleGate1 = schedule;
                _isSlovingExeptionGate1 = true;
                _exceptionTypeGate1 = exceptionType;
                MessageGate1 =
                                exceptionType == EScaleExceptionType.Duplicate ? "Partner is requesting to cancel the old transaction and create a new one !!!"
                              : exceptionType == EScaleExceptionType.WrongTransactionType ? "Wrong partner type"
                              : exceptionType == EScaleExceptionType.OverWeightImport ? "Storage capacity is full!!"
                              : exceptionType == EScaleExceptionType.OverWeightExport ? "Storage is out of stock!!!"
                              : "Normal";
                if (exceptionType == EScaleExceptionType.WrongTransactionType)
                {
                    BtnHanldeContentGate1 = EBtnHandleContent.Edit.ToString();
                    DisableButtonVisibilityGate1 = EVisibility.Visible.ToString();
                }
                ScaleExeption scaleExeption = new ScaleExeption(transactionScale, partner, schedule, exceptionType, MessageGate1);
                _eventAggregator.GetEvent<ScaleExceptionEvent>().Publish(scaleExeption);
                Gate1Bg = YELLOW_BG;
            }
        }

        public void CreateTransactionGate1()
        {
            if (_exceptionTypeGate1 == EScaleExceptionType.WrongTransactionType)
            {
                Transaction transaction = _transactionDataTransfer.IsProcessing(TransactionScaleGate1.Indentify);
                EditExceptionTransactionWindow editExceptionTransactionWindow = new EditExceptionTransactionWindow(transaction, TransactionScaleGate1);
                var point = Mouse.GetPosition(Application.Current.MainWindow);
                //Application curApp = Application.Current;
                //Window mainWindow = curApp.MainWindow;
                editExceptionTransactionWindow.Left = point.X;
                editExceptionTransactionWindow.Top = point.Y - 150;
                editExceptionTransactionWindow.ShowDialog();
                bool isCancel = editExceptionTransactionWindow.IsCancel;
                if (isCancel)
                {
                    return;
                }
                ChangePartner(transaction, TransactionScaleGate1, editExceptionTransactionWindow.SelectedPartner, ScheduleGate1);
                PartnerNameGate1 = "Partner:" + transaction.Partner.DisplayName;
                PartnerTypeNameGate1 = "Type: " + transaction.Partner.PartnerType.PartnerTypeName;
            }
            else
            {
                if (_exceptionTypeGate1 == EScaleExceptionType.Duplicate)
                {
                    _transactionDataTransfer.DisableAll(TransactionScaleGate1.Indentify);
                }
                if (_exceptionTypeGate1 == EScaleExceptionType.OverWeightImport || _exceptionTypeGate1 == EScaleExceptionType.OverWeightExport)
                {
                    Transaction transaction = _transactionDataTransfer.IsProcessing(TransactionScaleGate1.Indentify);
                    UpdateTransaction(transaction, TransactionScaleGate1, PartnerGate1, ScheduleGate1, false);
                }
                else
                {
                    CreateTransaction(TransactionScaleGate1, PartnerGate1, ScheduleGate1, false);
                }
            }
            Task.Run(new Action(() =>
            {
                if (TransactionScaleGate1.Device == EDeviceType.Android)
                {
                    _notifyService.NotifyAndroid();
                }
            }));
            ResetAcceptContent(new ScaleExeptionAction(EGate.Gate1, EScaleExceptionAction.Accept));
        }

        public void CreateTransactionGate2()
        {
            if (_exceptionTypeGate2 == EScaleExceptionType.WrongTransactionType)
            {
                Transaction transaction = _transactionDataTransfer.IsProcessing(TransactionScaleGate2.Indentify);
                EditExceptionTransactionWindow editExceptionTransactionWindow = new EditExceptionTransactionWindow(transaction, TransactionScaleGate2);
                var point = Mouse.GetPosition(Application.Current.MainWindow);
                //Application curApp = Application.Current;
                //Window mainWindow = curApp.MainWindow;
                editExceptionTransactionWindow.Left = point.X;
                editExceptionTransactionWindow.Top = point.Y - 150;
                editExceptionTransactionWindow.ShowDialog();
                bool isCancel = editExceptionTransactionWindow.IsCancel;
                if (isCancel)
                {
                    return;
                }
                ChangePartner(transaction, TransactionScaleGate2, editExceptionTransactionWindow.SelectedPartner, ScheduleGate2);
                PartnerNameGate2 = "Partner:" + transaction.Partner.DisplayName;
                PartnerTypeNameGate2 = "Type: " + transaction.Partner.PartnerType.PartnerTypeName;
            }
            else
            {
                if (_exceptionTypeGate2 == EScaleExceptionType.Duplicate)
                {
                    _transactionDataTransfer.DisableAll(TransactionScaleGate2.Indentify);
                }

                if (_exceptionTypeGate2 == EScaleExceptionType.OverWeightImport || _exceptionTypeGate2 == EScaleExceptionType.OverWeightExport)
                {
                    Transaction transaction = _transactionDataTransfer.IsProcessing(TransactionScaleGate2.Indentify);
                    UpdateTransaction(transaction, TransactionScaleGate2, PartnerGate2, ScheduleGate2, false);
                }
                else
                {
                    CreateTransaction(TransactionScaleGate2, PartnerGate2, ScheduleGate2, false);
                }
            }
            Task.Run(new Action(() =>
            {
                if (TransactionScaleGate2.Device == EDeviceType.Android)
                {
                    _notifyService.NotifyAndroid();
                }
            }));
            ResetAcceptContent(new ScaleExeptionAction(EGate.Gate2, EScaleExceptionAction.Accept));
        }

        public void ResetAcceptContent(ScaleExeptionAction exeptionAction)
        {
            if (exeptionAction.Gate == EGate.Gate2)
            {
                if (exeptionAction.Action == EScaleExceptionAction.Accept)
                {
                    Gate2ButtonVisibility = EVisibility.Hidden.ToString();
                    _isSlovingExeptionGate2 = false;
                }
                else if (exeptionAction.Action == EScaleExceptionAction.Cancel)
                {
                    Gate2ButtonVisibility = EVisibility.Hidden.ToString();
                    _isSlovingExeptionGate2 = false;
                }
                Gate2Bg = GREEN_BG;
                MessageGate2 = "";
                BtnHanldeContentGate2 = EBtnHandleContent.Accept.ToString();
                DisableButtonVisibilityGate2 = EVisibility.Hidden.ToString();
            }
            else if (exeptionAction.Gate == EGate.Gate1)
            {
                if (exeptionAction.Action == EScaleExceptionAction.Accept)
                {
                    Gate1ButtonVisibility = EVisibility.Hidden.ToString();
                    _isSlovingExeptionGate1 = false;
                }
                else if (exeptionAction.Action == EScaleExceptionAction.Cancel)
                {
                    Gate1ButtonVisibility = EVisibility.Hidden.ToString();
                    _isSlovingExeptionGate1 = false;
                }
                Gate1Bg = GREEN_BG;
                MessageGate1 = "";
                BtnHanldeContentGate1 = EBtnHandleContent.Accept.ToString();
                DisableButtonVisibilityGate1 = EVisibility.Hidden.ToString();
            }
            UpdateTable();
        }

        public void CancelTransaction(EGate gate, ScaleExeptionAction exceptionAction)
        {
            if (gate == EGate.Gate1)
            {
                CreateTransaction(TransactionScaleGate1, PartnerGate1, ScheduleGate1, true);
            }
            else if (gate == EGate.Gate2)
            {
                if (gate == EGate.Gate1)
                {
                    CreateTransaction(TransactionScaleGate2, PartnerGate2, ScheduleGate2, true);
                }
            }

            ResetAcceptContent(exceptionAction);

        }

        public void CreateTransaction(TransactionScale transactionScale, Partner partner, Schedule schedule, bool isCancel)
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
            transaction.TransactionStatus = isCancel ? 2 : 0;
            transaction.TransactionType = partner.PartnerTypeId == 1 ? 1 : 0;
            transaction.Gate = transactionScale.Gate.ToString();
            _transactionDataTransfer.InsertTransaction(transaction);
            if (transactionScale.Device == EDeviceType.Android && isCancel == false)
            {
                _notifyService.NotifyAndroid();
            }
        }

        public bool ChangePartner(Transaction transaction, TransactionScale transactionScale, Partner partner, Schedule schedule)
        {
            transaction.PartnerId = partner.PartnerId;
            transaction.TransactionType = transaction.TransactionType = partner.PartnerTypeId == 1 ? 1 : 0;
            transaction = UpdateTransaction(transaction, transactionScale, partner, schedule, true);
            if (transaction == null)
            {
                return false;
            }
            else
            {
                UpdateGood(partner, transaction);
            }
            Task.Run(new Action(() =>
            {
                if (transactionScale.Device == EDeviceType.Android)
                {
                    _notifyService.NotifyAndroid();
                    _notifyService.NotifyWeb();
                }
            }));
            return true;
        }

        public Transaction UpdateTransaction(Transaction transaction, TransactionScale transactionScale, Partner partner, Schedule schedule, bool isCheckWeight)
        {
            float goodInventory = _goodDataTransfer.getInventory();
            if (isCheckWeight == true && !CheckWeight(transaction, partner, schedule, transactionScale, goodInventory))
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
                    PartnerTypeNameGate2 = "Type: " + partner.PartnerType.PartnerTypeName;
                }
                else
                {
                    PartnerNameGate2 = "No partner found!!";
                }
                WeightGate2 = transactionScale.Weight + " kg";
            }
        }

        public void Disable(EGate gate)
        {
            if (gate == EGate.Gate1)
            {
                _transactionDataTransfer.DisableAll(TransactionScaleGate1.Indentify);
            }
            else if (gate == EGate.Gate2)
            {
                _transactionDataTransfer.DisableAll(TransactionScaleGate2.Indentify);
            }
            _transactionDataTransfer.Save();
            ResetAcceptContent(new ScaleExeptionAction(EGate.Gate2, EScaleExceptionAction.Accept));
        }

        public void CloseSearch()
        {
            CancelSearchVisibility = EVisibility.Hidden.ToString();
            SelectedPartner = null;
            ProcessingTransaction = _transactionDataTransfer.GetProcessingTransaction();
            SuccessTransaction = _transactionDataTransfer.GetSuccessTransaction();
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
        public bool CheckWeight(Transaction transaction, Partner partner, Schedule schedule, TransactionScale transactionScale, float goodInventory)
        {
            // Export
            if (partner.PartnerTypeId == 1)
            {
                if (transaction.WeightIn > transactionScale.Weight)
                {
                    AddException(transactionScale, partner, schedule, EScaleExceptionType.WrongTransactionType);
                    return false;
                }
                else if ((transactionScale.Weight - transaction.WeightIn) > goodInventory)
                {
                    AddException(transactionScale, partner, schedule, EScaleExceptionType.OverWeightExport);
                    return false;
                }
            }

            //Import
            else if (partner.PartnerId == 2)
            {
                if (transaction.WeightIn < transactionScale.Weight)
                {
                    AddException(transactionScale, partner, schedule, EScaleExceptionType.WrongTransactionType);
                    return false;
                }
                else if ((transaction.WeightIn - transactionScale.Weight) > (_storageCapacity - goodInventory))
                {
                    AddException(transactionScale, partner, schedule, EScaleExceptionType.OverWeightImport);
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

        public void GetAllPartners()
        {
            Partners = _partnerDataTransfer.GetAll();
        }

        public void SearchByPartner()
        {
            CancelSearchVisibility = EVisibility.Visible.ToString();
            ProcessingTransaction = _transactionDataTransfer.GetProcessingTransactionByPartnerToday(SelectedPartner.PartnerId);
            SuccessTransaction = _transactionDataTransfer.GetSuccessTransactionByPartnerToday(SelectedPartner.PartnerId);
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
            if (CancelSearchVisibility == EVisibility.Hidden.ToString())
            {
                ProcessingTransaction = _transactionDataTransfer.GetProcessingTransaction();
                SuccessTransaction = _transactionDataTransfer.GetSuccessTransaction();
            }
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

        public String Gate1Bg
        {
            get { return _gate1bg; }
            set
            {
                _gate1bg = value;
                NotifyPropertyChanged();
            }
        }
        public String Gate2Bg
        {
            get { return _gate2bg; }
            set
            {
                _gate2bg = value;
                NotifyPropertyChanged();
            }
        }
        public String BtnHanldeContentGate1
        {
            get { return _btnHanldeContentGate1; }
            set
            {
                _btnHanldeContentGate1 = value;
                NotifyPropertyChanged();
            }
        }
        public String BtnHanldeContentGate2
        {
            get { return _btnHanldeContentGate2; }
            set
            {
                _btnHanldeContentGate2 = value;
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

        public Partner SelectedPartner
        {
            get { return _selectedPartner; }
            set
            {
                _selectedPartner = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSlovingExeptionGate1
        {
            get { return _isSlovingExeptionGate1; }
            set
            {
                _isSlovingExeptionGate1 = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsSlovingExeptionGate2
        {
            get { return _isSlovingExeptionGate2; }
            set
            {
                _isSlovingExeptionGate2 = value;
                NotifyPropertyChanged();
            }
        }

        public String CancelSearchVisibility
        {
            get { return _cancelSearchVisibility; }
            set
            {
                _cancelSearchVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public String DisableButtonVisibilityGate1
        {
            get { return _disableButtonVisibilityGate1; }
            set
            {
                _disableButtonVisibilityGate1 = value;
                NotifyPropertyChanged();
            }
        }

        public String DisableButtonVisibilityGate2
        {
            get { return _disableButtonVisibilityGate2; }
            set
            {
                _disableButtonVisibilityGate2 = value;
                NotifyPropertyChanged();
            }
        }
    }
}
