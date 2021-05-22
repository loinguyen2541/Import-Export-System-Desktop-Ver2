using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace ImportExportDesktopApp.Windows
{
    /// <summary>
    /// Interaction logic for ScanCardWindow.xaml
    /// </summary>
    public partial class ScanCardWindow : Window
    {
        int BaudRate = 9600;
        SerialPort port1;
        Thread thread1;
        public String CardId { get; set; }
        public ScanCardWindow()
        {
            InitializeComponent();
            thread1 = new Thread(ReadSerial);
            thread1.Start();
        }

        public void ReadSerial()
        {
            port1 = new SerialPort();
            port1.DataReceived += new SerialDataReceivedEventHandler(DataRecive1);
            if (!port1.IsOpen)
            {
                try
                {
                    port1.PortName = "COM13";
                    port1.BaudRate = BaudRate;
                    port1.Open();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
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
                    if (value.Trim().StartsWith("uid"))
                    {
                        String[] values = value.Split(':');
                        if (values.Length == 2)
                        {
                            CardId = values[1];
                            this.Hide();
                        }
                    }
                }
            }));

        }

        protected override void OnClosed(EventArgs e)
        {
            this.Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;  // cancels the window close    
            this.Hide();
        }
    }
}
