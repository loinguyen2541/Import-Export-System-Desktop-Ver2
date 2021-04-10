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

/**
* @author Loi Nguyen
*
* @date - 4/10/2021 5:57:39 AM 
*/

namespace ImportExportDesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for TransactionListPage.xaml
    /// </summary>
    public partial class TransactionListPage : Page
    {
        public TransactionListPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateTransactionPage createTransactionPage = new CreateTransactionPage();
            this.NavigationService.Navigate(createTransactionPage);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
