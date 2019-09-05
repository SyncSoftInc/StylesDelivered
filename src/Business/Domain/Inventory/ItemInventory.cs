using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.Inventory;
using System;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class ItemInventory : IItemInventory
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IInventoryDAL> _lazyInventoryQueryDAL = ObjectContainer.LazyResolve<IInventoryDAL>();
        private IInventoryDAL InventoryQueryDAL => _lazyInventoryQueryDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        private readonly string _itemNo;

        #endregion
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        internal ItemInventory(string itemNo)
        {
            _itemNo = itemNo;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  IsAvailable  -

        public bool IsAvailable(int qty)
        {
            var invQty = InventoryQueryDAL.GetAvailableInventory(_itemNo);
            return invQty >= qty;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Get  -

        public int Get()
        {
            var invQty = InventoryQueryDAL.GetAvailableInventory(_itemNo);
            return invQty;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Set  -

        public string Set(int invQty)
        {
            try
            {
                InventoryQueryDAL.SetItemInventories((_itemNo, invQty));
                return MsgCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.GetRootExceptionMessage();
            }
        }

        #endregion
    }
}
