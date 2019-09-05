using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.StylesDelivered.DataAccess.Inventory;
using System;
using System.Threading;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class ItemInventory : IItemInventory
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IInventoryDAL> _lazyInventoryQueryDAL = ObjectContainer.LazyResolve<IInventoryDAL>();
        private IInventoryDAL InventoryQueryDAL => _lazyInventoryQueryDAL.Value;

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<ItemInventory>();
        private ILogger Logger => _lazyLogger.Value;

        private readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

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
            _locker.EnterReadLock();
            try
            {
                var invQty = InventoryQueryDAL.GetAvailableInventory(_itemNo);
                return invQty >= qty;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Get inventory failed.");
                return false;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Get  -

        public int Get()
        {
            _locker.EnterReadLock();
            try
            {
                var invQty = InventoryQueryDAL.GetAvailableInventory(_itemNo);
                return invQty;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Get inventory failed.");
                return 0;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Set  -

        public string Set(int invQty)
        {
            _locker.EnterWriteLock();
            try
            {
                InventoryQueryDAL.SetItemInventories((_itemNo, invQty));
                return MsgCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.GetRootExceptionMessage();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        #endregion
    }
}
