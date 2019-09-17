using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.DTO.Order
{
    public partial class OrderDTO
    {
        public string OrderNo { get; set; }
        public Guid User_ID { get; set; }
        public string User { get; set; }
        public string Shipping_Address1 { get; set; }
        public string Shipping_Address2 { get; set; }
        public string Shipping_City { get; set; }
        public string Shipping_State { get; set; }
        public string Shipping_ZipCode { get; set; }
        public string Shipping_Country { get; set; }
        public OrderStatusEnum Status { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public IList<OrderItemDTO> Items { get; set; }
    }
}
