using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

/**
* @author Loi Nguyen
*
* @date - 4/7/2021 5:53:03 PM 
*/

namespace ImportExportDesktopApp.ViewModels
{
    class MainViewModel
    {
        public KeyValuePair<String, String> SelectedItem { get; set; }
        public Dictionary<String, String> PageNameList { get; set; }
        private Page _page;

        public MainViewModel()
        {
            PageNameList = new Dictionary<string, string>();
            PageNameList.Add("Processing", "ChartDonut");
            PageNameList.Add("Partner", "AccountMultiple");
            PageNameList.Add("Schedules", "schedule");
            PageNameList.Add("Inventory", "PackageVariantClosed");
            PageNameList.Add("Goods", "Gift");
            SelectedItem = PageNameList.First();
        }

        public Page Page
        {
            get { return _page; }
            set
            {
                _page = value;
                //NotifyPropertyChanged();
            }
        }
    }
}
