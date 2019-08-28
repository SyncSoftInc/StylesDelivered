using SyncSoft.StylesDelivered.DTO.Common;
using System;
using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.DTO.User
{
    public class UserDTO
    {
        public Guid ID { get; set; }
        public string Phone { get; set; }
        public IList<AddressDTO> Addresses { get; set; } = new List<AddressDTO>();
    }
}
