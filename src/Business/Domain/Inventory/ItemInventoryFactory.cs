using SyncSoft.App.Collections;
using SyncSoft.App.Components;
using System;
using System.Collections.Concurrent;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class ItemInventoryFactory : IItemInventoryFactory
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<ConcurrentDictionary<string, IItemInventory>> _lazyInventories
            = new Lazy<ConcurrentDictionary<string, IItemInventory>>(() =>
        {
            var factory = ObjectContainer.Resolve<IDictionaryFactory>();
            return factory.CreateSmartDictionary<string, IItemInventory>();
        });
        private ConcurrentDictionary<string, IItemInventory> Inventories => _lazyInventories.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Create  -

        public IItemInventory Create(string itemNo)
        {
            if (itemNo.IsMissing())
            {
                throw new ArgumentNullException(nameof(itemNo));
            }

            itemNo = itemNo.Trim().ToUpper();
            return Inventories.GetOrAdd(itemNo, x => new ItemInventory(itemNo));
        }

        #endregion
    }
}
