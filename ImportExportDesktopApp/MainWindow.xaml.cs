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
        private ManageInventoriesScreen manageInventoriesScreen;
        private ManagePartnersScreen manPartnersScreen;
        private ManageSchedulesScreen manScheduleScreen;
        public MainWindow()
        {
            InitializeComponent();
            _mainFrame = (Frame)this.FindName("MainFrame");
            //if (processingScreen == null)
            //{
            //    processingScreen = new ProcessingScreen();
            //}
            _mainFrame.Navigate(new ManageTransactionsScreen());
        }

        public void Navigate(string value)
        {
            switch (value)
            {
                case "ChartDonut":
                    if (manPartnersScreen == null)
                    {
                        manPartnersScreen = new ManagePartnersScreen();
                    }
                    _mainFrame.Navigate(manPartnersScreen);
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
                    if (manPartnersScreen == null)
                    {
                        manPartnersScreen = new ManagePartnersScreen();
                    }
                    _mainFrame.Navigate(manPartnersScreen);
                    break;
                default:
                    break;
            }
        }

        private void page_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            String value;
            value = (DataContext as MainViewModel).SelectedItem.Value;
            Navigate(value);
        }
        private void page_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
