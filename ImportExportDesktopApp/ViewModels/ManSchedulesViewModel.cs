using ImportExportDesktopApp.Utils;
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
        private String _searchDate;
        private String _searchType;
        private String _searchName;
        private bool _isSearch;
        private bool _isLoading;
        private string _autoschedule;
        private int _currentPage = 1;
        private string _pagingInfo;
        private int _maxPage;
        private ScheduleDataTransfer _scheduleDataTransfer;
        private SystemConfigDataTransfer _systemDataTransfer;

        public Thread trd { get; set; }
        public ManSchedulesViewModel()
        {
            _scheduleDataTransfer = new ScheduleDataTransfer();
            _systemDataTransfer = new SystemConfigDataTransfer();

            MaxPage = _scheduleDataTransfer.GetMaxPage(10);
            Task.Run(() => { Init(); });

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
            SetPagingInfo();
        }

        public void SearchSchedules()
        {
            IsSearch = true;
            DateTime searchDate;
            if (DateTime.TryParse(SearchDate, out searchDate))
            {
                Schedules = _scheduleDataTransfer.SearchSchedule(searchDate, SearchName);
            }
        }

        public void CancelSearch()
        {
            IsSearch = false;
            CurrentPage = 1;
            Schedules = _scheduleDataTransfer.GetAllSchedule(CurrentPage);
        }

        public void Refresh()
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
            SetPagingInfo();
        }
        public void BeforePage()
        {
            CurrentPage--;
            Schedules = _scheduleDataTransfer.GetAllSchedule(CurrentPage);
            SetPagingInfo();
        }
        private void SetPagingInfo()
        {
            PagingInfo = String.Format("Page {0} of {1}", CurrentPage, MaxPage);
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
        public string SearchDate
        {
            get { return _searchDate; }
            set
            {
                _searchDate = value;
                NotifyPropertyChanged();
            }
        }
        public string SelectedType
        {
            get { return _searchType; }
            set
            {
                _searchType = value;
                NotifyPropertyChanged();
            }
        }
        public string SearchName
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
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

        public String PagingInfo
        {
            get { return _pagingInfo; }
            set
            {
                _pagingInfo = value;
                NotifyPropertyChanged();
            }
        }
        public int MaxPage
        {
            get { return _maxPage; }
            set
            {
                _maxPage = value;
                NotifyPropertyChanged();
            }
        }

    }
}
