using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.Domain.Order.ApproveOrder;
using SyncSoft.StylesDelivered.Domain.Order.CreateOrder;
using SyncSoft.StylesDelivered.Domain.Order.DeleteOrder;
using SyncSoft.StylesDelivered.Domain.Order.ShipOrder;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order
{
    public class OrderService : IOrderService
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IControllerFactory> _lazyControllerFactory = ObjectContainer.LazyResolve<IControllerFactory>();
        private IControllerFactory ControllerFactory => _lazyControllerFactory.Value;

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        private static readonly Lazy<IMessageDispatcher> _lazyMessageDispatcher = ObjectContainer.LazyResolve<IMessageDispatcher>();
        private IMessageDispatcher MessageDispatcher => _lazyMessageDispatcher.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CreateOrder  -

        public async Task<string> CreateOrderAsync(CreateOrderCommand cmd)
        {
            var userId = cmd.Identity.UserID();

            if (cmd.Order.Shipping_Address1.IsNull() || cmd.Order.Shipping_City.IsNull()
                || cmd.Order.Shipping_State.IsNull() || cmd.Order.Shipping_ZipCode.IsNull())
            {
                var err = $"Missing address information.";
                return err;
            }

            foreach (var item in cmd.Order.Items)
            {
                var count = await OrderDAL.CountOrderedItemsAsync(userId, item.SKU).ConfigureAwait(false);
                if (count > 0)
                {
                    var err = $"You have already applied this item.";
                    return err;
                }
            }

            var tran = new CreateOrderTransaction(cmd);
            var ctl = ControllerFactory.CreateForTcc(tran);
            var msgCode = await ctl.RunAsync().ConfigureAwait(false);
            return msgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  ApproveOrder  -

        public async Task<string> ApproveOrderAsync(ApproveOrderCommand cmd)
        {
            var tran = new ApproveOrderTransaction(cmd);
            var ctl = ControllerFactory.CreateForTcc(tran);
            var msgCode = await ctl.RunAsync().ConfigureAwait(false);

            if (msgCode.IsSuccess())
            {
                //await MessageDispatcher.PublishAsync()
            }

            return msgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  ShipOrder  -

        public async Task<string> ShipOrderAsync(ShipOrderCommand cmd)
        {
            var tran = new ShipOrderTransaction(cmd);
            var ctl = ControllerFactory.CreateForTcc(tran);
            var msgCode = await ctl.RunAsync().ConfigureAwait(false);
            return msgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  DeleteOrder  -

        public async Task<string> DeleteOrderAsync(DeleteOrderCommand cmd)
        {
            var tran = new DeleteOrderTransaction(cmd);
            var ctl = ControllerFactory.CreateForTcc(tran);
            var msgCode = await ctl.RunAsync().ConfigureAwait(false);
            return msgCode;
        }

        #endregion
    }
}
