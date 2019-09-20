using SyncSoft.App.Messaging;
using SyncSoft.ECOM.DTOs;

namespace SyncSoft.StylesDelivered.Command.User
{
    public class SaveAdminUserCommand : RequestCommand
    {
        public UserDTO User { get; set; }
    }
}
