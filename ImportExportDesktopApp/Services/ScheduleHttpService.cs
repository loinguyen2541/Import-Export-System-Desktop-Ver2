using ImportExportDesktopApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.Services
{
    class ScheduleHttpService : BaseHttpService
    {
        public async Task<Pagination<Schedule>> GetSchedules(int page, int size, String dateTime, String type, String partnerName)
        {

            try
            {
                String url = String.Format("api/schedules/search?Page={0}&Size={1}&TransactionType={2}&PartnerName={3}&ScheduleDate={4}", page, size, type, partnerName, dateTime);
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    Pagination<Schedule> schedules = new Pagination<Schedule>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    schedules = JsonConvert.DeserializeObject<Pagination<Schedule>>(result);
                    return schedules;
                }
                else
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch
            {
                MessageBox.Show("Can not connect to server");
            }
            return null;
        }

        public List<String> GetTransactionTypes()
        {
            try
            {

                HttpResponseMessage response = Client.GetAsync("api/transactions/types").Result;
                if (response.IsSuccessStatusCode)
                {

                    List<String> types = new List<String>();
                    types.Add("All");
                    String result = response.Content.ReadAsStringAsync().Result;
                    types.AddRange(JsonConvert.DeserializeObject<List<String>>(result));
                    return types;
                }
                else
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch
            {
                MessageBox.Show("Can not connect to server");
            }
            return null;
        }


        public bool PostPartner(Partner partner)
        {
            try
            {
                String jsonContent = JsonConvert.SerializeObject(partner);
                Debug.WriteLine(jsonContent);
                var data = new System.Net.Http.StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = Client.PostAsync("api/partners", data).Result;
                if (response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;

                    Partner newPartner = JsonConvert.DeserializeObject<Partner>(result);
                    return true;
                }
                else
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<PartnerType> GetPartnerTypes()
        {
            try
            {
                HttpResponseMessage response = Client.GetAsync("api/partnertypes").Result;
                if (response.IsSuccessStatusCode)
                {

                    List<PartnerType> types = new List<PartnerType>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    types.AddRange(JsonConvert.DeserializeObject<List<PartnerType>>(result));
                    return types;
                }
                else
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch
            {

            }
            return null;
        }
    }
}
