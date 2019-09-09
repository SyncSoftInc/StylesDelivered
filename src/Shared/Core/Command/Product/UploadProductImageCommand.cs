using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class UploadProductImageCommand : RequestCommand
    {
        public string asin { get; set; }
        public byte[] PictureData { get; set; }
    }
}
