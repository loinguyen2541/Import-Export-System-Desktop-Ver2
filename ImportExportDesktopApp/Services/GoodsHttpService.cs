using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.Services
{
    class GoodHttpService : BaseHttpService
    {
        public ObservableCollection<Good> GetGood()
        {

            try
            {
                String url = "api/goods";
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    ObservableCollection<Good> Good = new ObservableCollection<Good>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    Good = JsonConvert.DeserializeObject<ObservableCollection<Good>>(result);
                    return Good;
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

        public List<String> GetStatus()
        {
            try
            {

                HttpResponseMessage response = Client.GetAsync("api/Good/status").Result;
                if (response.IsSuccessStatusCode)
                {

                    List<String> status = new List<String>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    status.AddRange(JsonConvert.DeserializeObject<List<String>>(result));
                    return status;
                }
                else
                {
                    //MessageBox.Show("Get Status -> Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch
            {
                MessageBox.Show("Can not connect to server");
            }
            return null;
        }
        public Boolean UpdateGood(Good Good)
        {
            try
            {
                string url = "api/Good/" + Good.GoodsId.ToString();
                JsonContent content = JsonContent.Create(Good);
                var response = Client.PutAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Update Successfully");
                    return true;
                }
                else
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch
            {
                MessageBox.Show("Can not connect to server");
            }
            return false;
        }
        public Boolean AddGood(Good Good)
        {
            try
            {
                string url = "api/Good";
                JsonContent content = JsonContent.Create(Good);
                var response = Client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Add Successfully");
                    return true;
                }
                else
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch
            {
                MessageBox.Show("Can not connect to server");
            }
            return false;
        }
    }
}