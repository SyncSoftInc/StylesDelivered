using SyncSoft.StylesDelivered.DTO.Customer;
using System;
using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.DTO.Product
{
    public class ProductItemDTO
    {
        public string ItemNo { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int InvQty { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public IList<CustomerDTO> Customers { get; set; }
    }
}
