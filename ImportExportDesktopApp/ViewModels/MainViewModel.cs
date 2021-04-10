using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Events;
using ImportExportDesktopApp.Utils;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

/**
* @author Loi Nguyen
*
* @date - 4/7/2021 5:53:03 PM 
*/

namespace ImportExportDesktopApp.ViewModels
{
    class MainViewModel : BaseNotifyPropertyChanged
    {
        public KeyValuePair<String, String> SelectedItem { get; set; }
        public Dictionary<String, String> PageNameList { get; set; }
        private SystemConfigDataTransfer _systemCongifDataTransfer;
        private GoodDataTransfer _goodDataTransfer;
        private Page _page;
        private String _inventory;
        private String _available;
        private float _capacity;
        private String _inventoryWarn;
        private String _availableWarn;

        public MainViewModel(IEventAggregator ea)
        {
            PageNameList = new Dictionary<string, string>();
            PageNameList.Add("Processing", "ChartDonut");
            PageNameList.Add("Partner", "AccountMultiple");
            PageNameList.Add("Schedules", "schedule");
            PageNameList.Add("Inventory", "PackageVariantClosed");
            PageNameList.Add("Goods", "Gift");
            SelectedItem = PageNameList.First();
            _systemCongifDataTransfer = new SystemConfigDataTransfer();
            _goodDataTransfer = new GoodDataTransfer();
            _capacity = _systemCongifDataTransfer.GetStorageCappacity();
            GetInventory();
            ea.GetEvent<UpdateInventoryEvent>().Subscribe(HandleEvent);
        }
        private void HandleEvent(String value)
        {
            GetInventory();
        }
        public void GetInventory()
        {
            float inventory = _goodDataTransfer.getInventory();
            if (inventory < (_capacity * 0.2))
            {
                InventoryWarn = "Hidden";
            }
            else
            {
                InventoryWarn = "Visible";
            }
            float available = _capacity - inventory;
            if (available < (_capacity * 0.2))
            {
                AvailableWarn = "Hidden";
            }
            else
            {
                AvailableWarn = "Visible";
            }
            Inventory = Math.Round(inventory, 1) + " kg";
            Available = Math.Round(available, 1) + " kg";
        }

        public Page Page
        {
            get { return _page; }
            set
            {
                _page = value;
                NotifyPropertyChanged();
            }
        }

        public String Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                NotifyPropertyChanged();
            }
        }

        public String Available
        {
            get { return _available; }
            set
            {
                _available = value;
                NotifyPropertyChanged();
            }
        }

        public String AvailableWarn
        {
            get { return _availableWarn; }
            set
            {
                _availableWarn = value;
                NotifyPropertyChanged();
            }
        }

        public String InventoryWarn
        {
            get { return _inventoryWarn; }
            set
            {
                _inventoryWarn = value;
                NotifyPropertyChanged();
            }
        }
    }
}
