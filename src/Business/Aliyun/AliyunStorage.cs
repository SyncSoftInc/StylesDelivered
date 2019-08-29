using Aliyun.OSS;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.App.Securities;
using SyncSoft.App.Settings;
using SyncSoft.StylesDelivered.DTO.Common;
using SyncSoft.StylesDelivered.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Aliyun
{
    public class AliyunStorage : IStorage
    {
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        private static SyncSoft.App.Tasks.AsyncLock _settingLocker = new SyncSoft.App.Tasks.AsyncLock();
        //private static volatile StorageAccountDTO _setting;
        private static volatile OssClient _client;

        #endregion
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<AliyunStorage>();
        private ILogger _Logger => _lazyLogger.Value;

        private static readonly Lazy<ISettingProvider> _lazySettingProvider = ObjectContainer.LazyResolve<ISettingProvider>();
        private static ISettingProvider _SettingProvider => _lazySettingProvider.Value;

        private static readonly Lazy<ICredentialProtector> _lazyCredentialProtector = ObjectContainer.LazyResolve<ICredentialProtector>();
        private ICredentialProtector _CredentialProtector => _lazyCredentialProtector.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Save  -

        public async Task<string> SaveAsync(string key, byte[] data)
        {
            //_Logger.Error("C1");

            try
            {
                //_Logger.Error("C2");
                var setting = await this.GetSettingDTOAsync().ConfigureAwait(false);
                var client = await this.GetClientAsync(setting).ConfigureAwait(false);

                //_Logger.Error("C3");
                using (var stream = data.ToStream())
                {
                    //_Logger.Error("C4");
                    client.PutObject(setting.Bucket, key, stream);
                    //_Logger.Error("C5");
                }

                //_Logger.Error("C6");
                return MsgCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                //_Logger.Error("C7");
                _Logger.Error(ex, ex.GetRootExceptionMessage());
                return MsgCodes.SaveFileToCloudFailed;
            }
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Delete  -

        public async Task<string> DeleteAsync(string key)
        {
            try
            {
                var setting = await this.GetSettingDTOAsync().ConfigureAwait(false);
                var client = await this.GetClientAsync(setting).ConfigureAwait(false);

                client.DeleteObject(setting.Bucket, key);

                return MsgCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                _Logger.Error(ex, ex.GetRootExceptionMessage());
                return MsgCodes.SaveFileToCloudFailed;
            }
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Utilities  -

        private async Task<OssClient> GetClientAsync(StorageAccountDTO setting)
        {
            if (null == _client)
            {
                using (await _settingLocker.LockAsync().ConfigureAwait(false))
                {
                    if (null == _client)
                    {
                        _CredentialProtector.TryDecrypt(setting.AccessKeyID, out string key);
                        _CredentialProtector.TryDecrypt(setting.AccessKeySecret, out string secret);
                        _client = new OssClient(setting.Endpoint, key, secret);
                    }
                }
            }

            return _client;
        }

        private Task<StorageAccountDTO> GetSettingDTOAsync() => _SettingProvider.GetSettingAsync<StorageAccountDTO>();

        #endregion
    }
}
