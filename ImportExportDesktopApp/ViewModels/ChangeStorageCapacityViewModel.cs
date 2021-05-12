using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class ChangeStorageCapacityViewModel : BaseNotifyPropertyChanged
    {
        private int _capacity;
        private SystemConfigDataTransfer _systemConfigDataTransfer;
        private SystemConfig _systemConfig;
        public ICommand UpdateCommand { get; set; }
        public ChangeStorageCapacityViewModel()
        {
            _systemConfigDataTransfer = new SystemConfigDataTransfer();
            UpdateCommand = new RelayCommand<Window>((p) => { return true; }, Update);
            _systemConfig = _systemConfigDataTransfer.GetStorageCappacityEntity();
            Capacity = int.Parse(_systemConfig.AttributeValue);
        }

        private void Update(Window window)
        {
            _systemConfig.AttributeValue = _capacity + "";
            try
            {
                _systemConfigDataTransfer.Update(_systemConfig);
                _systemConfigDataTransfer.Save();
                MessageBox.Show("Success!");
                window.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                _capacity = value;
                NotifyPropertyChanged();
            }
        }
    }
}
