using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Events;
using ImportExportDesktopApp.Utils;
using Prism.Events;
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
    class CreateTransactionViewModel : BaseNotifyPropertyChanged
    {
        private IEventAggregator _eventAggregator;

        private Transaction _transaction;
        private ObservableCollection<Partner> _partners;
        private String _weight;

        private PartnerDataTransfer _partnerDataTransfer;
        private TransactionDataTransfer _transactionDataTransfer;
        private SystemConfigDataTransfer _systemCongifDataTransfer;
        private GoodDataTransfer _goodDataTransfer;
        private InventoryDataTransfer _inventoryDataTransfer;
        private InventoryDetailDataTransfer _inventoryDetailDataTransfer;
        private TimeTemplateItemDataTransfer _timeTemplateItemDataTransfer;

        public ICommand InsertTransactionCommnad { get; set; }

        public CreateTransactionViewModel()
        {
            Transaction = new Transaction();
            Transaction.CreatedDate = DateTime.Now;
            Transaction.TimeIn = DateTime.Now;
            Transaction.TimeOut = DateTime.Now;

            _partnerDataTransfer = new PartnerDataTransfer();
            _transactionDataTransfer = new TransactionDataTransfer();
            _systemCongifDataTransfer = new SystemConfigDataTransfer();
            _goodDataTransfer = new GoodDataTransfer();
            _inventoryDataTransfer = new InventoryDataTransfer();
            _inventoryDetailDataTransfer = new InventoryDetailDataTransfer();
            _timeTemplateItemDataTransfer = new TimeTemplateItemDataTransfer();

            Partners = _partnerDataTransfer.GetAll();

            InsertTransactionCommnad = new RelayCommand<Window>(p => { return true; }, p =>
            {
                InsertTransaction(p);
            });

            _eventAggregator = AppService.Instance.EventAggregator;
        }

        private void InsertTransaction(Window window)
        {
            int storageCapacity = _systemCongifDataTransfer.GetStorageCappacity();

            Transaction.WeightOut = float.Parse(Weight);
            Transaction.GoodsId = 1;
            Transaction.TransactionStatus = 1;
            int partnerTypeId = GetPartnerTypeId();
            Transaction.TransactionType = partnerTypeId == 1 ? 1 : 0;
            if (Transaction.TransactionType == 0)
            {
                Transaction.WeightIn = float.Parse(Weight);
                Transaction.WeightOut = 0;
            }
            _transactionDataTransfer.InsertTransaction(Transaction);

            float goodInventory = _goodDataTransfer.getInventory();
            Inventory inventory = _inventoryDataTransfer.CheckExist();
            if (inventory == null)
            {
                inventory = _inventoryDataTransfer.CreateInventoryToday(goodInventory);
            }
            float totalWeight = getTotalWeight(partnerTypeId, Transaction.WeightIn, Transaction.WeightOut);
            _inventoryDetailDataTransfer.InsertInventoryDetailNotSaveChanges(1, Transaction.PartnerId, partnerTypeId, totalWeight, inventory.InventoryId);
            _timeTemplateItemDataTransfer.updateTimetemplateItemWeight(Transaction.TimeOut, totalWeight, partnerTypeId);
            _transactionDataTransfer.Save();
            Good good = _goodDataTransfer.UpdateInventory(partnerTypeId, totalWeight, storageCapacity);
            _eventAggregator.GetEvent<UpdateInventoryEvent>().Publish(good.QuantityOfInventory + "");
            MessageBox.Show("Success!!");

            if (window != null)
            {
                window.Close();
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

        private int GetPartnerTypeId()
        {
            foreach (var item in Partners)
            {
                if (item.PartnerId == Transaction.PartnerId)
                {
                    return item.PartnerTypeId;
                }
            }

            return 0;
        }

        public Transaction Transaction
        {
            get { return _transaction; }
            set
            {
                _transaction = value;
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

        public String Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                NotifyPropertyChanged();
            }
        }

    }
}
