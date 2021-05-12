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
    /// Interaction logic for PartnerDetailWindow.xaml
    /// </summary>
    public partial class PartnerDetailWindow : Window
    {
        PartnerDetailViewModel _viewModel;

        public PartnerDetailWindow(int partnerId)
        {
            InitializeComponent();
            _viewModel = this.DataContext as PartnerDetailViewModel;
            _viewModel.SetInit(partnerId);
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void CardGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
