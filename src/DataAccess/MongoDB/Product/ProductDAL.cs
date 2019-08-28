using MongoDB.Driver;
using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MongoDB.Product
{
    public class ProductDAL : IProductDAL
    {
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        private IStylesDeliveredDB DB { get; }

        #endregion
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public ProductDAL(IStylesDeliveredDB db)
        {
            DB = db;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        public Task<string> InsertItemAsync(ProductItemDTO dto)
            => DB.ProductItems.TryInsertOneAsync(dto);

        public Task<ProductItemDTO> GetProductItemAsync(string itemNo)
            => DB.ProductItems.Find(x => x.ItemNo == itemNo).FirstOrDefaultAsync();

        public Task<string> DeleteProductItemAsync(string itemNo)
            => DB.ProductItems.TryDeleteOneAsync(x => x.ItemNo == itemNo);


        public async Task<PagedList<ProductItemDTO>> GetProductItemsAsync(GetProductsQuery query)
        {
            // 搜索
            var filter = Builders<ProductItemDTO>.Filter.Empty;
            if (query.Keyword.IsPresent())
            {
                var itemNoFilter = Builders<ProductItemDTO>.Filter.Regex(x => x.ItemNo, query.Keyword);
                var keywordFilter = Builders<ProductItemDTO>.Filter.Regex(x => x.ProductName, query.Keyword);
                filter = Builders<ProductItemDTO>.Filter.Or(itemNoFilter, keywordFilter);
            }

            // 输出数据
            var fluent = DB.ProductItems.Find(filter);
            var total = await fluent.CountDocumentsAsync().ConfigureAwait(false);

            fluent = fluent.SortByDescending(x => x.CreatedOnUtc);
            if (query.PageSize > 0 && query.PageIndex > 0)
            {
                fluent = fluent
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Limit(query.PageSize);
            }

            var list = await fluent.ToListAsync().ConfigureAwait(false);

            return new PagedList<ProductItemDTO>(total, query.PageSize, list);
        }

        #endregion
    }
}
