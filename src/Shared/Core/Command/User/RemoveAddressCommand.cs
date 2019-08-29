using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.Common;

namespace SyncSoft.StylesDelivered.Command.User
{
    public class RemoveAddressCommand : RequestCommand
    {
        public AddressDTO Address { get; set; }
    }
}
