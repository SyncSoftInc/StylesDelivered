using SyncSoft.ECP;

namespace SyncSoft.StylesDelivered
{
    public static class MsgCodes
    {
        public const string SUCCESS = MSGCODEs.SUCCESS;
        public const string UserNotExists = nameof(UserNotExists);
        public const string AddressExists = nameof(AddressExists);
        public const string SaveFileToCloudFailed = nameof(SaveFileToCloudFailed);
        public const string DeleteFileOnCloudFailed = nameof(DeleteFileOnCloudFailed);
        public const string SecurityCheckFailed = nameof(SecurityCheckFailed);
    }
}
