using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.HttpServices
{
    class AccountApiService
    {
        private HttpClient Client = null;
        public AccountApiService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://iesystem-api.azurewebsites.net/");
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public void SendPassword(Account account,Partner partner)
        {
            try
            {
                String url = "api/accounts/requestpassword?username="+account.Username+"&password="+account.Password+"&displayName="+partner.DisplayName+"&email="+partner.Email;
                HttpResponseMessage response = Client.GetAsync(url).Result;
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
    }
}
