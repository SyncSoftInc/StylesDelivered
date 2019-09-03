using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.ShoppingCart;
using SyncSoft.StylesDelivered.DTO.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.ShoppingCart
{
    public class ShoppingCartDAL : ECPMySqlDAL, IShoppingCartDAL
    {
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public ShoppingCartDAL(IMasterDB db) : base(db)
        {
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  GetCart  -

        public Task<ShoppingCartDTO> GetCartAsync(Guid userId)
        {
            return base.QueryFirstOrDefaultAsync<ShoppingCartDTO>("SELECT * FROM ShoppingCart WHERE ID = @ID", new { ID = userId });
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Cart  -

        public Task<string> InsertCartAsync(ShoppingCartDTO dto)
        {
            return base.TryExecuteAsync(@"INSERT INTO ShoppingCart
(ID,
Address1_Shipping,
Address2_Shipping,
City_Shipping,
State_Shipping,
ZipCode_Shipping,
Country_Shipping,
Phone_Shipping,
Email_Shipping)
VALUES
(@ID,
@Address1_Shipping,
@Address2_Shipping,
@City_Shipping,
@State_Shipping,
@ZipCode_Shipping,
@Country_Shipping,
@Phone_Shipping,
@Email_Shipping)", dto);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Item  -

        public Task<string> InsertItemAsync(ShoppingCartItemDTO dto)
        {
            return base.TryExecuteAsync(@"INSERT INTO ShoppingCartItem
(
  Cart_ID
, ItemNo
, Status
, Qty
, AddedOnUtc
)VALUES(
  @Cart_ID
, @ItemNo
, @Status
, @Qty
, @AddedOnUtc
)", dto);
        }

        public Task<ShoppingCartItemDTO> GetItemAsync(Guid cartId, string itemNo)
        {
            return base.QueryFirstOrDefaultAsync<ShoppingCartItemDTO>("SELECT * FROM ShoppingCartItem WHERE Cart_ID = @Cart_ID AND ItemNo = @ItemNo",
                new
                {
                    Cart_ID = cartId,
                    ItemNo = itemNo
                });
        }

        public Task<string> UpdateItemStatusAsync(ShoppingCartItemDTO dto)
        {
            return base.TryExecuteAsync(@"UPDATE ShoppingCartItem SET Status = @Status WHERE Cart_ID = @Cart_ID AND ItemNo = @ItemNo", dto);
        }

        public Task<string> UpdateItemQtyAsync(ShoppingCartItemDTO dto)
        {
            return base.TryExecuteAsync(@"UPDATE ShoppingCartItem SET Qty = @Qty WHERE Cart_ID = @Cart_ID AND ItemNo = @ItemNo", dto);
        }

        public Task<string> DeleteItemAsync(ShoppingCartItemDTO dto)
        {
            return base.TryExecuteAsync(@"DELETE FROM ShoppingCartItem WHERE Cart_ID = @Cart_ID AND ItemNo = @ItemNo", dto);
        }

        #endregion
    }
}
