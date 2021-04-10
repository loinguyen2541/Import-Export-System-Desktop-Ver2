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
using ImportExportDesktopApp.Events;
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
        private ManageInventoriesScreen manageInventoriesScreen;
        private ManagePartnersScreen manPartnersScreen;
        private ManageSchedulesScreen manScheduleScreen;
        private ManageGoods manGoodsScreen;
        private ProcessingPage proccessingPage;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(AppService.Instance.EventAggregator);
            _mainFrame = (Frame)this.FindName("MainFrame");
            if (proccessingPage == null)
            {
                proccessingPage = new ProcessingPage();
            }
            _mainFrame.Navigate(proccessingPage);
            AppService.Instance.EventAggregator.GetEvent<ScaleExceptionEvent>().Subscribe(HandleScaleEvent);
        }



        private void HandleScaleEvent(String value)
        {
            MainWindow main = new MainWindow();
            main.Show();
            MessageBox.Show("hihi");
        }

        public void Navigate(string value)
        {
            switch (value)
            {
                case "ChartDonut":
                    if (proccessingPage == null)
                    {
                        proccessingPage = new ProcessingPage();
                    }
                    _mainFrame.Navigate(proccessingPage);
                    break;
                case "AccountMultiple":
                    if (manPartnersScreen == null)
                    {
                        manPartnersScreen = new ManagePartnersScreen();
                    }
                    _mainFrame.Navigate(manPartnersScreen);
                    break;
                case "schedule":
                    if (manScheduleScreen == null)
                    {
                        manScheduleScreen = new ManageSchedulesScreen();
                    }
                    _mainFrame.Navigate(manScheduleScreen);
                    break;
                case "PackageVariantClosed":
                    if (manageInventoriesScreen == null)
                    {
                        manageInventoriesScreen = new ManageInventoriesScreen();
                    }
                    _mainFrame.Navigate(manageInventoriesScreen);
                    break;
                case "Gift":
                    if (manGoodsScreen == null)
                    {
                        manGoodsScreen = new ManageGoods();
                    }
                    _mainFrame.Navigate(manGoodsScreen);
                    break;
                default:
                    break;
            }
        }

        private void page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            String value;
            value = (DataContext as MainViewModel).SelectedItem.Value;
            Navigate(value);
        }
        private void page_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
