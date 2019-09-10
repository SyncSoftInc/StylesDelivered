using Dapper;
using SyncSoft.ECP.DTOs;
using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.Product
{
    public class ProductDAL : ECPMySqlDAL, IProductDAL
    {
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public ProductDAL(IMasterDB db) : base(db)
        {
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        public Task<string> InsertProductAsync(ProductDTO dto)
        {
            return base.TryExecuteAsync(@"INSERT INTO Product
(ASIN, ProductName, Description, ImageUrl, Status, CreatedOnUtc)
VALUES
(@ASIN, @ProductName, @Description, @ImageUrl, @Status, @CreatedOnUtc)", dto);
        }

        public Task<string> UpdateProductAsync(ProductDTO dto)
        {
            return base.TryExecuteAsync(@"UPDATE Product SET ProductName = @ProductName, Description = @Description WHERE ASIN = @ASIN", dto);
        }

        public Task<string> UpdateProductImageAsync(ProductDTO dto)
        {
            return base.TryExecuteAsync(@"UPDATE Product SET ImageUrl = @ImageUrl WHERE ASIN = @ASIN", dto);
        }

        public Task<string> UpdateProductStatusAsync(ProductDTO dto)
        {
            return base.TryExecuteAsync(@"UPDATE Product SET Status = @Status WHERE ASIN = @ASIN", dto);
        }

        public async Task<string> DeleteProductAsync(string asin)
        {
            using (var conn = await DB.CreateConnectionAsync().ConfigureAwait(false))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    await conn.ExecuteAsync("DELETE FROM ProductItem WHERE ASIN = @ASIN", new { ASIN = asin }, tran).ConfigureAwait(false);
                    await conn.ExecuteAsync("DELETE FROM Product WHERE ASIN = @ASIN", new { ASIN = asin }, tran).ConfigureAwait(false);

                    tran.Commit();
                    return MsgCodes.SUCCESS;
                }
                catch (DbException ex)
                {
                    tran?.Rollback();
                    var rs = base.HandleException<string>(null, ex, null, null);
                    return rs.MsgCode; ;
                }
                finally
                {
                    conn?.Dispose();
                }
            }
        }

        public Task<ProductDTO> GetProductAsync(string asin)
        {
            return base.QueryFirstOrDefaultAsync<ProductDTO>("SELECT * FROM Product WHERE ASIN = @ASIN", new { ASIN = asin });
        }

        public Task<PagedList<ProductDTO>> GetProductsAsync(GetProductsQuery query)
        {
            var where = new StringBuilder();

            if (query.Keyword.IsPresent())
            {
                where.AppendFormat(" AND (ASIN LIKE '%{0}%' OR ProductName LIKE '%{0}%')", query.Keyword);
            }
            if (query.Status.HasValue)
            {
                where.AppendFormat(" AND Status = {0}", (int)query.Status.Value);
            }

            string orderBy = "CreatedOnUtc";

            switch (query.OrderBy.GetValueOrDefault())
            {
                case 1:
                    orderBy = "ASIN";
                    break;
            }

            orderBy += " " + query.SortDirection;

            return base.GetPagedListAsync<ProductDTO>(query.PageSize, query.PageIndex, "Product", "*", where.ToString(), orderBy);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  UpdateItemsJson  -

        public Task<string> UpdateItemsJsonAsync(string asin, string json)
        {
            return base.TryExecuteAsync("SP_UpdateProductItemsJson", new { ASIN = asin, ItemsJson = json }, commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}
