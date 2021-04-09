using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Loi Nguyen
*
* @date - 4/8/2021 8:44:44 PM 
*/

namespace ImportExportDesktopApp.DataTransfers
{
    class InventoryDetailDataTransfer
    {
        IEEntities ie;

        public InventoryDetailDataTransfer()
        {
            ie = DataContext.GetInstance().DB;
        }

        public void InsertInventoryDetailNotSaveChanges(int goodsId, int partnerId, int partnerTypeId, float weight, int inventoryId)
        {
            InventoryDetail inventoryDetail = ie.InventoryDetails
                .Where(id => id.InventoryId == inventoryId && id.PartnerId == partnerId).SingleOrDefault();
            if (inventoryDetail == null)
            {
                inventoryDetail = new InventoryDetail();
                inventoryDetail.GoodsId = goodsId;
                inventoryDetail.PartnerId = partnerId;
                inventoryDetail.Weight = weight;
                inventoryDetail.InventoryId = inventoryId;
                inventoryDetail.Type = partnerTypeId == 1 ? 1 : 0;
                ie.InventoryDetails.Add(inventoryDetail);
            }
            else
            {
                inventoryDetail.Weight += weight;
                ie.Entry(inventoryDetail).State = System.Data.Entity.EntityState.Modified;
            }
        }
    }
}
