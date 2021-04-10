using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImportExportDesktopApp.Pages;
using ImportExportDesktopApp.ViewModels;

namespace ImportExportDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Frame _mainFrame;
        private ProcessingPage processingPage;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(AppService.Instance.EventAggregator);
            _mainFrame = (Frame)this.FindName("MainFrame");
            if (processingPage == null)
            {
                processingPage = new ProcessingPage();
            }
            _mainFrame.Navigate(processingPage);
        }

        public void Navigate(string value)
        {
            switch (value)
            {
                case "ChartDonut":
                    if (processingPage == null)
                    {
                        processingPage = new ProcessingPage();
                    }
                    _mainFrame.Navigate(processingPage);
                    break;
                //case "AccountMultiple":
                //    if (partnerList == null)
                //    {
                //        partnerList = new PartnerList();
                //    }
                //    mainFrame.Navigate(partnerList);
                //    break;
                //case "schedule":
                //    if (scheduleDetail == null)
                //    {
                //        scheduleDetail = new ScheduleDetail();
                //    }
                //    mainFrame.Navigate(scheduleDetail);
                //    break;
                //case "PackageVariantClosed":
                //    if (inventoriesScreen == null)
                //    {
                //        inventoriesScreen = new ManInventoriesScreen();
                //    }
                //    mainFrame.Navigate(inventoriesScreen);
                //    break;
                //case "Gift":
                //    if (manGoodsScreen == null)
                //    {
                //        manGoodsScreen = new ManGoodsScreen();
                //    }
                //    mainFrame.Navigate(manGoodsScreen);
                //    break;
                default:
                    break;
            }
        }

        private void page_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void page_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
