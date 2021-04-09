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
    class InventoryHttpService : BaseHttpService
    {
        public async Task<Pagination<Inventory>> GetInventory(int page, int size, String FromDate, String ToDate)
        {
            try
            {
                String url = String.Format("api/inventories/search?Page={0}&Size={1}&FromDate={2}&ToDate={3}", page, size, FromDate, ToDate);
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    Pagination<Inventory> listInventory = new Pagination<Inventory>();
                    String result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    listInventory = JsonConvert.DeserializeObject<Pagination<Inventory>>(result);
                    foreach (var item in listInventory.Data)
                    {
                        if(item.InventoryDetails.Count != 0)
                        {
                            foreach (var itemDetail in item.InventoryDetails)
                            {
                                if(itemDetail.Type == "Import")
                                {
                                    item.TotalImport += itemDetail.Weight;
                                }
                                if (itemDetail.Type == "Export")
                                {
                                    item.TotalExport += itemDetail.Weight;
                                }
                            }
                        }
                    }
                    return listInventory;
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

        public async Task<String> GetInventoryTotal(int type, DateTime date)
        {
            try
            {
                String url;
                url = String.Format("api/inventories/total?date={0}&type={1}", date.Date, type);
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    Pagination<Inventory> listInventory = new Pagination<Inventory>();
                    String result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    result = result.Replace("\"", "");
                    return result;
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
