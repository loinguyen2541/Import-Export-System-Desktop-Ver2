using ImportExportDesktopApp.Commands;
using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.DisplayModel;
using ImportExportDesktopApp.HttpServices;
using ImportExportDesktopApp.Pages;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class ManInventoriesViewModel : BaseNotifyPropertyChanged
    {
        private Transaction _trans;
        private InventoryDisplay _selectedInventory;
        public List<InventoryDisplay> ListInventory { get; set; }

        private bool _isSearch;
        private int _currentPage = 1;
        private bool _isLoading;
        private string _pagingInfo;
        private int _maxPage;
        public List<String> Types { get; }
        private String _fromDate;
        private String _toDate;
        private InventoryDataTransfer _inventoryDataTransfer;
        public ICommand SearchCommand { get; set; }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand BeforePageCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand CancelSearchCommand { get; set; }
        public int transactionID { get; set; }
        private readonly IEEntities ie;


        public ManInventoriesViewModel()
        {
            IsSearch = false;
            IsLoading = false;
            _inventoryDataTransfer = new InventoryDataTransfer();
            MaxPage = _inventoryDataTransfer.GetMaxPage(10);
            Init();

            SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                SearchInventory();
            });
            _selectedInventory = new InventoryDisplay();
            DoubleClickCommand = new RelayCommand<InventoryDisplay>((p) => { return true; }, p =>
            {
                if (p != null)
                    SearchInventoryDetailByInventory(p);
            });
            NextPageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                NextPage();
            });
            BeforePageCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                BeforePage();
            });
            CancelSearchCommand = new RelayCommand<Object>((p) => { return true; }, p =>
            {
                CancelSearch();
            });
            RefreshCommand = new RelayCommand<Object>((p) => { return true; }, p =>
            {
                Refresh();
            });

        }

        private void Init()
        {
            ObservableCollection<Inventory> inventory = _inventoryDataTransfer.GetAllInventory(CurrentPage);
            ListInventory = new List<InventoryDisplay>();
            if (inventory != null)
            {
                foreach (var item in inventory)
                {

                    ListInventory.Add(GetDisplayInventory(item));
                }
            }
            SetPagingInfo();
        }

        private void SearchInventory()
        {
            DateTime startDate, endDate;
            CurrentPage = 1;
            if (DateTime.TryParse(FromDate, out startDate) && DateTime.TryParse(ToDate, out endDate))
            {
                ObservableCollection<Inventory> inventory = _inventoryDataTransfer.SearchInventory(startDate, endDate, CurrentPage);
                if (inventory != null && inventory.Count!=0)
                {
                    foreach (var item in inventory)
                    {

                        ListInventory.Add(GetDisplayInventory(item));
                    }
                }
            }
        }

        private InventoryDisplay GetDisplayInventory(Inventory item)
        {
            InventoryDisplay temp = new InventoryDisplay();
            temp.InventoryId = item.InventoryId;
            temp.OpeningStock = item.OpeningStock;
            temp.RecordedDate = item.RecordedDate;
            if (item.InventoryDetails.Count != 0)
            {
                float totalImport = 0, totalExport = 0;
                foreach (var detail in item.InventoryDetails)
                {
                    if (detail.Type == 0)
                    {
                        totalImport += detail.Weight;
                    }
                    if (detail.Type == 1)
                    {
                        totalExport += detail.Weight;
                    }
                }
                temp.TotalExport = totalExport;
                temp.TotalImport = totalImport;
                temp.ClosingStock = (float)Math.Round(temp.OpeningStock + temp.TotalImport - temp.TotalExport, 2);
            }
            else
            {
                temp.TotalExport = 0;
                temp.TotalImport = 0;
                temp.ClosingStock = (float)Math.Round(temp.OpeningStock, 2);
            }
            return temp;
        }
        public Transaction Transaction
        {
            get { return _trans; }
            set
            {
                _trans = value;
                NotifyPropertyChanged();
            }
        }
        public void SearchInventoryDetailByInventory(InventoryDisplay inventory)
        {
            InventoryDetailWindow detailTransactinoWindow = new InventoryDetailWindow(inventory: inventory);
            detailTransactinoWindow.ShowDialog();
        }

        public void NextPage()
        {
            CurrentPage++;
            ObservableCollection<Inventory> inventory = _inventoryDataTransfer.GetAllInventory(CurrentPage);
            ListInventory = new List<InventoryDisplay>();
            if (inventory != null)
            {
                foreach (var item in inventory)
                {

                    ListInventory.Add(GetDisplayInventory(item));
                }
            }
            SetPagingInfo();
        }
        public void BeforePage()
        {
            CurrentPage--;
            ObservableCollection<Inventory> inventory = _inventoryDataTransfer.GetAllInventory(CurrentPage);
            ListInventory = new List<InventoryDisplay>();
            if (inventory != null)
            {
                foreach (var item in inventory)
                {

                    ListInventory.Add(GetDisplayInventory(item));
                }
            }
            SetPagingInfo();
        }

        public void CancelSearch()
        {
            IsSearch = false;
            CurrentPage = 1;
            ObservableCollection<Inventory> inventory = _inventoryDataTransfer.GetAllInventory(CurrentPage);
            ListInventory = new List<InventoryDisplay>();
            if (inventory != null)
            {
                foreach (var item in inventory)
                {

                    ListInventory.Add(GetDisplayInventory(item));
                }
            }
        }

        public void Refresh()
        {
            IsLoading = true;
            CurrentPage = 1;
            ObservableCollection<Inventory> inventory = _inventoryDataTransfer.GetAllInventory(CurrentPage);
            ListInventory = new List<InventoryDisplay>();
            if (inventory != null)
            {
                foreach (var item in inventory)
                {

                    ListInventory.Add(GetDisplayInventory(item));
                }
            }
            IsLoading = false;
        }
        private void SetPagingInfo()
        {
            PagingInfo = String.Format("Page {0} of {1}", CurrentPage, MaxPage);
        }
        public String FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                NotifyPropertyChanged();
            }
        }

        public String ToDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
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

        public InventoryDisplay SelectedInventory
        {
            get { return _selectedInventory; }
            set
            {
                _selectedInventory = value;
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
        public int MaxPage
        {
            get { return _maxPage; }
            set
            {
                _maxPage = value;
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
    }
}
