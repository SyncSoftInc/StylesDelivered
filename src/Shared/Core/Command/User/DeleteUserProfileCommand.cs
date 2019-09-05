using SyncSoft.App.Messaging;
using System;

namespace SyncSoft.StylesDelivered.Command.User
{
    public class DeleteUserProfileCommand : RequestCommand
    {
        public Guid ID { get; set; }
    }
}
