﻿using ImportExportDesktopApp.Models;
using ImportExportDesktopApp.Pages;
using ImportExportDesktopApp.Services;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class ManageInventoryModel : BaseNotifyPropertyChanged
    {
        private Transaction _trans;
        private Inventory _selectedInventory;
        public Pagination<Inventory> ListInventory { get; set; }

        private bool _isSearch;

        private bool _isLoading;

        public QueryParams Paging { get; set; }
        public List<String> Types { get; }
        private String _fromDate;
        private String _toDate;
        public ICommand SearchCommand { get; set; }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand GetNextPageCommand { get; set; }
        public ICommand GetBeforePageCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand CancelSearchCommand { get; set; }

        private String _txtPageInfo;
        private bool _isMaxPage;
        private bool _isFirstPage;
        InventoryHttpService inventoryService;
        public int transactionID { get; set; }


        public ManageInventoryModel()
        {
            IsSearch = false;
            IsLoading = false;

            inventoryService = new InventoryHttpService();
            ListInventory = inventoryService.GetInventory(1, 8, null, null).Result;
            formatDateRecord(ListInventory);
            Paging = new QueryParams();
            Paging.Size = ListInventory.Size;
            Paging.Page = ListInventory.Page;

            _txtPageInfo = String.Format("Page {0} of {1}", ListInventory.Page, ListInventory.TotalPage);
            CheckPage();

            SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                SearchInventory();
            });
            DoubleClickCommand = new RelayCommand<Inventory>((p) => { return true; }, p =>
            {
                SearchInventoryDetailByInventory(p);
            });
            GetNextPageCommand = new RelayCommand<QueryParams>((p) => { return true; }, GetNextPage);

            GetBeforePageCommand = new RelayCommand<QueryParams>((p) => { return true; }, GetBeforePage);
            CancelSearchCommand = new RelayCommand<Object>((p) => { return true; }, p =>
            {
                CancelSearch();
            });
            RefreshCommand = new RelayCommand<Object>((p) => { return true; }, p =>
            {
                Refresh();
            });

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
        public void SearchInventoryDetailByInventory(Inventory inventory)
        {
            InventoryDetailWindow detailTransactinoWindow = new InventoryDetailWindow(inventory: inventory);
            detailTransactinoWindow.ShowDialog();
        }

        public void SearchInventory()
        {
            IsSearch = true;
            Pagination<Inventory> newInventory = inventoryService.GetInventory(Paging.Page, Paging.Size, FromDate, ToDate).Result;
            formatDateRecord(newInventory);
            RefreshTableAndLabel(newInventory);
        }

        public void GetNextPage(QueryParams query)
        {
            query.Page = query.Page + 1;
            Pagination<Inventory> inventory = inventoryService.GetInventory(query.Page, query.Size, FromDate, ToDate).Result;
            RefreshTableAndLabel(inventory);
        }

        public void GetBeforePage(QueryParams query)
        {
            query.Page = query.Page - 1;
            Pagination<Inventory> inventory = inventoryService.GetInventory(query.Page, query.Size, FromDate, ToDate).Result;
            RefreshTableAndLabel(inventory);
        }
        public void formatDateRecord(Pagination<Inventory> inventories)
        {
            String[] temp;
            if (inventories != null && inventories.Data != null)
            {
                foreach (var item in inventories.Data)
                {
                    //temp = item.RecordedDate.Split('T');
                    //item.RecordedDate = temp[0];
                }
            }
        }

        public void CancelSearch()
        {
            IsSearch = false;
            Paging.Date = "";
            Paging.Search = "";
            Paging.Type = "";
            Paging.Page = 1;
            FromDate = "";
            ToDate = "";
            Pagination<Inventory> inventories = inventoryService.GetInventory(Paging.Page, Paging.Size, null, null).Result;
            RefreshTableAndLabel(inventories);
        }

        public async void Refresh()
        {
            IsLoading = true;
            Pagination<Inventory> inventories = await inventoryService.GetInventory(Paging.Page, Paging.Size, FromDate, ToDate);
            RefreshTableAndLabel(inventories);
            IsLoading = false;
        }

        public void RefreshTableAndLabel(Pagination<Inventory> inventories)
        {
            Clear(inventories);
            TxtPageInfo = String.Format("Page {0} of {1}", inventories.Page, inventories.TotalPage);
            CheckPage();
        }

        public void CheckPage()
        {
            if (ListInventory.Page >= ListInventory.TotalPage)
            {
                IsMaxPage = true;
            }
            else
            {
                IsMaxPage = false;
            }

            if (ListInventory.Page <= 1)
            {
                IsFirstPage = true;
            }
            else
            {
                IsFirstPage = false;
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

        public void Clear(Pagination<Inventory> inventories)
        {
            ListInventory.Page = inventories.Page;
            ListInventory.Size = inventories.Size;
            ListInventory.TotalPage = inventories.TotalPage;
            ListInventory.Data.Clear();
            foreach (var item in inventories.Data)
            {
                ListInventory.Data.Add(item);
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

        public Inventory SelectedInventory
        {
            get { return _selectedInventory; }
            set
            {
                _selectedInventory = value;
                NotifyPropertyChanged();
            }
        }
    }
}
