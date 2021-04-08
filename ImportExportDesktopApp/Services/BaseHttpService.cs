using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.Services
{
    class BaseHttpService
    {
        protected HttpClient Client = null;

        public BaseHttpService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://iesystem-api.azurewebsites.net/");
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
