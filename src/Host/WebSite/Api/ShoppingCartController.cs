using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.ShoppingCart;
using SyncSoft.StylesDelivered.DataAccess.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class ShoppingCartController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IShoppingCartDF> _lazyShoppingCartDF = ObjectContainer.LazyResolve<IShoppingCartDF>();
        private IShoppingCartDF ShoppingCartDF => _lazyShoppingCartDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Item  -

        /// <summary>
        /// 添加Item到购物车
        /// </summary>
        [HttpPost("api/shoppingcart/item")]
        public Task<string> AddItemAsync(AddItemCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 修改Item数量
        /// </summary>
        [HttpPatch("api/shoppingcart/item/qty")]
        public Task<string> RemoveItemAsync(ChangeItemQtyCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 以后再买
        /// </summary>
        [HttpPost("api/shoppingcart/item/saved")]
        public Task<string> SaveForLaterAsync(SaveForLaterCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 现在购买
        /// </summary>
        [HttpDelete("api/shoppingcart/item/saved")]
        public Task<string> ShopNowAsync(ShopNowCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 从购物车移除Item
        /// </summary>
        [HttpDelete("api/shoppingcart/item")]
        public Task<string> RemoveItemAsync(RemoveItemCommand cmd) => base.RequestAsync(cmd);

        #endregion
    }
}