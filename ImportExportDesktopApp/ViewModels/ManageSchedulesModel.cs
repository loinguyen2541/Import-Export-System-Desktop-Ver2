using ImportExportDesktopApp.Models;
using ImportExportDesktopApp.Services;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class ManageSchedulesModel : BaseNotifyPropertyChanged
    {
        public ICommand SearchCommand { get; set; }
        public ICommand GetNextPageCommand { get; set; }
        public ICommand GetBeforePageCommand { get; set; }
        public ICommand CancelSearchCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        private Pagination<Schedule> _schedules;
        private QueryParams _paging;
        private List<String> _types;
        private ScheduleHttpService HttpService;
        private SystemConfigHttpService _systemConfigHttpService;
        private String _txtPageInfo;
        private bool _isMaxPage;
        private bool _isFirstPage;
        private bool _isSearch;
        private bool _isLoading;
        private string _autoschedule;

        public Thread trd { get; set; }
        public ManageSchedulesModel()
        {

            HttpService = new ScheduleHttpService();
            _systemConfigHttpService = new SystemConfigHttpService();


            Task.Run(() => { Init(); });

            GetNextPageCommand = new RelayCommand<QueryParams>((p) => { return true; }, GetNextPage);
            GetBeforePageCommand = new RelayCommand<QueryParams>((p) => { return true; }, GetBeforePage);
            SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                SearchSchedules();
            });
            RefreshCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                Refresh();
            });
            CancelSearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                CancelSearch();
            });
        }

        public void Init()
        {
            AutoSchedule = _systemConfigHttpService.getScheduledTime().Result;
            IsLoading = false;
            Schedules = HttpService.GetSchedules(1, 10, null, null, null).Result;
            Types = HttpService.GetTransactionTypes();

            Paging = new QueryParams();
            Paging.Size = Schedules.Size;
            Paging.Page = Schedules.Page;
            Paging.Type = Types[0];

            TxtPageInfo = String.Format("Page {0} of {1}", Schedules.Page, Schedules.TotalPage);
            CheckPage();
        }

        public async void SearchSchedules()
        {
            IsSearch = true;
            Pagination<Schedule> newSchedules = await HttpService.GetSchedules(1, Paging.Size, Paging.Date, Paging.Type, Paging.Search);
            RefreshTableAndLabel(newSchedules);
        }

        public void GetAutoSchedule()
        {
            AutoSchedule = _systemConfigHttpService.getScheduledTime().Result;
        }

        public async void GetNextPage(QueryParams query)
        {
            query.Page = query.Page + 1;
            Pagination<Schedule> schedules = await HttpService.GetSchedules(query.Page, query.Size, query.Date, query.Type, query.Search);
            RefreshTableAndLabel(schedules);
        }

        public void CancelSearch()
        {
            IsSearch = false;
            Paging.Date = "";
            Paging.Search = "";
            Paging.Type = _types[0];
            Paging.Page = 1;

            Pagination<Schedule> schedules = HttpService.GetSchedules(1, 10, null, null, null).Result;
            RefreshTableAndLabel(schedules);
        }

        public async void GetBeforePage(QueryParams query)
        {
            query.Page = query.Page - 1;
            Pagination<Schedule> schedules = await HttpService.GetSchedules(query.Page, query.Size, query.Date, query.Type, query.Search);
            RefreshTableAndLabel(schedules);
        }

        public void RefreshTableAndLabel(Pagination<Schedule> schedules)
        {
            Clear(schedules);
            TxtPageInfo = String.Format("Page {0} of {1}", Schedules.Page, Schedules.TotalPage);
            CheckPage();
        }

        public async void Refresh()
        {
            IsLoading = true;
            Pagination<Schedule> schedules = await HttpService.GetSchedules(Paging.Page, Paging.Size, Paging.Date, Paging.Type, Paging.Search);
            RefreshTableAndLabel(schedules);
            IsLoading = false;
        }

        public void CheckPage()
        {
            if (Schedules.Page >= Schedules.TotalPage)
            {
                IsMaxPage = true;
            }
            else
            {
                IsMaxPage = false;
            }

            if (Schedules.Page <= 1)
            {
                IsFirstPage = true;
            }
            else
            {
                IsFirstPage = false;
            }
        }

        public void Clear(Pagination<Schedule> schedules)
        {
            Schedules.Page = schedules.Page;
            Schedules.Size = schedules.Size;
            Schedules.TotalPage = schedules.TotalPage;
            Schedules.Data.Clear();
            foreach (var item in schedules.Data)
            {
                Schedules.Data.Add(item);
            }
        }


        public QueryParams Paging
        {
            get { return _paging; }
            set
            {
                _paging = value;
                NotifyPropertyChanged();
            }
        }


        public String TxtPageInfo
        {
            get { return _txtPageInfo; }
            set
            {
                _txtPageInfo = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsMaxPage
        {
            get { return _isMaxPage; }
            set
            {
                _isMaxPage = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsFirstPage
        {
            get { return _isFirstPage; }
            set
            {
                _isFirstPage = value;
                NotifyPropertyChanged();
            }
        }

        public Pagination<Schedule> Schedules
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
    }
}
