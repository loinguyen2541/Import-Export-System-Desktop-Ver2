using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.Models
{
    public class Pagination<TEntity> : BaseNotifyPropertyChanged where TEntity : class
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int TotalPage { get; set; }
        private ObservableCollection<TEntity> _data;
        public ObservableCollection<TEntity> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                NotifyPropertyChanged();
            }
        }
    }
}
