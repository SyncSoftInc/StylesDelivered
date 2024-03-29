﻿using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Query.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Order
{
    public interface IOrderItemDAL
    {
        Task<IList<OrderItemDTO>> GetOrderItemsAsync(string orderNo);
        Task<PagedList<OrderItemDTO>> GetOrderItemsAsync(GetOrderItemsQuery query);
    }
}
