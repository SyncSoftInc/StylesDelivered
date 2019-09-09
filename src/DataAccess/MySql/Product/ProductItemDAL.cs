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
            return base.QueryFirstOrDefaultAsync<ProductItemDTO>("SELECT * FROM ProductItem WHERE SKU = @SKU AND ASIN = @ASIN"
                , new { SKU = sku, ASIN = asin });
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  SetItemInventories  -

        public Task<string> SetItemInventoriesAsync(IDictionary<string, int> inventories)
        {
            var parameters = inventories.Select(x =>
            {
                var para = new DynamicParameters();

                para.Add("SKU", x.Key, DbType.String);
                para.Add("InvQty", x.Value, DbType.Int32);

                return para;
            }).ToArray();

            return base.TryExecuteAsync("SP_SetItemInventory", parameters, commandType: CommandType.StoredProcedure);
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
            return base.QueryListAsync<ProductItemDTO>("CALL SP_GetProductItems", new { ASIN = productASIN });
        }

        #endregion
    }
}
