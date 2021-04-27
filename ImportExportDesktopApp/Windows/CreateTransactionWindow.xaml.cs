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
using System.Windows.Shapes;

namespace ImportExportDesktopApp.Windows
{
    /// <summary>
    /// Interaction logic for CreateTransactionWindow.xaml
    /// </summary>
    public partial class CreateTransactionWindow : Window
    {
        public bool IsCancel { get; set; }
        private CreateTransactionViewModel _viewModel;
        public CreateTransactionWindow(float weight)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            _viewModel = DataContext as CreateTransactionViewModel;
            _viewModel.Weight = weight + 1 + "";
            _viewModel.ExpectedWeight = weight + 1;
            _viewModel.GetPartner(true);
            IsCancel = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsCancel = true;
            this.Close();
        }
    }
}
