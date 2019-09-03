using SyncSoft.ECP.DTOs;
using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.Product
{
    public class ProductDAL : ECPMySqlDAL, IProductDAL
    {
        public ProductDAL(IMasterDB db) : base(db)
        {
        }

        public Task<string> DeleteProductItemAsync(string itemNo)
        {
            return base.TryExecuteAsync("DELETE FROM ProductItem WHERE ItemNo = @ItemNo", new { ItemNo = itemNo });
        }

        public Task<ProductItemDTO> GetProductItemAsync(string itemNo)
        {
            return base.QueryFirstOrDefaultAsync<ProductItemDTO>("SELECT * FROM ProductItem WHERE ItemNo = @ItemNo", new { ItemNo = itemNo });
        }

        public Task<PagedList<ProductItemDTO>> GetProductItemsAsync(GetProductsQuery query)
        {
            var where = new StringBuilder();

            if (query.Keyword.IsPresent())
            {
                where.AppendFormat(" AND (ItemNo LIKE '%{0}%' OR ProductName LIKE '%{0}%')", query.Keyword);
            }

            string orderBy = "CreatedOnUtc";

            switch (query.OrderBy.GetValueOrDefault())
            {
                case 1:
                    orderBy = "ItemNo";
                    break;
            }

            orderBy += " " + query.SortDirection;

            return base.GetPagedListAsync<ProductItemDTO>(query.PageSize, query.PageIndex, "ProductItem", "*", where.ToString(), orderBy);
        }

        public Task<string> InsertItemAsync(ProductItemDTO dto)
        {
            return base.TryExecuteAsync(@"INSERT INTO ProductItem(ItemNo, ProductName, Description, ImageUrl, InvQty, CreatedOnUtc)
VALUES(@ItemNo, @ProductName, @Description, @ImageUrl, @InvQty, @CreatedOnUtc)", dto);
        }

        public Task<string> UpdateItemAsync(ProductItemDTO dto)
        {
            return base.TryExecuteAsync(@"
UPDATE ProductItem SET 
ProductName = @ProductName
, Description = @Description
, InvQty = @InvQty
WHERE ItemNo = @ItemNo", dto);
        }

        public Task<string> UpdateItemImageAsync(ProductItemDTO dto)
        {
            return base.TryExecuteAsync(@"
UPDATE ProductItem SET 
ImageUrl = @ImageUrl
WHERE ItemNo = @ItemNo", dto);
        }
    }
}
