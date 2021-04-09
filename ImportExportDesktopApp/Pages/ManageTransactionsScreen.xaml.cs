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
    /// Interaction logic for ManageTransactionsScreen.xaml
    /// </summary>
    public partial class ManageTransactionsScreen : Page
    {
        private ManageTransactionModel ViewModel;
        public ManageTransactionsScreen()
        {
            InitializeComponent();
            ViewModel = (DataContext as ManageTransactionModel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateTransaction insertTransaction = new CreateTransaction();
            this.NavigationService.Navigate(insertTransaction);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();

        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
