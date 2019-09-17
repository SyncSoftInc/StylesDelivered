namespace SyncSoft.StylesDelivered.DTO.Order
{
    public class OrderItemDTO
    {
        public string OrderNo { get; set; }
        public string SKU { get; set; }
        public string ASIN { get; set; }
        public string Alias { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public int Qty { get; set; }
    }
}
