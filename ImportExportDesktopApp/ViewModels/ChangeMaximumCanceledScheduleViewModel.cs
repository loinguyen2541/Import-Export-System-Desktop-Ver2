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
    class ChangeMaximumCanceledScheduleViewModel : BaseNotifyPropertyChanged
    {
        private int _slots;
        private SystemConfigDataTransfer _systemConfigDataTransfer;
        private SystemConfig _systemConfig;
        public ICommand UpdateCommand { get; set; }
        public ChangeMaximumCanceledScheduleViewModel()
        {
            _systemConfigDataTransfer = new SystemConfigDataTransfer();
            UpdateCommand = new RelayCommand<Window>((p) => { return true; }, Update);
            _systemConfig = _systemConfigDataTransfer.GetMaximumCanceledSchechule();
            Slots = int.Parse(_systemConfig.AttributeValue);
        }

        private void Update(Window window)
        {
            _systemConfig.AttributeValue = Slots + "";
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

        public int Slots
        {
            get { return _slots; }
            set
            {
                _slots = value;
                NotifyPropertyChanged();
            }
        }
    }
}
