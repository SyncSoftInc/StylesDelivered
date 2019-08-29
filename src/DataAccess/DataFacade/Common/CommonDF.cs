using SyncSoft.App.Caching;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.Common;
using SyncSoft.StylesDelivered.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.Common
{
    public class CommonDF : ICommonDF
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IInMemoryCacheProvider> _lazyInMemoryCacheProvider = ObjectContainer.LazyResolve<IInMemoryCacheProvider>();
        private IInMemoryCacheProvider InMemoryCacheProvider => _lazyInMemoryCacheProvider.Value;

        private static readonly Lazy<ICommonDAL> _lazyCommonDAL = ObjectContainer.LazyResolve<ICommonDAL>();
        private ICommonDAL CommonDAL => _lazyCommonDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        private const string StatesKey = "STATES_CACHE";

        #endregion
        // *******************************************************************************************************************************
        #region -  GetStates  -

        public Task<IList<StateDTO>> GetStatesAsync(string countryCode)
        {
            var key = (StatesKey + countryCode).ToUpper();

            return InMemoryCacheProvider.GetOrSetAsync(key, async () =>
            {
                var states = await CommonDAL.GetStatesAsync(countryCode).ConfigureAwait(false);
                return new GetValueFunctionResult<IList<StateDTO>>(states); // 州不经常变动，可不设置过期时间
            });
        }

        #endregion
    }
}
