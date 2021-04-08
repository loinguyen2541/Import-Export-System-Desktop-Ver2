using ImportExportDesktopApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.Services
{
    class PartnerHttpService : BaseHttpService
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
        public List<Partner> GetPartnerActive()
        {
            try
            {
                HttpResponseMessage response = Client.GetAsync("api/partners").Result;
                if (response.IsSuccessStatusCode)
                {
                    List<Partner> listPartner = new List<Partner>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    listPartner = JsonConvert.DeserializeObject<List<Partner>>(result);
                    return listPartner;
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

        public async Task<Pagination<Partner>> SearchPartner(QueryParams query)
        {

            try
            {
                if (query == null)
                {
                    query = new QueryParams(10, 1, null, null, null);
                }

                //String url = String.Format("api/partners/search?Size={0}&Page={1}&Type={2}&Name={3}", query.Size, query.Page, query.Type, query.Search);
                String url = String.Format("api/partners/search?Page={0}&Size={1}&Name={2}", query.Page, query.Size, query.Search);
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    Pagination<Partner> partners = new Pagination<Partner>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    partners = JsonConvert.DeserializeObject<Pagination<Partner>>(result);
                    return partners;
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

        public List<PartnerType> GetTransactionTypes()
        {
            try
            {

                HttpResponseMessage response = Client.GetAsync("api/partnertypes").Result;
                if (response.IsSuccessStatusCode)
                {

                    List<PartnerType> types = new List<PartnerType>();

                    PartnerType typeAll = new PartnerType();
                    typeAll.PartnerTypeId = 0;
                    typeAll.PartnerTypeName = "All";

                    types.Add(typeAll);

                    String result = response.Content.ReadAsStringAsync().Result;
                    types.AddRange(JsonConvert.DeserializeObject<List<PartnerType>>(result));

                    return types;
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

        public Pagination<Transaction> GetTransactionByPartxnerId(int page, int size, int partnerId)
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
    }
}
