using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.DataAccess.Common;
using SyncSoft.StylesDelivered.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class CommonController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<ICommonDF> _lazyCommonDF = ObjectContainer.LazyResolve<ICommonDF>();
        private ICommonDF CommonDF => _lazyCommonDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Get States  -

        /// <summary>
        /// 获取用户基础信息
        /// </summary>
        [HttpGet("api/states/{countryCode}")]
        public Task<IList<StateDTO>> GetStatesAsync(string countryCode)
        {
            return CommonDF.GetStatesAsync(countryCode);
        }

        #endregion
    }
}