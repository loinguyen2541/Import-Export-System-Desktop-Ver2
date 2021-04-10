using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.DisplayModel
{
   public class InventoryDisplay
    {
        public int InventoryId { get; set; }
        public float OpeningStock { get; set; }
        public System.DateTime RecordedDate { get; set; }
        public float TotalImport { get; set; }
        public float TotalExport { get; set; }
        public float ClosingStock { get; set; }
    }
}
