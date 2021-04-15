using ImportExportDesktopApp.DataTransfers;
using ImportExportDesktopApp.Enums;
using ImportExportDesktopApp.ScaleModels;
using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
* @date - 4/13/2021 12:31:34 AM 
*/

namespace ImportExportDesktopApp.Windows
{
    /// <summary>
    /// Interaction logic for EditExceptionTransactionWindow.xaml
    /// </summary>
    public partial class EditExceptionTransactionWindow : Window, INotifyPropertyChanged
    {
        public Transaction _transaction { get; set; }
        public TransactionScale _transactionScale { get; set; }
        private float _total;
        private String _transactionType;
        public bool IsCancel { get; set; }
        private Partner _selectedPartner;
        private ObservableCollection<Partner> _partners;
        private PartnerDataTransfer _partnerDataTransfer { get; set; }
        public EditExceptionTransactionWindow(Transaction transaction, TransactionScale transactionScale)
        {
            InitializeComponent();
            IsCancel = true;
            Transaction = transaction;
            TransactionScale = transactionScale;
            if (Transaction != null && TransactionScale != null)
            {
                Total = Transaction.WeightIn - TransactionScale.Weight;
                if (Total < 0)
                {
                    TransactionType = ETransactionType.Export.ToString();
                    Total = Total * -1;
                }
                else
                {
                    TransactionType = ETransactionType.Import.ToString();
                }
            }
            _partnerDataTransfer = new PartnerDataTransfer();
            int partnerTypeId = (Transaction.WeightIn - TransactionScale.Weight) > 0 ? 2 : 1;
            Partners = _partnerDataTransfer.GetAllByType(partnerTypeId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsCancel = true;
            this.Close();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Partner> Partners
        {
            get { return _partners; }
            set
            {
                _partners = value;
                NotifyPropertyChanged();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectedPartner != null)
            {
                IsCancel = false;
                this.Close();
            }
        }

        public Partner SelectedPartner
        {
            get { return _selectedPartner; }
            set
            {
                _selectedPartner = value;
                NotifyPropertyChanged();
            }
        }

        public Transaction Transaction
        {
            get { return _transaction; }
            set
            {
                _transaction = value;
                NotifyPropertyChanged();
            }
        }
        public TransactionScale TransactionScale
        {
            get { return _transactionScale; }
            set
            {
                _transactionScale = value;
                NotifyPropertyChanged();
            }
        }
        public float Total
        {
            get { return _total; }
            set
            {
                _total = value;
                NotifyPropertyChanged();
            }
        }

        public String TransactionType
        {
            get { return _transactionType; }
            set
            {
                _transactionType = value;
                NotifyPropertyChanged();
            }
        }
    }
}
