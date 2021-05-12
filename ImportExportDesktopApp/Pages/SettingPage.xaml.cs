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
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        private ScheduleSettingPage scheduleTimeSettingPage;
        private SystemSettingPage systemSettingPage;
        public SettingPage()
        {
            InitializeComponent();
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (scheduleTimeSettingPage == null)
            {
                scheduleTimeSettingPage = new ScheduleSettingPage();
            }
            this.NavigationService.Navigate(scheduleTimeSettingPage);
        }

        private void StackPanel_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (systemSettingPage == null)
            {
                systemSettingPage = new SystemSettingPage();
            }
            this.NavigationService.Navigate(systemSettingPage);
        }
    }
}
