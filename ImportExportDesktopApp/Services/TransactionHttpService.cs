using ImportExportDesktopApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.Services
{
    class TransactionHttpService: BaseHttpService
    {
        public bool PostTransaction(Transaction trans)
        {
            try
            {
                string url = "api/transactions";
                JsonContent content = JsonContent.Create(trans);
                var response = Client.PostAsync(url, content).Result;
                var temp = trans;
                //convert obj to json
                var json = JsonConvert.SerializeObject(trans);


                if (response.IsSuccessStatusCode)
                {
                    //chuyeern huowng
                    return true;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public async Task<Pagination<Transaction>> GetTransaction(int page, int size, String dateCreate, String partnerName, String transactionType)
        {
            try
            {
                String url = String.Format("api/transactions?DateCreate={0}&PartnerName={1}&TransactionType={2}&Page={3}&Size={4}", dateCreate, partnerName, isImportorExport(transactionType), page, size);
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Pagination<Transaction> transaction = new Pagination<Transaction>();
                    String result = await response.Content.ReadAsStringAsync();
                    transaction = JsonConvert.DeserializeObject<Pagination<Transaction>>(result);
                    return transaction;
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

        public List<String> GetTransactionStatus()
        {
            List<String> listStatus = null;
            try
            {
                HttpResponseMessage response = Client.GetAsync("api/transactions/states").Result;
                if (response.IsSuccessStatusCode)
                {
                    listStatus = new List<String>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    listStatus = JsonConvert.DeserializeObject<List<String>>(result);
                    return listStatus;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        public async Task<Pagination<Transaction>> GetLastProcessTransaction(int page, int size)
        {
            try
            {
                String url = String.Format("api/transactions/last?Page={0}&Size={1}", page, size);
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
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

        public Transaction GetTransactionByID(int transactionID)
        {
            try
            {
                String url = String.Format("api/transactions/{0}", transactionID);
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Transaction transaction = new Transaction();
                    String result = response.Content.ReadAsStringAsync().Result;
                    transaction = JsonConvert.DeserializeObject<Transaction>(result);
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
                MessageBox.Show(e.Message.ToString());
                MessageBox.Show("Can not connect to server");
            }
            return null;
        }

        public List<String> GetTransactionType()
        {
            List<String> listType = null;
            try
            {
                HttpResponseMessage response = Client.GetAsync("api/transactions/types").Result;
                if (response.IsSuccessStatusCode)
                {
                    listType = new List<String>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    listType = JsonConvert.DeserializeObject<List<String>>(result);
                    return listType;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Can not connect to server: " + e.Message);
            }
            return null;
        }

        public String isImportorExport(String transactionType)
        {
            if (transactionType == null)
            {
                return null;
            }
            else if (transactionType.Contains("Import"))
            {
                return "Import";
            }
            else
            {
                return "Export";
            }
        }
    }
}
