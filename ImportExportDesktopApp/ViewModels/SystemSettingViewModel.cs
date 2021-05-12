using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.ViewModels
{
    class SystemSettingViewModel : BaseNotifyPropertyChanged
    {
        private int _storageCapacity;
        private SystemConfigDataTransfer _systemConfigDataTransfer;
        public SystemSettingViewModel()
        {
            _systemConfigDataTransfer = new SystemConfigDataTransfer();
            StorageCapacity = _systemConfigDataTransfer.GetStorageCappacity();
        }

        public void Reload()
        {
            StorageCapacity = _systemConfigDataTransfer.GetStorageCappacity();
        }
        public int StorageCapacity
        {
            get { return _storageCapacity; }
            set
            {
                _storageCapacity = value;
                NotifyPropertyChanged();
            }
        }
    }
}
