using SyncSoft.App.Components;
using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Query.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.Order
{
    public class OrderDF : IOrderDF
    {
        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        public async Task<OrderDTO> GetOrderAsync(string orderNo)
        {
            return await OrderDAL.GetOrderAsync(orderNo).ConfigureAwait(false);
        }

        public async Task<PagedList<OrderDTO>> GetOrdersAsync(GetOrdersQuery query)
        {
            return await OrderDAL.GetOrdersAsync(query).ConfigureAwait(false);
        }
    }
}
