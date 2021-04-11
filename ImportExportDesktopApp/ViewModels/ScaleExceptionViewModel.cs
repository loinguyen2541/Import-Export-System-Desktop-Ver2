using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.ScaleModels;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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


        private String _messageGate;
        public ScaleExceptionViewModel()
        {

        }

        public void Init(ScaleExeption scaleExeption)
        {
            TransactionScaleGate = scaleExeption.TransactionScale;
            PartnerGate = scaleExeption.Partner;
            ScheduleGate = scaleExeption.Schedule;
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
    }
}
