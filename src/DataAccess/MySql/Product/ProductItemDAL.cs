using Dapper;
using SyncSoft.ECP.DTOs;
using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.Product
{
    public class ProductItemDAL : ECPMySqlDAL, IProductItemDAL
    {
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public ProductItemDAL(IMasterDB db) : base(db)
        {
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        public Task<string> InsertItemAsync(ProductItemDTO dto)
        {
            return base.TryExecuteAsync(@"INSERT INTO ProductItem
(SKU, ASIN, Alias, Color, Size, Url, InvQty)
VALUES
(@SKU, @ASIN, @Alias, @Color, @Size, @Url, @InvQty)", dto);
        }

        public Task<string> UpdateItemAsync(ProductItemDTO dto)
        {
            return base.TryExecuteAsync(@"UPDATE ProductItem SET Alias = @Alias, Color = @Color, Size = @Size, Url = @Url, InvQty = @InvQty 
WHERE SKU = @SKU AND ASIN = @ASIN", dto);
        }

        public Task<string> DeleteItemAsync(string asin, string sku)
        {
            return base.TryExecuteAsync("DELETE FROM ProductItem WHERE SKU = @SKU AND ASIN = @ASIN", new { SKU = sku, ASIN = asin });
        }

        public Task<ProductItemDTO> GetItemAsync(string asin, string sku)
        {
            return base.QueryFirstOrDefaultAsync<ProductItemDTO>("SP_GetProductItem", new { SKU = sku, ASIN = asin }
            , commandType: CommandType.StoredProcedure);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  SetItemInventories  -

        public async Task<string> SetItemInventoriesAsync(IDictionary<string, long> inventories)
        {
            var parameters = inventories.Select(x =>
            {
                var para = new DynamicParameters();

                para.Add("SKU", x.Key, DbType.String);
                para.Add("InvQty", x.Value, DbType.Int64);

                return para;
            }).ToArray();

            using (var conn = await base.CreateConnectionAsync().ConfigureAwait(false))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    await conn.ExecuteAsync("UPDATE ProductItem SET InvQty = 0", transaction: tran).ConfigureAwait(false);
                    await conn.ExecuteAsync("SP_SetProductItemInventory", parameters, tran, commandType: CommandType.StoredProcedure);

                    tran.Commit();
                    return MsgCodes.SUCCESS;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return ex.GetRootExceptionMessage();
                }
            }
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  GetItems  -

        public Task<PagedList<ProductItemDTO>> GetItemsAsync(GetProductItemsQuery query)
        {
            var where = new StringBuilder();
            where.AppendFormat($" AND ASIN = '{query.ASIN}'");

            if (query.Keyword.IsPresent())
            {
                where.AppendFormat(" AND (SKU LIKE '%{0}%' OR Alias LIKE '%{0}%')", query.Keyword);
            }

            string orderBy = "SKU";

            switch (query.OrderBy.GetValueOrDefault())
            {
                case 0:
                    orderBy = "SKU";
                    break;
                case 1:
                    orderBy = "Alias";
                    break;
            }

            orderBy += " " + query.SortDirection;

            return base.GetPagedListAsync<ProductItemDTO>(query.PageSize, query.PageIndex, "ProductItem", "*", where.ToString(), orderBy);
        }

        public Task<IList<ProductItemDTO>> GetItemsAsync(string productASIN)
        {
            return base.QueryListAsync<ProductItemDTO>("SP_GetProductItems", new { ASIN = productASIN }, commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}
