using ImportExportDesktopApp.ViewModels;
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
    /// Interaction logic for ManagePartnersScreen.xaml
    /// </summary>
    public partial class ManagePartnersScreen : Page
    {
        public ManagePartnersScreen()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (addProvider == null)
            //{
            //    addProvider = new AddProvider();
            //}
            //this.NavigationService.Navigate(addProvider);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ScheduleGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ScheduleGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void PartnerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PartnerGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
