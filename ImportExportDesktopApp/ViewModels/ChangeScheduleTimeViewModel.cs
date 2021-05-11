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
    class ChangeScheduleTimeViewModel : BaseNotifyPropertyChanged
    {
        private SystemConfigDataTransfer _systemConfigDataTransfer;
        private TimeTemplateItemDataTransfer _timeTemplateItemDataTransfer;

        private SystemConfig _startWorkingTime;
        private SystemConfig _finishWorkingTime;
        private SystemConfig _startBreakTime;
        private SystemConfig _finishBreakTime;
        private SystemConfig _timeBetweenSlot;

        private String _oldStartWorkingTime;
        private String _oldFinishWorkingTime;
        private String _oldStartBreakTime;
        private String _oldFinishBreakTime;
        private String _oldTimeBetweenSlot;
        public ICommand SaveChangesCommand { get; set; }

        public ChangeScheduleTimeViewModel()
        {
            _systemConfigDataTransfer = new SystemConfigDataTransfer();
            _timeTemplateItemDataTransfer = new TimeTemplateItemDataTransfer();

            TimeBetweenSlot = _systemConfigDataTransfer.GetTimeBetweenSlot();
            StartWorkingTime = _systemConfigDataTransfer.GetStartWorking();
            FinishWorkingTime = _systemConfigDataTransfer.GetFinishWorking();
            StartBreakTime = _systemConfigDataTransfer.GetStartBreak();
            FinishBreakTime = _systemConfigDataTransfer.GetFinishBreak();

            _oldTimeBetweenSlot = TimeBetweenSlot.AttributeValue;
            _oldStartWorkingTime = StartWorkingTime.AttributeValue;
            _oldFinishWorkingTime = FinishWorkingTime.AttributeValue;
            _oldStartBreakTime = StartBreakTime.AttributeValue;
            _oldFinishBreakTime = FinishBreakTime.AttributeValue;

            SaveChangesCommand = new RelayCommand<Window>((p) => { return true; }, SaveChanges);
        }

        private void SaveChanges(Window window)
        {
            try
            {
                ConvertTimeValue();
                if (IsChanged())
                {
                    _timeTemplateItemDataTransfer.CreateListTimeTemplateItem(
                        StartWorkingTime.AttributeValue,
                        FinishWorkingTime.AttributeValue,
                        StartBreakTime.AttributeValue,
                        FinishBreakTime.AttributeValue,
                        TimeBetweenSlot.AttributeValue
                        );
                }
                _systemConfigDataTransfer.Update(StartWorkingTime);
                _systemConfigDataTransfer.Update(FinishWorkingTime);
                _systemConfigDataTransfer.Update(StartBreakTime);
                _systemConfigDataTransfer.Update(FinishBreakTime);
                _systemConfigDataTransfer.Update(TimeBetweenSlot);
                _systemConfigDataTransfer.Save();
                MessageBox.Show("Thành công! Thay đổi của bạn sẽ được áp dụng vào ngày làm việc kế tiếp.");
                window.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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

        private Boolean IsChanged()
        {
            if (_oldTimeBetweenSlot != TimeBetweenSlot.AttributeValue)
            {
                return true;
            }
            if (_oldStartWorkingTime != StartWorkingTime.AttributeValue)
            {
                return true;
            }
            if (_oldFinishWorkingTime != FinishWorkingTime.AttributeValue)
            {
                return true;
            }
            if (_oldStartBreakTime != StartBreakTime.AttributeValue)
            {
                return true;
            }
            if (_oldFinishBreakTime != FinishBreakTime.AttributeValue)
            {
                return true;
            }
            return false;
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
