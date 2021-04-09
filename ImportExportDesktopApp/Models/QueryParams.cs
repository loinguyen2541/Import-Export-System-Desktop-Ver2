using ImportExportDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.Models
{
    public class QueryParams : BaseNotifyPropertyChanged
    {
        public QueryParams() { }
        public QueryParams(int size, int page, string date, string type, string search)
        {
            Size = size;
            Page = page;
            Date = date;
            Type = type;
            Search = search;
        }

        private int _size;
        private int _page;
        private String _date;
        private String _type;
        private String _search;

        public int Size
        {
            get { return _size; }
            set
            {
                _size = value;
                NotifyPropertyChanged();
            }
        }

        public int Page
        {
            get { return _page; }
            set
            {
                _page = value;
                NotifyPropertyChanged();
            }
        }

        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                NotifyPropertyChanged();
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged();
            }
        }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                NotifyPropertyChanged();
            }
        }

    }
}
