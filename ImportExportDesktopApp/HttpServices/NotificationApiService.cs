using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportDesktopApp.HttpServices
{
    class NotificationApiService
    {
        private HttpClient Client = new HttpClient();

        public NotificationApiService()
        {
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool PostNotification(Notification notification)
        {
            var json = JsonConvert.SerializeObject(notification);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = Client.PostAsync("https://iesystem-api.azurewebsites.net/api/notifications", content).Result;
                Console.WriteLine(json);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
