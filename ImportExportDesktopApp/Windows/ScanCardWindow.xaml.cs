using ImportExportDesktopApp.DataTransfers;
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

        CardDataTransfer _cardDataTransfer;

        public String CardId { get; set; }
        public ScanCardWindow()
        {
            InitializeComponent();
            _cardDataTransfer = new CardDataTransfer();
        }

        public void ReadSerial()
        {
            port1 = new SerialPort();
            port1.DataReceived += new SerialDataReceivedEventHandler(DataRecive1);
            if (!port1.IsOpen)
            {
                try
                {
                    port1.PortName = "COM14";
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
            bool isTrue = false;
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
                            CardId = values[1].Trim();
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                        }
                    }
                    else
                    {
                        isTrue = false;
                    }
                }
            }));

            if (isTrue)
            {
                if (_cardDataTransfer.IsExist(CardId.Trim()))
                {
                    CardId = "";
                    MessageBox.Show("Card is already exist!!");
                }
                CloseSerialOnExit();
                Dispatcher.Invoke((new Action(() =>
                {
                    this.Hide();
                })));
            }

        }

        private void CloseSerialOnExit()
        {

            try
            {
                if (port1 != null)
                {
                    port1.DiscardInBuffer();
                    port1.DiscardOutBuffer();
                    port1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;  // cancels the window close
            if (port1.IsOpen)
            {
                CloseSerialOnExit();
            }
            this.Hide();
        }
    }
}
