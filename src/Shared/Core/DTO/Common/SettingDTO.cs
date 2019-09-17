namespace SyncSoft.StylesDelivered.DTO.Common
{
    public class SettingDTO : SyncSoft.App.DTOs.Setting.SettingDTO
    {
        public int MaxPendingOrder { get; set; } = 1;
    }
}
