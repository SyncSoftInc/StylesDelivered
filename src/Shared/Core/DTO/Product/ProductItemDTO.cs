namespace SyncSoft.StylesDelivered.DTO.Product
{
    public class ProductItemDTO
    {
        public string SKU { get; set; }
        public string ASIN { get; set; }
        public string Alias { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }
        public int InvQty { get; set; }
        //public IList<CustomerDTO> Customers { get; set; }
    }
}
