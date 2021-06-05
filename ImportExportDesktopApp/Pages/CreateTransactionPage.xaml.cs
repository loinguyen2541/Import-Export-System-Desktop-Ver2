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

/**
* @author Loi Nguyen
*
* @date - 4/10/2021 6:04:16 AM 
*/

namespace ImportExportDesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for CreateTransactionPage.xaml
    /// </summary>
    public partial class CreateTransactionPage : Page
    {
        public CreateTransactionPage()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void cbxPartnerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateTransactionViewModel createTransactionViewModel = DataContext as CreateTransactionViewModel;
            createTransactionViewModel.GetPartner(false);
        }
    }
}
