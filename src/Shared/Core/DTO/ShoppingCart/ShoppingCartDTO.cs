using System;

namespace SyncSoft.StylesDelivered.DTO.ShoppingCart
{
    public class ShoppingCartDTO
    {
        public Guid ID { get; set; }
        public string Address1_Shipping { get; set; }
        public string Address2_Shipping { get; set; }
        public string City_Shipping { get; set; }
        public string State_Shipping { get; set; }
        public string ZipCode_Shipping { get; set; }
        public string Country_Shipping { get; set; }
        public string Phone_Shipping { get; set; }
        public string Email_Shipping { get; set; }
    }
}
