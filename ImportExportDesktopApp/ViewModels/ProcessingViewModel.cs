using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Enums;
using ImportExportDesktopApp.ScaleModels;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/8/2021 1:24:03 PM 
*/

namespace ImportExportDesktopApp.ViewModels
{
    class ProcessingViewModel : BaseNotifyPropertyChanged
    {
        private TransactionDataTransfer _transactionDataTransfer;
        private CardDataTransfer _cardDataTransfer;
        private SystemConfigDataTransfer _systemCongifDataTransfer;
        private GoodDataTransfer _goodDataTransfer;
        private ObservableCollection<Transaction> _processingTransaction;
        private ObservableCollection<Transaction> _successTransaction;
        private int _storageCapacity = 0;

        public ProcessingViewModel()
        {
            _transactionDataTransfer = new TransactionDataTransfer();
            _cardDataTransfer = new CardDataTransfer();
            _systemCongifDataTransfer = new SystemConfigDataTransfer();
            _goodDataTransfer = new GoodDataTransfer();
            _storageCapacity = _systemCongifDataTransfer.GetStorageCappacity();

            ProcessingTransaction = _transactionDataTransfer.GetProcessingTransaction();
            SuccessTransaction = _transactionDataTransfer.GetSuccessTransaction();

        }

        public bool CheckCard(TransactionScale transactionScale)
        {
            Partner partner = _cardDataTransfer.CheckCard(transactionScale.Indentify);
            if (partner != null)
            {
                Transaction transaction = _transactionDataTransfer.IsProcessing(transactionScale.Indentify);
                if (transaction == null)
                {
                    CreateTransaction(transactionScale, partner);
                    return true;
                }
                else
                {
                    if (transaction.Gate.Contains(transactionScale.Gate.ToString()))
                    {
                        return false;
                    }

                    Transaction newTransaction = UpdateTransaction(transaction, transactionScale, partner);
                    if (newTransaction == null)
                    {
                        return false;
                    }

                    if (partner.PartnerTypeId == 1)
                    {
                        float weight = newTransaction.WeightOut - newTransaction.WeightIn;
                        _goodDataTransfer.UpdateInventory(partner.PartnerTypeId, weight, _storageCapacity);
                    }
                    else if (partner.PartnerTypeId == 2)
                    {
                        float weight = newTransaction.WeightIn - newTransaction.WeightOut;
                        _goodDataTransfer.UpdateInventory(partner.PartnerTypeId, weight, _storageCapacity);
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

        public void CreateTransaction(TransactionScale transactionScale, Partner partner)
        {
            Transaction transaction = new Transaction();
            transaction.PartnerId = partner.PartnerId;
            transaction.IsScheduled = false;
            transaction.CreatedDate = DateTime.Now;
            transaction.GoodsId = 1;
            transaction.TimeIn = DateTime.Now;
            transaction.WeightIn = transactionScale.Weight;
            transaction.WeightOut = 0;
            transaction.IdentificationCode = transactionScale.Indentify;
            transaction.TransactionStatus = "Success";
            transaction.TransactionType = partner.PartnerTypeId == 1 ? "Export" : "Import";
            transaction.Gate = transactionScale.Gate.ToString();
            _transactionDataTransfer.InsertTransaction(transaction);
        }

        public Transaction UpdateTransaction(Transaction transaction, TransactionScale transactionScale, Partner partner)
        {
            if (partner.PartnerTypeId == 1)
            {
                if (transaction.WeightIn > transactionScale.Weight)
                {
                    return null;
                }
                else if ((transaction.WeightOut - transaction.WeightIn) > _goodDataTransfer.getInventory())
                {
                    return null;
                }
            }
            else if (partner.PartnerTypeId == 2)
            {
                if (transaction.WeightIn < transactionScale.Weight)
                {
                    return null;
                }
                else if ((transaction.WeightIn - transaction.WeightOut) > (_storageCapacity - _goodDataTransfer.getInventory()))
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            transaction.WeightOut = transactionScale.Weight;
            transaction.TimeOut = DateTime.Now;
            transaction.TransactionStatus = "Success";
            transaction.Gate = transactionScale.Gate.ToString();
            transaction = _transactionDataTransfer.UpdateTransaction(transaction);
            return transaction;
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
    }
}
