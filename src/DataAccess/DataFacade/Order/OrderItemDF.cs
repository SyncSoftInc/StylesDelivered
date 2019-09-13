using SyncSoft.App.Components;
using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Query.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.Order
{
    public class OrderItemDF : IOrderItemDF
    {
        private static readonly Lazy<IOrderItemDAL> _lazyOrderItemDAL = ObjectContainer.LazyResolve<IOrderItemDAL>();
        private IOrderItemDAL OrderItemDAL => _lazyOrderItemDAL.Value;

        public async Task<PagedList<OrderItemDTO>> GetOrderItemsAsync(GetOrderItemsQuery query)
        {
            return await OrderItemDAL.GetOrderItemsAsync(query).ConfigureAwait(false);
        }
    }
}
