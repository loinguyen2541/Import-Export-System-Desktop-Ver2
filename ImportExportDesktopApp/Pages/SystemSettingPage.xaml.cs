using ImportExportDesktopApp.ViewModels;
using ImportExportDesktopApp.Windows;
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

namespace ImportExportDesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for SystemSettingPage.xaml
    /// </summary>
    public partial class SystemSettingPage : Page
    {
        public SystemSettingPage()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeStorageCapacityWindow changeStorageCapacityWindow = new ChangeStorageCapacityWindow();
            changeStorageCapacityWindow.ShowDialog();
            SystemSettingViewModel systemSettingViewModel = DataContext as SystemSettingViewModel;
            systemSettingViewModel.Reload();
        }
    }
}
