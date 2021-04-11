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
        private TransactionDataTransfer _transactionDataTransfer;

        //Gate Exception handle
        private TransactionScale _transactionScaleGate;
        private Partner _partnerGate;
        private Schedule _scheduleGate;
        private EScaleExceptionType _exceptionType;

        private String _messageGate;

        private IEventAggregator _eventAggregator;

        private NotificationManager _notificationManager;
        public ICommand CreateTransactionGateCommand { get; set; }
        public ICommand CancelGateCommand { get; set; }
        public ScaleExceptionViewModel()
        {
            _notificationManager = new NotificationManager();
            _eventAggregator = AppService.Instance.EventAggregator;
            _transactionDataTransfer = new TransactionDataTransfer();
            CreateTransactionGateCommand = new RelayCommand<Window>(p => { return true; }, p =>
            {
                Accept(p);
            });

            CancelGateCommand = new RelayCommand<Window>(p => { return true; }, p =>
            {
                Cancel(p);
            });
        }

        public void Init(ScaleExeption scaleExeption)
        {
            TransactionScaleGate = scaleExeption.TransactionScale;
            PartnerGate = scaleExeption.Partner;
            ScheduleGate = scaleExeption.Schedule;
            ExceptionType = scaleExeption.ExceptionType;
        }
        public void Accept(Window window)
        {
            if (CreateTransaction(TransactionScaleGate, PartnerGate, ScheduleGate, ExceptionType))
            {
                _notificationManager.Show(new NotificationContent
                {
                    Title = "Notification",
                    Message = "Transaction has been created for " + PartnerGate.DisplayName,
                    Type = NotificationType.Information
                });

                _eventAggregator.GetEvent<ReslovedScaleExceptionEvent>().Publish(new ScaleExeptionAction(EGate.Gate2, EScaleExceptionAction.Accept));
                if (window != null)
                {
                    window.Close();
                }
            }
        }

        public void Cancel(Window window)
        {
            _eventAggregator.GetEvent<ReslovedScaleExceptionEvent>().Publish(new ScaleExeptionAction(EGate.Gate2, EScaleExceptionAction.Accept));
            if (window != null)
            {
                window.Close();
            }
        }

        public bool CreateTransaction(TransactionScale transactionScale, Partner partner, Schedule schedule, EScaleExceptionType exceptionType)
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
            transaction.TransactionStatus = 0;
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
    }
}
