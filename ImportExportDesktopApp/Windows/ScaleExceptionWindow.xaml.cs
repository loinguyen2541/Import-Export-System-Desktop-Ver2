using ImportExportDesktopApp.ScaleModels;
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

/**
* @author Loi Nguyen
*
* @date - 4/11/2021 12:01:50 AM 
*/

namespace ImportExportDesktopApp.Windows
{
    /// <summary>
    /// Interaction logic for ScaleExceptionWindow.xaml
    /// </summary>
    public partial class ScaleExceptionWindow : Window
    {
        public ScaleExceptionWindow(ScaleExeption scaleExeption)
        {
            InitializeComponent();
            (this.DataContext as ScaleExceptionViewModel).Init(scaleExeption);
            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(() =>
            //{
            //    var workingArea = System.Windows.SystemParameters.WorkArea;
            //    var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            //    var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

            //    this.Left = corner.X - this.ActualWidth - 100;
            //    this.Top = corner.Y - this.ActualHeight;
            //}));
        }
    }
}
