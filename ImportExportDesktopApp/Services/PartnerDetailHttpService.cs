using ImportExportDesktopApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.Services
{
    class PartnerDetailHttpService : BaseHttpService
    {
        public Partner GetPartnerById(int id)
        {
            try
            {
                String url = String.Format("api/partners/{0}", id);
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Partner partner = new Partner();
                    String result = response.Content.ReadAsStringAsync().Result;
                    partner = JsonConvert.DeserializeObject<Partner>(result);
                    return partner;
                }
                else
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Can not connect to server " + e.Message);
            }
            return null;
        }

        public ObservableCollection<IdentityCard> getCardByPartnerID(int id)
        {

            try
            {
                String url = String.Format("api/partners/{0}/cards", id);
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Partner partner = new Partner();
                    String result = response.Content.ReadAsStringAsync().Result;
                    partner = JsonConvert.DeserializeObject<Partner>(result);
                    return (ObservableCollection<IdentityCard>)partner.IdentityCards;
                }
                else
                {
                    MessageBox.Show("Class: " + this.ToString() + "Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }



        public Pagination<Transaction> GetTransactionByPartnerId(int page, int size, int partnerId)
        {
            try
            {
                String url = String.Format("api/transactions/partners/search?Page={0}&Size={1}&id={2}", page, size, partnerId);
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Pagination<Transaction> transaction = new Pagination<Transaction>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    transaction = JsonConvert.DeserializeObject<Pagination<Transaction>>(result);

                    return transaction;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }
        public List<String> getListPartnerStatus()
        {
            try
            {
                String url = String.Format("api/partners/status");
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    List<String> listPartnerTypeName = new List<string>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    listPartnerTypeName = JsonConvert.DeserializeObject<List<String>>(result);
                    return listPartnerTypeName;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch
            {
                MessageBox.Show("Can not connect to server");
            }
            return null;
        }

        public async Task<bool> UpdatePartner(Partner partner)
        {
            try
            {
                String jsonContent = JsonConvert.SerializeObject(partner);
                var data = new System.Net.Http.StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PutAsync("api/partners/" + partner.PartnerId, data).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

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
    }
}