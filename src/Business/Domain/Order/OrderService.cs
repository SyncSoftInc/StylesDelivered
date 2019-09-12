using Logistics;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Inventory;
using SyncSoft.StylesDelivered.Domain.Order.CreateOrder;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order
{
    public class OrderService : IOrderService
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CreateOrder  -

        public async Task<string> CreateOrderAsync(CreateOrderCommand cmd)
        {
            var tran = new CreateOrderTransaction(cmd);
            await tran.RunAsync().ConfigureAwait(false);
            if (tran.IsSuccess)
            {
                return MsgCodes.SUCCESS;
            }
            else
            {
                var err = tran.Context.Get<string>(CreateOrderTransaction.Error);
                return err.IsPresent() ? err : MsgCodes.CreateOrderFailed;
            }
        }

        #endregion
    }
}
