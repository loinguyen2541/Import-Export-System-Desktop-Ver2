using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.ViewModels
{
    class AddCardViewModel : BaseNotifyPropertyChanged
    {
        public int PartnerId { get; set; }
        public ObservableCollection<IdentityCard> _identityCards;

        public AddCardViewModel()
        {
            _identityCards = new ObservableCollection<IdentityCard>();
        }

        public ObservableCollection<IdentityCard> IdentityCards
        {
            get { return _identityCards; }
            set
            {
                _identityCards = value;
                NotifyPropertyChanged();
            }
        }
    }
}
