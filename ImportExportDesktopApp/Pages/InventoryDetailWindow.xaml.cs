using ImportExportDesktopApp.DisplayModel;
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
        InventoryDetailViewModel model;

        public InventoryDetailWindow(InventoryDisplay inventory)
        {
            InitializeComponent();
            model = (this.DataContext as InventoryDetailViewModel);
            model.Init(inventory);
        }

        private InventoryDisplay GetDisplay(Inventory item)
        {
            InventoryDisplay temp = new InventoryDisplay();
            temp.InventoryId = item.InventoryId;
            temp.OpeningStock = item.OpeningStock;
            temp.RecordedDate = item.RecordedDate;
            if (item.InventoryDetails.Count != 0)
            {
                float totalImport = 0, totalExport = 0;
                foreach (var detail in item.InventoryDetails)
                {
                    if (detail.Type == 0)
                    {
                        totalImport += detail.Weight;
                    }
                    if (detail.Type == 1)
                    {
                        totalExport += detail.Weight;
                    }
                }
                temp.TotalExport = totalExport;
                temp.TotalImport = totalImport;
            }
            return temp;
        }
    }
}
