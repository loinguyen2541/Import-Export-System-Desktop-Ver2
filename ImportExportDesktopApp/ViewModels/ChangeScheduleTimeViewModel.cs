using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class ChangeScheduleTimeViewModel : BaseNotifyPropertyChanged
    {
        private SystemConfigDataTransfer _systemConfigDataTransfer;
        private SystemConfig _startWorkingTime;
        private SystemConfig _finishWorkingTime;
        private SystemConfig _startBreakTime;
        private SystemConfig _finishBreakTime;
        private SystemConfig _timeBetweenSlot;

        private SystemConfig _oldStartWorkingTime;
        private SystemConfig _oldFinishWorkingTime;
        private SystemConfig _oldStartBreakTime;
        private SystemConfig _oldFinishBreakTime;
        private SystemConfig _oldTimeBetweenSlot;
        public ICommand SaveChangesCommand { get; set; }

        public ChangeScheduleTimeViewModel()
        {
            _systemConfigDataTransfer = new SystemConfigDataTransfer();
            TimeBetweenSlot = _systemConfigDataTransfer.GetTimeBetweenSlot();
            StartWorkingTime = _systemConfigDataTransfer.GetStartWorking();
            FinishWorkingTime = _systemConfigDataTransfer.GetFinishWorking();
            StartBreakTime = _systemConfigDataTransfer.GetStartBreak();
            FinishBreakTime = _systemConfigDataTransfer.GetFinishBreak();

            _oldTimeBetweenSlot = TimeBetweenSlot;
            _oldStartWorkingTime = StartWorkingTime;
            _oldFinishWorkingTime = FinishWorkingTime;
            _oldStartBreakTime = StartBreakTime;
            _oldFinishBreakTime = FinishBreakTime;

            SaveChangesCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SaveChanges(); });
        }

        private void SaveChanges()
        {
            ConvertTimeValue();
            _systemConfigDataTransfer.Update(StartWorkingTime);
            _systemConfigDataTransfer.Update(FinishWorkingTime);
            _systemConfigDataTransfer.Update(StartBreakTime);
            _systemConfigDataTransfer.Update(FinishBreakTime);
            _systemConfigDataTransfer.Update(TimeBetweenSlot);
            _systemConfigDataTransfer.Save();
        }

        private void ConvertTimeValue()
        {
            DateTime startWorkingTime;
            DateTime.TryParse(StartWorkingTime.AttributeValue, out startWorkingTime);
            StartWorkingTime.AttributeValue = startWorkingTime.TimeOfDay.ToString();

            DateTime finishWorkingTime;
            DateTime.TryParse(FinishWorkingTime.AttributeValue, out finishWorkingTime);
            FinishWorkingTime.AttributeValue = finishWorkingTime.TimeOfDay.ToString();

            DateTime startBreakTime;
            DateTime.TryParse(StartBreakTime.AttributeValue, out startBreakTime);
            StartBreakTime.AttributeValue = startBreakTime.TimeOfDay.ToString();

            DateTime finishBreakTime;
            DateTime.TryParse(FinishBreakTime.AttributeValue, out finishBreakTime);
            FinishBreakTime.AttributeValue = finishBreakTime.TimeOfDay.ToString();
        }

        public SystemConfig StartWorkingTime
        {
            get { return _startWorkingTime; }
            set
            {
                _startWorkingTime = value;
                NotifyPropertyChanged();
            }
        }
        public SystemConfig FinishWorkingTime
        {
            get { return _finishWorkingTime; }
            set
            {
                _finishWorkingTime = value;
                NotifyPropertyChanged();
            }
        }

        public SystemConfig StartBreakTime
        {
            get { return _startBreakTime; }
            set
            {
                _startBreakTime = value;
                NotifyPropertyChanged();
            }
        }

        public SystemConfig FinishBreakTime
        {
            get { return _finishBreakTime; }
            set
            {
                _finishBreakTime = value;
                NotifyPropertyChanged();
            }
        }
        public SystemConfig TimeBetweenSlot
        {
            get { return _timeBetweenSlot; }
            set
            {
                _timeBetweenSlot = value;
                NotifyPropertyChanged();
            }
        }
    }
}
