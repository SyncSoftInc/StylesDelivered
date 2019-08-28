using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.User
{
    public class RemoveAddressCommand : RequestCommand
    {
        public string AddressID { get; set; }
    }
}
