using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.ViewModels
{
    class ScheduleSettingViewModel : BaseNotifyPropertyChanged
    {
        private SystemConfigDataTransfer _systemConfigDataTransfer;
        private TimeTemplateItemDataTransfer _timeTemplateItemDataTransfer;

        private SystemConfig _startWorkingTime;
        private SystemConfig _finishWorkingTime;
        private SystemConfig _startBreakTime;
        private SystemConfig _finishBreakTime;
        private SystemConfig _timeBetweenSlot;
        public ScheduleSettingViewModel()
        {
            _systemConfigDataTransfer = new SystemConfigDataTransfer();
            _timeTemplateItemDataTransfer = new TimeTemplateItemDataTransfer();

            TimeBetweenSlot = _systemConfigDataTransfer.GetTimeBetweenSlot();
            StartWorkingTime = _systemConfigDataTransfer.GetStartWorking();
            FinishWorkingTime = _systemConfigDataTransfer.GetFinishWorking();
            StartBreakTime = _systemConfigDataTransfer.GetStartBreak();
            FinishBreakTime = _systemConfigDataTransfer.GetFinishBreak();
        }

        public void ResetData()
        {
            TimeBetweenSlot = _systemConfigDataTransfer.GetTimeBetweenSlot();
            StartWorkingTime = _systemConfigDataTransfer.GetStartWorking();
            FinishWorkingTime = _systemConfigDataTransfer.GetFinishWorking();
            StartBreakTime = _systemConfigDataTransfer.GetStartBreak();
            FinishBreakTime = _systemConfigDataTransfer.GetFinishBreak();
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
