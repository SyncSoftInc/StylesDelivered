using SyncSoft.ECP.Enums.User;
using System;

namespace SyncSoft.StylesDelivered.DTO.User
{
    public class UserDTO
    {
        public Guid ID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public UserStatusEnum Status { get; set; }
        public UserRoleEnum Roles { get; set; }
    }
}
