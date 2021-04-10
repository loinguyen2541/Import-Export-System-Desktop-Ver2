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
    class ManPartnersViewModel : BaseNotifyPropertyChanged
    {
        private List<Partner> _partners;
        //private QueryParams _paging;
        private List<PartnerType> _types;

        private bool _isSearch;

        private PartnerType _selectedType;
        public Partner TableSelectedItem { get; set; }
        //private PartnerHttpService HttpService;
        private bool _isLoading;
        public ICommand SearchCommand { get; set; }
        public ICommand TableDoubleClickCommand { get; set; }
        public ICommand GetNextPageCommand { get; set; }
        public ICommand GetBeforePageCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand CancelSearchCommand { get; set; }

        private String _txtPageInfo;
        private bool _isMaxPage;
        private bool _isFirstPage;

        private readonly IEEntities ie;

        public ManPartnersViewModel()
        {

            //HttpService = new PartnerHttpService();
            ie = new IEEntities();
            Task.Run(() =>
            {
                Init();
            });

            //GetNextPageCommand = new RelayCommand<QueryParams>((p) => { return true; }, GetNextPage);
            //GetBeforePageCommand = new RelayCommand<QueryParams>((p) => { return true; }, GetBeforePage);
            //TableDoubleClickCommand = new RelayCommand<object>((p) => { return true; }, p =>
            //{
            //    OpenDialog();
            //});
            //SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            //{
            //    SearchSchedules();
            //});
            //RefreshCommand = new RelayCommand<object>((p) => { return true; }, p =>
            //{
            //    Refresh();
            //});
            //CancelSearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            //{
            //    CancelSearch();
            //});
        }

        public void Init()
        {
            try
            {
                Partners = ie.Partners.Where(p => p.PartnerStatus == 0).ToList();
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            Types = ie.PartnerTypes.ToList();

            IsLoading = false;
            IsSearch = false;


            SelectedType = Types[0];

            TxtPageInfo = String.Format("Page {0} of {1}", 0, 0);
            CheckPage();
        }

        private void OpenDialog()
        {
            //Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //Partner newPartner = HttpService.GetPartnerById(TableSelectedItem.PartnerId);
            //Pagination<Transaction> transactions = HttpService.GetTransactionByPartxnerId(1, 3, TableSelectedItem.PartnerId);
            //PartnerDetailWindow window = new PartnerDetailWindow(newPartner, transactions);
            //window.ShowDialog();
        }

        public void SearchSchedules()
        {
            IsSearch = true;
            //Paging.Page = 1;
            //if (SelectedType.PartnerTypeId != 0)
            //{
            //    Paging.Type = SelectedType.PartnerTypeName;
            //}
            //else
            //{
            //    Paging.Type = null;
            //}
            //Pagination<Partner> partners = HttpService.SearchPartner(Paging).Result;
            //RefreshTableAndLabel(partners);
        }

        //public void GetNextPage(QueryParams query)
        //{
        //    query.Page = query.Page + 1;
        //    if (SelectedType.PartnerTypeId != 0)
        //    {
        //        Paging.Type = SelectedType.PartnerTypeName;
        //    }
        //    else
        //    {
        //        Paging.Type = null;
        //    }
        //    Pagination<Partner> partners = HttpService.SearchPartner(query).Result;
        //    RefreshTableAndLabel(partners);
        //}

        //public void GetBeforePage(QueryParams query)
        //{
        //    query.Page = query.Page - 1;
        //    if (SelectedType.PartnerTypeId != 0)
        //    {
        //        Paging.Type = SelectedType.PartnerTypeName;
        //    }
        //    else
        //    {
        //        Paging.Type = null;
        //    }
        //    Pagination<Partner> partners = HttpService.SearchPartner(query).Result;
        //    RefreshTableAndLabel(partners);
        //}
        //public async void Refresh()
        //{
        //    IsLoading = true;
        //    Pagination<Partner> partners = await HttpService.SearchPartner(Paging);
        //    RefreshTableAndLabel(partners);
        //    IsLoading = false;
        //}

        public void CheckPage()
        {
            //if (Partners.Page >= Partners.TotalPage)
            //{
            //    IsMaxPage = true;
            //}
            //else
            //{
            //    IsMaxPage = false;
            //}

            //if (Partners.Page <= 1)
            //{
            //    IsFirstPage = true;
            //}
            //else
            //{
            //    IsFirstPage = false;
            //}
        }

        //public void CancelSearch()
        //{
        //    IsSearch = false;
        //    Paging.Date = "";
        //    Paging.Search = "";
        //    Paging.Type = "";
        //    Paging.Page = 1;
        //    Paging.Page = 1;
        //    Pagination<Partner> partners = HttpService.SearchPartner(null).Result;
        //    RefreshTableAndLabel(partners);
        //}

        //public void RefreshTableAndLabel(Pagination<Partner> partners)
        //{
        //    Clear(partners);
        //    TxtPageInfo = String.Format("Page {0} of {1}", Partners.Page, Partners.TotalPage);
        //    CheckPage();
        //}

        //public void Clear(Pagination<Partner> schedules)
        //{
        //    Partners.Page = schedules.Page;
        //    Partners.Size = schedules.Size;
        //    Partners.TotalPage = schedules.TotalPage;
        //    Partners.Data.Clear();
        //    foreach (var item in schedules.Data)
        //    {
        //        Partners.Data.Add(item);
        //    }
        //}

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

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged();
            }
        }

        public PartnerType SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
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

        //public QueryParams Paging
        //{
        //    get { return _paging; }
        //    set
        //    {
        //        _paging = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        public List<Partner> Partners
        {
            get { return _partners; }
            set
            {
                _partners = value;
                NotifyPropertyChanged();
            }
        }

        public List<PartnerType> Types
        {
            get { return _types; }
            set
            {
                _types = value;
                NotifyPropertyChanged();
            }
        }
    }
}
