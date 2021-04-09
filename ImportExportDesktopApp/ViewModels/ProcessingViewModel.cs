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
        private InventoryDataTransfer _inventoryDataTransfer;
        private InventoryDetailDataTransfer _inventoryDetailDataTransfer;
        private ScheduleDataTransfer _scheduleDataTransfer;
        private ObservableCollection<Transaction> _processingTransaction;
        private ObservableCollection<Transaction> _successTransaction;
        private int _storageCapacity = 0;

        public ProcessingViewModel()
        {
            _transactionDataTransfer = new TransactionDataTransfer();
            _cardDataTransfer = new CardDataTransfer();
            _systemCongifDataTransfer = new SystemConfigDataTransfer();
            _goodDataTransfer = new GoodDataTransfer();
            _inventoryDataTransfer = new InventoryDataTransfer();
            _inventoryDetailDataTransfer = new InventoryDetailDataTransfer();
            _scheduleDataTransfer = new ScheduleDataTransfer();
            _storageCapacity = _systemCongifDataTransfer.GetStorageCappacity();
            ProcessingTransaction = _transactionDataTransfer.GetProcessingTransaction();
            SuccessTransaction = _transactionDataTransfer.GetSuccessTransaction();

        }

        public bool CheckCard(TransactionScale transactionScale)
        {
            Partner partner = _cardDataTransfer.CheckCard(transactionScale);
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
            _transactionDataTransfer.Save();
            return transaction;
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
    }
}
