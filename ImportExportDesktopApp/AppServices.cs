using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/10/2021 8:43:46 AM 
*/

namespace ImportExportDesktopApp
{
    internal sealed class AppService
    {
        private AppService() { }

        private static readonly AppService _instance = new AppService();

        internal static AppService Instance { get { return _instance; } }

        private IEventAggregator _eventAggregator;
        internal IEventAggregator EventAggregator
        {
            get
            {
                if (_eventAggregator == null)
                    _eventAggregator = new EventAggregator();

                return _eventAggregator;
            }
        }
    }
}
