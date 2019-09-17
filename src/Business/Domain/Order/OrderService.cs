using Logistics;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Inventory;
using SyncSoft.StylesDelivered.Domain.Order.ApproveOrder;
using SyncSoft.StylesDelivered.Domain.Order.CreateOrder;
using SyncSoft.StylesDelivered.Domain.Order.DeleteOrder;
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
        // *******************************************************************************************************************************
        #region -  ApproveOrder  -

        public async Task<string> ApproveOrderAsync(ApproveOrderCommand cmd)
        {
            var tran = new ApproveOrderTransaction(cmd);
            await tran.RunAsync().ConfigureAwait(false);
            if (tran.IsSuccess)
            {
                return MsgCodes.SUCCESS;
            }
            else
            {
                var err = tran.Context.Get<string>(ApproveOrderTransaction.Error);
                return err.IsPresent() ? err : MsgCodes.ApproveOrderFailed;
            }
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  DeleteOrder  -

        public async Task<string> DeleteOrderAsync(DeleteOrderCommand cmd)
        {
            var tran = new DeleteOrderTransaction(cmd);
            await tran.RunAsync().ConfigureAwait(false);
            if (tran.IsSuccess)
            {
                return MsgCodes.SUCCESS;
            }
            else
            {
                var err = tran.Context.Get<string>(DeleteOrderTransaction.Error);
                return err.IsPresent() ? err : MsgCodes.DeleteOrderFailed;
            }
        }

        #endregion
    }
}
