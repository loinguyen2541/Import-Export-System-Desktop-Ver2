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

namespace ImportExportDesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for InventoryDetailWindow.xaml
    /// </summary>
    public partial class InventoryDetailWindow : Window
    {
        DetailInventoryModel model = new DetailInventoryModel();

        public InventoryDetailWindow(Inventory inventory)
        {
            InitializeComponent();
            model = (this.DataContext as DetailInventoryModel) ;
            model.Init(inventory);
        }
    }
}
