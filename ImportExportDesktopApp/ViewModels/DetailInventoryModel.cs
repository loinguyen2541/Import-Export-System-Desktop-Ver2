using ImportExportDesktopApp.Models;
using ImportExportDesktopApp.Services;
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
    class DetailInventoryModel : BaseNotifyPropertyChanged
    {
        public Pagination<InventoryDetail> _listDetail { get; set; }
        private String _txtPageInfo;
        private bool _isMaxPage;
        private bool _isFirstPage;
        private String _partnerName;
        private Inventory _inventory;
        public QueryParams Paging { get; set; }
        public ICommand SearchCommand { get; set; }
        InventoryDetailHttpService detailService;
        private readonly PartnerDetailHttpService _partnerDetailHttpService;
        private readonly GoodHttpService _GoodHttpService;
        public DetailInventoryModel()
        {
            detailService = new InventoryDetailHttpService();
            _partnerDetailHttpService = new PartnerDetailHttpService();
            _GoodHttpService = new GoodHttpService();
            //ListDetails = detailService.GetInventoryDetails(1, 8, Inventory.InventoryId, null);
            SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                SearchTransaction();
            });
        }

        public void Init(Inventory inventory)
        {
            Inventory = inventory;
            GetPartnerAndGood();

        }

        public void GetPartnerAndGood()
        {
            ObservableCollection<Good> Good = _GoodHttpService.GetGood();
            if (Inventory != null && Inventory.InventoryDetails.Count > 0)
            {
                foreach (var item in Inventory.InventoryDetails)
                {
                    Partner partner = _partnerDetailHttpService.GetPartnerById(item.PartnerId);
                    item.Partner = partner;
                    item.Good = Good[0];
                }
            }
        }
        public void SearchTransaction()
        {
            Pagination<InventoryDetail> newInventory = detailService.GetInventoryDetails(1, 8, Inventory.InventoryId, PartnerName);
            Clear(newInventory);
            CheckPage();
        }
        public void GetNextPage(QueryParams query)
        {
            query.Page = query.Page + 1;
            Pagination<InventoryDetail> details = detailService.GetInventoryDetails(query.Page, query.Size, Inventory.InventoryId, null);
            Clear(details);
            TxtPageInfo = String.Format("Page {0} of {1}", ListDetails.Page, ListDetails.TotalPage);
            CheckPage();
        }

        public void GetBeforePage(QueryParams query)
        {
            query.Page = query.Page - 1;
            Pagination<InventoryDetail> details = detailService.GetInventoryDetails(query.Page, query.Size, Inventory.InventoryId, null);
            Clear(details);
            TxtPageInfo = String.Format("Page {0} of {1}", ListDetails.Page, ListDetails.TotalPage);
            CheckPage();
        }
        public void CheckPage()
        {
            if (ListDetails.Page >= ListDetails.TotalPage)
            {
                IsMaxPage = true;
            }
            else
            {
                IsMaxPage = false;
            }

            if (ListDetails.Page <= 1)
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
        public Pagination<InventoryDetail> ListDetails
        {
            get { return _listDetail; }
            set
            {
                _listDetail = value;
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
        public void Clear(Pagination<InventoryDetail> details)
        {
            ListDetails.Page = details.Page;
            ListDetails.Size = details.Size;
            ListDetails.TotalPage = details.TotalPage;
            ListDetails.Data.Clear();
            foreach (var item in details.Data)
            {
                ListDetails.Data.Add(item);
            }
        }
        public Inventory Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                NotifyPropertyChanged();
            }
        }
        public String PartnerName
        {
            get { return _partnerName; }
            set
            {
                _partnerName = value;
                NotifyPropertyChanged();
            }
        }
    }
}