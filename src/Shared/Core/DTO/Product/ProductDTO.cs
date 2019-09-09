using System;
using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.DTO.Product
{
    public class ProductDTO
    {
        public string ASIN { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ItemsJson { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public IList<ProductItemDTO> Items { get; set; }
    }
}
