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
    class InventoryDetailHttpService : BaseHttpService
    {
        public Pagination<InventoryDetail> GetInventoryDetails(int page, int size, int inventoryId, String partnerName)
        {
            try
            {
                String url = String.Format("api/inventorydetails?Page={0}&Size={1}&InventoryId={2}&PartnerName={3}", page, size, inventoryId, partnerName);
                HttpResponseMessage response = Client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Pagination<InventoryDetail> listDetails = new Pagination<InventoryDetail>();
                    String result = response.Content.ReadAsStringAsync().Result;
                    listDetails = JsonConvert.DeserializeObject<Pagination<InventoryDetail>>(result);
                    return listDetails;
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
                return null;
            }
        }
    }
}
