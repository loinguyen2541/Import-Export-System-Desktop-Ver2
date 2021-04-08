using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.Services
{
    class SystemConfigHttpService : BaseHttpService
    {
        public async Task<String> getScheduledTime()
        {
            try
            {
                String url = String.Format("https://iesystem-api.azurewebsites.net/api/systemconfigs/auto-schedule");
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    result = result.Replace("\"", "");
                    return result;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateScheduledTime(String time)
        {
            try
            {
                String url = String.Format("https://iesystem-api.azurewebsites.net/api/systemconfigs/auto-schedule?time=" + time);
                HttpResponseMessage response = await Client.PutAsync(url, null).ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {

                    return true;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
