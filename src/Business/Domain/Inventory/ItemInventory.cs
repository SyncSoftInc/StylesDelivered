using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class ItemInventory : IItemInventory
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IInventoryQueryDAL> _lazyInventoryQueryDAL = ObjectContainer.LazyResolve<IInventoryQueryDAL>();
        private IInventoryQueryDAL InventoryQueryDAL => _lazyInventoryQueryDAL.Value;

        #endregion


        private readonly string _itemNo;

        internal ItemInventory(string itemNo)
        {
            _itemNo = itemNo;
        }

        public async Task<bool> IsAvailableAsync(int qty)
        {
            var invQty = await InventoryQueryDAL.GetAvailableInventoryAsync(_itemNo).ConfigureAwait(false);
            return invQty > 0;
        }
    }
}
