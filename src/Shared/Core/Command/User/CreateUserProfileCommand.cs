using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.User;

namespace SyncSoft.StylesDelivered.Command.User
{
    public class CreateUserProfileCommand : RequestCommand
    {
        public UserDTO User { get; set; }
    }
}
