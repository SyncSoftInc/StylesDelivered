using SyncSoft.App.Messaging;
using System;

namespace SyncSoft.StylesDelivered.Command.User
{
    public class DeleteAdminUserCommand : RequestCommand
    {
        public Guid ID { get; set; }
    }
}
