using SyncSoft.App.DTOs.Setting;

namespace SyncSoft.StylesDelivered.DTO.Common
{
    public class StorageAccountDTO : SettingDTO
    {
        public string Endpoint { get; set; }
        public string Bucket { get; set; }
        public string AccessKeyID { get; set; }
        public string AccessKeySecret { get; set; }
    }
}
