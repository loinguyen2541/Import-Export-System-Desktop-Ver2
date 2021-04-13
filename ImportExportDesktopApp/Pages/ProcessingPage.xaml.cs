using ImportExportDesktopApp.ScaleModels;
using ImportExportDesktopApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
* @date - 4/7/2021 6:24:39 PM 
*/

namespace ImportExportDesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for ProcessingPage.xaml
    /// </summary>
    public partial class ProcessingPage : Page
    {
        int BaudRate = 115200;
        SerialPort port1;
        Thread thread1;
        SerialPort port2;
        Thread thread2;
        ProcessingViewModel processingViewModel;

        public ProcessingPage()
        {
            InitializeComponent();
            this.DataContext = new ProcessingViewModel(AppService.Instance.EventAggregator);
            processingViewModel = DataContext as ProcessingViewModel;
            thread1 = new Thread(ReadSerial);
            thread1.Start();
            thread2 = new Thread(ReadSerial2);
            thread2.Start();
        }

        public void ReadSerial2()
        {
            port2 = new SerialPort();
            port2.DataReceived += new SerialDataReceivedEventHandler(DataRecive2);
            if (!port2.IsOpen)
            {
                try
                {
                    port2.PortName = "COM5";
                    port2.BaudRate = BaudRate;
                    port2.Open();
                }
                catch
                {

                }
            }
        }

        public void ReadSerial()
        {
            port1 = new SerialPort();
            port1.DataReceived += new SerialDataReceivedEventHandler(DataRecive1);
            if (!port1.IsOpen)
            {
                try
                {
                    port1.PortName = "COM7";
                    port1.BaudRate = BaudRate;
                    port1.Open();
                }
                catch
                {

                }
            }
        }

        public void DataRecive1(object obj, SerialDataReceivedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                String value = port1.ReadLine();
                if (value != String.Empty)
                {
                    if (value.Trim().StartsWith("@request"))
                    {
                        String[] values = value.Split('|');
                        TransactionScale transScale = JsonConvert.DeserializeObject<TransactionScale>(values[1]);
                        bool result = processingViewModel.CheckCardGate2(transScale);
                        if (result)
                        {
                            processingViewModel.UpdateTable();
                            port1.WriteLine("@response|Success");
                        }
                        else
                        {
                            port1.WriteLine("@response|Fail");
                        }
                    }
                    else if (value.Trim().StartsWith("@getOffScale"))
                    {
                        if (!processingViewModel.IsSlovingExeptionGate1)
                        {
                            processingViewModel.PartnerNameGate1 = null;
                            processingViewModel.PartnerTypeNameGate1 = null;
                            processingViewModel.WeightGate1 = "...";
                        }
                    }
                    else
                    {
                        Console.WriteLine(value);
                    }
                }
            }));

        }

        public void DataRecive2(object obj, SerialDataReceivedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                String value = port2.ReadLine();
                if (value != String.Empty)
                {
                    if (value.Trim().StartsWith("@request"))
                    {
                        String[] values = value.Split('|');
                        TransactionScale transScale = JsonConvert.DeserializeObject<TransactionScale>(values[1]);
                        bool result = processingViewModel.CheckCardGate2(transScale);
                        if (result)
                        {
                            processingViewModel.UpdateTable();
                            port2.WriteLine("@response|Success");
                        }
                        else
                        {
                            port2.WriteLine("@response|Fail");
                        }
                    }
                    else if (value.Trim().StartsWith("@getOffScale"))
                    {
                        if (!processingViewModel.IsSlovingExeptionGate2)
                        {
                            processingViewModel.PartnerNameGate2 = null;
                            processingViewModel.PartnerTypeNameGate2 = null;
                            processingViewModel.WeightGate2 = "...";
                        }
                    }
                    else
                    {
                        Console.WriteLine(value);
                    }
                }

            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TransactionListPage());
        }
    }
}
