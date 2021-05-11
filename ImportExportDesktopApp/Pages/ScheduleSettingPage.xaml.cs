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
    /// Interaction logic for ScheduleTimeSettingPage.xaml
    /// </summary>
    public partial class ScheduleSettingPage : Page
    {
        ScheduleSettingViewModel _viewModel;
        public ScheduleSettingPage()
        {
            InitializeComponent();
            _viewModel = this.DataContext as ScheduleSettingViewModel;
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeScheduleTimeWindow changeScheduleTimeWindow = new ChangeScheduleTimeWindow();
            changeScheduleTimeWindow.ShowDialog();
            _viewModel.ResetData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeAutoResetTimeWindow changeAutoResetTimeWindow = new ChangeAutoResetTimeWindow();
            changeAutoResetTimeWindow.time.SelectedTime = DateTime.Parse(_viewModel.ResetTime);
            changeAutoResetTimeWindow.ShowDialog();
            _viewModel.ResetData();
        }
    }
}
