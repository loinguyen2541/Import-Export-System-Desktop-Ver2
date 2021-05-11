using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.HttpServices
{
    class SystemConfigApiService
    {
        private HttpClient Client = null;
        public SystemConfigApiService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://iesystem-api.azurewebsites.net/");
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public void PutAutoSchedue(String time)
        {
            try
            {
                String url = "api/systemconfigs/auto-schedule?time=" + time;
                HttpResponseMessage response = Client.PutAsync(url, null).Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch
            {
                MessageBox.Show("Can not connect to server");
            }
        }

        public String GetAutoSchedue()
        {
            try
            {
                String url = "api/systemconfigs/auto-schedule";
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
                else
                {
                    return response.Content.ReadAsStringAsync().Result.Trim().Replace("\"", "");
                }

            }
            catch
            {
                MessageBox.Show("Can not connect to server");
            }
            return null;
        }
    }
}
