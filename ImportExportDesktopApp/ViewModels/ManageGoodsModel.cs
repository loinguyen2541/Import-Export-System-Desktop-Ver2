using ImportExportDesktopApp.Services;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportExportDesktopApp.ViewModels
{
    class ManageGoodsModel : BaseNotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Good> Good { get; set; }
        public List<String> Status { get; }
        private GoodHttpService HttpService;
        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public String selectedItemCbx { get; set; }
        private Good goodsTemp;
        public ManageGoodsModel()
        {
            HttpService = new GoodHttpService();
            Good = HttpService.GetGood();
            Status = HttpService.GetStatus();
            goodsTemp = new Good();
            goodsTemp.GoodsId = Good.First().GoodsId;
            goodsTemp.GoodName = Good.First().GoodName;
            EditCommand = new RelayCommand<Good>((g) => { return true; }, editGood);
            AddCommand = new RelayCommand<Good>((g) => { return true; }, addGood);
        }

        void editGood(Good GoodTemp)
        {
            HttpService.UpdateGood(GoodTemp);
            Good[0] = HttpService.GetGood().First();
        }
        void addGood(Good Good)
        {
            HttpService.AddGood(Good);
        }

        public Good GoodTemp
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
