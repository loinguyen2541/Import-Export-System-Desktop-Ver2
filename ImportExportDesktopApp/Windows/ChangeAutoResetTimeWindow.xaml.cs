using ImportExportDesktopApp.HttpServices;
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
using System.Windows.Shapes;

namespace ImportExportDesktopApp.Windows
{
    /// <summary>
    /// Interaction logic for ChangeAutoResetTimeWindow.xaml
    /// </summary>
    public partial class ChangeAutoResetTimeWindow : Window
    {
        SystemConfigApiService _systemConfigApiService;
        public ChangeAutoResetTimeWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            _systemConfigApiService = new SystemConfigApiService();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            DateTime dateTime = (DateTime)time.SelectedTime;

            try
            {
                _systemConfigApiService.PutAutoSchedue(dateTime.TimeOfDay.ToString());

                MessageBox.Show("Thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
