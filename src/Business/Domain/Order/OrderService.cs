using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
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

        private static readonly Lazy<IControllerFactory> _lazyControllerFactory = ObjectContainer.LazyResolve<IControllerFactory>();
        private IControllerFactory ControllerFactory => _lazyControllerFactory.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CreateOrder  -

        public async Task<string> CreateOrderAsync(CreateOrderCommand cmd)
        {
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
