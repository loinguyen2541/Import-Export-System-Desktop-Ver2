﻿using ImportExportDesktopApp.Utils;
using ImportExportDesktopApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ImportExportDesktopApp.DataTransfers;
using System.Collections.ObjectModel;

namespace ImportExportDesktopApp.ViewModels
{
    class ManSchedulesViewModel : BaseNotifyPropertyChanged
    {
        public ICommand SearchCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand BeforePageCommand { get; set; }
        public ICommand CancelSearchCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        private ObservableCollection<Schedule> _schedules;
        private List<String> _types;
        private String _txtPageInfo;
        private bool _isMaxPage;
        private bool _isFirstPage;
        private bool _isSearch;
        private bool _isLoading;
        private string _autoschedule;
        private int _currentPage = 1;
        private ScheduleDataTransfer _scheduleDataTransfer;
        private SystemConfigDataTransfer _systemDataTransfer;

        public Thread trd { get; set; }
        public ManSchedulesViewModel()
        {
            _scheduleDataTransfer = new ScheduleDataTransfer();
            _systemDataTransfer = new SystemConfigDataTransfer();
            Task.Run(() => { Init(); });

            //SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            //{
            //    SearchSchedules();
            //});
            RefreshCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                Refresh();
            });
            CancelSearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                CancelSearch();
            });
            NextPageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                NextPage();
            });
            BeforePageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                BeforePage();
            });
        }

        public void Init()
        {
            SystemConfig system = _systemDataTransfer.GetTimeSchedule();
            AutoSchedule = system.AttributeValue;
            Schedules = _scheduleDataTransfer.GetAllSchedule(CurrentPage);
            Types = new List<string>();
            Types.Add("Import");
            Types.Add("Export");
        }

        public async void SearchSchedules()
        {
            //IsSearch = true;
            //ObservableCollection<Schedule> newSchedules = ie.Schedules.Where(s => s.ScheduleDate);
            //RefreshTableAndLabel(newSchedules);
        }

        public void CancelSearch()
        {
            IsSearch = false;
            CurrentPage = 1;
            Schedules = _scheduleDataTransfer.GetAllSchedule(CurrentPage);
        }

        public async void Refresh()
        {
            IsLoading = true;
            CurrentPage = 1;
            ObservableCollection<Schedule> schedule = _scheduleDataTransfer.GetAllSchedule(CurrentPage);
            Schedules.Clear();
            foreach (var item in schedule)
            {
                Schedules.Add(item);
            }
            IsLoading = false;
        }

        public void NextPage()
        {
            CurrentPage++;
            Schedules = _scheduleDataTransfer.GetAllSchedule(CurrentPage);
        }
        public void BeforePage()
        {
            CurrentPage--;
            Schedules = _scheduleDataTransfer.GetAllSchedule(CurrentPage);
        }

        public ObservableCollection<Schedule> Schedules
        {
            get { return _schedules; }
            set
            {
                _schedules = value;
                NotifyPropertyChanged();
            }
        }

        public List<String> Types
        {
            get { return _types; }
            set
            {
                _types = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSearch
        {
            get { return _isSearch; }
            set
            {
                _isSearch = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged();
            }
        }

        public string AutoSchedule
        {
            get { return _autoschedule; }
            set
            {
                _autoschedule = value;
                NotifyPropertyChanged();
            }
        }
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                NotifyPropertyChanged();
            }
        }
    }
}
