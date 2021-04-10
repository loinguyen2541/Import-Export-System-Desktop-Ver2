using ImportExportDesktopApp.DisplayModel;
using ImportExportDesktopApp.HttpServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class ManGoods : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<GoodsDisplay> goods { get; set; }
        public List<String> Status { get; }
        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public String selectedItemCbx { get; set; }
        private Good goodsTemp;
        private readonly IEEntities ie;
        public ManGoods()
        {
            ie = new IEEntities();
            List<Good> listgood = new List<Good>();
            listgood = ie.Goods.ToList();
            goods = new List<GoodsDisplay>();
            foreach (var item in listgood)
            {
                GoodsDisplay temp = new GoodsDisplay();
                temp.GoodsId = item.GoodsId;
                temp.GoodName = item.GoodName;
                temp.QuantityOfInventory = item.QuantityOfInventory;
                if(item.GoodsStatus == 0)
                {
                    temp.GoodsStatus = "Available";
                }
                else
                {
                    temp.GoodsStatus = "UnAvailable";
                }
                goods.Add(temp);
            }
            Status = new List<string>();
            Status.Add("Available");
            Status.Add("UnAvailable");
            selectedItemCbx = Status.First();
            GoodsTemp = new Good();
            goodsTemp.GoodsId = goods.First().GoodsId;
            goodsTemp.GoodName = goods.First().GoodName;
            EditCommand = new RelayCommand<Good>((g) => { return true; }, editGoods);
            AddCommand = new RelayCommand<Good>((g) => { return true; }, addGoods);
        }

        void editGoods(Good goodsTemp)
        {
            //HttpService.UpdateGoods(goodsTemp);
            //goods[0] = HttpService.GetGoods().First();
        }
        void addGoods(Good goods)
        {
            //HttpService.AddGoods(goods);
        }

        public Good GoodsTemp
        {
            get { return goodsTemp; }
            set
            {
                goodsTemp = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
