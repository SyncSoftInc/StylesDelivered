using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class InventoryService : IInventoryService
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        //private static readonly Lazy<IInventoryDAL> _lazyInventoryDAL = ObjectContainer.LazyResolve<IInventoryDAL>();
        //private IInventoryDAL InventoryDAL => _lazyInventoryDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CleanInventories  -

        public async Task<string> CleanInventoriesAsync()
        {
            try
            {
                //await InventoryDAL.CleanInventoriesAsync().ConfigureAwait(false);
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
