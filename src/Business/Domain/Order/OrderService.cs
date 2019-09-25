using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.Domain.Order.ApproveOrder;
using SyncSoft.StylesDelivered.Domain.Order.CreateOrder;
using SyncSoft.StylesDelivered.Domain.Order.DeleteOrder;
using SyncSoft.StylesDelivered.Domain.Order.ShipOrder;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Event.Order;
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

        public async Task<MsgResult<string>> CreateOrderAsync(CreateOrderCommand cmd)
        {
            var userId = cmd.Identity.UserID();

            var err = CheckOrderDTO(cmd.Order);
            if (!err.IsSuccess()) return new MsgResult<string>(msgCode: err);
            // ^^^^^^^^^^

            foreach (var item in cmd.Order.Items)
            {
                var count = await OrderDAL.CountOrderedItemsAsync(userId, item.SKU).ConfigureAwait(false);
                if (count > 0)
                {
                    err = $"You have already applied this item.";
                    return new MsgResult<string>(msgCode: err);
                    // ^^^^^^^^^^
                }
            }

            var tran = new CreateOrderTransaction(cmd);
            var ctl = ControllerFactory.CreateForTcc(tran);
            var msgCode = await ctl.RunAsync().ConfigureAwait(false);
            var orderNo = tran.Result;

            return new MsgResult<string>(msgCode: msgCode, result: orderNo);
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
                // 抛出Order更改事件
                _ = MessageDispatcher.PublishAsync(new OrderApprovedEvent(cmd.OrderNo, tran.Result));
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

            if (msgCode.IsSuccess())
            {
                // 抛出Order更改事件
                _ = MessageDispatcher.PublishAsync(new OrderShippedEvent(cmd.OrderNo, tran.Result));
            }

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
        // *******************************************************************************************************************************
        #region -  Utilities  -

        private string CheckOrderDTO(OrderDTO dto)
        {
            if (dto.Shipping_Email.IsMissing()) return MsgCodes.EmailCannotBeEmpty;
            if (dto.Shipping_Phone.IsMissing()) return MsgCodes.PhoneCannotBeEmpty;
            if (dto.Shipping_Address1.IsMissing()) return MsgCodes.AddressCannotBeEmpty;
            if (dto.Shipping_City.IsMissing()) return MsgCodes.CityCannotBeEmpty;
            if (dto.Shipping_State.IsMissing()) return MsgCodes.StateCannotBeEmpty;
            if (dto.Shipping_ZipCode.IsMissing()) return MsgCodes.ZipCodeCannotBeEmpty;

            if (dto.Shipping_Email.IsNotNull() && dto.Shipping_Email.Length > 50) return MsgCodes.InvalidEmailLength;
            if (dto.Shipping_Phone.IsNotNull() && dto.Shipping_Phone.Length > 50) return MsgCodes.InvalidPhoneLength;
            if (dto.Shipping_Address1.IsNotNull() && dto.Shipping_Address1.Length > 100) return MsgCodes.InvalidAddressLength;
            if (dto.Shipping_Address2.IsNotNull() && dto.Shipping_Address2.Length > 200) return MsgCodes.InvalidAddressLength;
            if (dto.Shipping_City.IsNotNull() && dto.Shipping_City.Length > 50) return MsgCodes.InvalidCityLength;
            if (dto.Shipping_State.IsNotNull() && dto.Shipping_State.Length > 5) return MsgCodes.InvalidStateLength;
            if (dto.Shipping_ZipCode.IsNotNull() && dto.Shipping_ZipCode.Length > 15) return MsgCodes.InvalidZipCodeLength;

            return MsgCodes.SUCCESS;
        }

        #endregion
    }
}
