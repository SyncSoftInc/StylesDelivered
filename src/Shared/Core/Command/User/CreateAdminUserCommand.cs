using SyncSoft.App.Messaging;
using SyncSoft.ECOM.DTOs;

namespace SyncSoft.StylesDelivered.Command.User
{
    public class CreateAdminUserCommand : RequestCommand
    {
        //public string Phone { get; set; }
        //public string Email { get; set; }
        //public UserStatusEnum? Status { get; set; }
        //public long? Roles { get; set; }

        public UserDTO User { get; set; }
    }
}
