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
        private int _nondeliveries;
        public SystemSettingViewModel()
        {
            _systemConfigDataTransfer = new SystemConfigDataTransfer();
            StorageCapacity = _systemConfigDataTransfer.GetStorageCappacity();
            Nondeliveries = int.Parse(_systemConfigDataTransfer.GetMaximumCanceledSchechule().AttributeValue);
        }

        public void Reload()
        {
            StorageCapacity = _systemConfigDataTransfer.GetStorageCappacity();
            Nondeliveries = int.Parse(_systemConfigDataTransfer.GetMaximumCanceledSchechule().AttributeValue);
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

        public int Nondeliveries
        {
            get { return _nondeliveries; }
            set
            {
                _nondeliveries = value;
                NotifyPropertyChanged();
            }
        }
    }
}
