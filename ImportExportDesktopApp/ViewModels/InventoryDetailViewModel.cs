using ImportExportDesktopApp.DisplayModel;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class InventoryDetailViewModel : BaseNotifyPropertyChanged
    {
        public List<InventoryDetail> _listDetail { get; set; }
        private String _txtPageInfo;
        private bool _isMaxPage;
        private bool _isFirstPage;
        private String _partnerName;
        private Inventory _inventory;
        private InventoryDisplay _display;
        private readonly IEEntities ie;
        //public QueryParams Paging { get; set; }
        public ICommand SearchCommand { get; set; }
        //InventoryDetailHttpService detailService;
        //private readonly PartnerDetailHttpService _partnerDetailHttpService;
        //private readonly GoodsHttpService _goodsHttpService;
        public InventoryDetailViewModel()
        {
            ie = new IEEntities();
            //detailService = new InventoryDetailHttpService();
            //_partnerDetailHttpService = new PartnerDetailHttpService();
            //_goodsHttpService = new GoodsHttpService();
            ////ListDetails = detailService.GetInventoryDetails(1, 8, Inventory.InventoryId, null);
            //SearchCommand = new RelayCommand<object>((p) => { return true; }, p =>
            //{
            //    SearchTransaction();
            //});
        }

        public void Init(InventoryDisplay dispay)
        {
            Inventory = ie.Inventories.Find(dispay.InventoryId);
            InventoryDisplay = dispay;
            GetPartnerAndGood();
        }

        public void GetPartnerAndGood()
        {
            List<Good> goods = ie.Goods.ToList();
            if (Inventory != null && Inventory.InventoryDetails.Count > 0)
            {
                foreach (var item in Inventory.InventoryDetails)
                {
                    Partner partner = ie.Partners.Where(p => p.PartnerId == item.PartnerId).SingleOrDefault();
                    item.Partner = partner;
                    item.Good = goods[0];
                }
            }
        }
        public void SearchTransaction()
        {
            List<InventoryDetail> newInventory = ie.InventoryDetails.Where(d => d.InventoryId == Inventory.InventoryId && d.Partner.DisplayName.Contains(PartnerName)).ToList();
            Clear(newInventory);
        }
        //public void GetNextPage(QueryParams query)
        //{
        //    query.Page = query.Page + 1;
        //    List<InventoryDetail> details = detailService.GetInventoryDetails(query.Page, query.Size, Inventory.InventoryId, null);
        //    Clear(details);
        //    TxtPageInfo = String.Format("Page {0} of {1}", ListDetails.Page, ListDetails.TotalPage);
        //    CheckPage();
        //}

        //public void GetBeforePage(QueryParams query)
        //{
        //    query.Page = query.Page - 1;
        //    List<InventoryDetail> details = detailService.GetInventoryDetails(query.Page, query.Size, Inventory.InventoryId, null);
        //    Clear(details);
        //    TxtPageInfo = String.Format("Page {0} of {1}", ListDetails.Page, ListDetails.TotalPage);
        //    CheckPage();
        //}
        //public void CheckPage()
        //{
        //    if (ListDetails.Page >= ListDetails.TotalPage)
        //    {
        //        IsMaxPage = true;
        //    }
        //    else
        //    {
        //        IsMaxPage = false;
        //    }

        //    if (ListDetails.Page <= 1)
        //    {
        //        IsFirstPage = true;
        //    }
        //    else
        //    {
        //        IsFirstPage = false;
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
        public List<InventoryDetail> ListDetails
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
        public void Clear(List<InventoryDetail> details)
        {
            //ListDetails.Page = details.Page;
            //ListDetails.Size = details.Size;
            //ListDetails.TotalPage = details.TotalPage;
            //ListDetails.Data.Clear();
            //foreach (var item in details.Data)
            //{
            //    ListDetails.Data.Add(item);
            //}
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
        public InventoryDisplay InventoryDisplay
        {
            get { return _display; }
            set
            {
                _display = value;
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
