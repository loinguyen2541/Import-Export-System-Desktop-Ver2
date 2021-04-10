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
    /// Interaction logic for ManageSchedulesScreen.xaml
    /// </summary>
    public partial class ManageSchedulesScreen : Page
    {
        public ManageSchedulesScreen()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //llllll
            //Schedule schedule = (Schedule)ScheduleGrid.SelectedItem;
            //ScheduleDetailWindow scheduleDetailWindow = new ScheduleDetailWindow(schedule);
            //scheduleDetailWindow.ShowDialog();
        }

        private void ScheduleGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ScheduleGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //EditScheduleWindow scheduleWindow = new EditScheduleWindow();
            //scheduleWindow.ShowDialog();
            //(DataContext as ScheduleViewModel).GetAutoSchedule();
        }
    }
}
