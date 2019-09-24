using SyncSoft.ECP.DTOs;
using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Review;
using SyncSoft.StylesDelivered.DTO.Review;
using SyncSoft.StylesDelivered.Query.Review;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.Review
{
    public class ReviewDAL : ECPMySqlDAL, IReviewDAL
    {
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public ReviewDAL(IMasterDB db) : base(db)
        {
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        public Task<string> InsertReviewAsync(ReviewDTO dto)
        {
            return base.TryExecuteAsync(@"INSERT INTO Review
(ID, OrderNo, User_ID, User, SKU, Title, Content, Status, CreatedOnUtc)
VALUES
(@ID, @OrderNo, @User_ID, @User, @SKU, @Title, @Content, @Status, @CreatedOnUtc)", dto);
        }

        public Task<string> UpdateReviewAsync(ReviewDTO dto)
        {
            return base.TryExecuteAsync(@"UPDATE Review SET Title = @Title, Content = @Content WHERE ID = @ID", dto);
        }

        public Task<string> UpdateReviewStatusAsync(ReviewDTO dto)
        {
            return base.TryExecuteAsync(@"UPDATE Review SET Status = @Status WHERE ID = @ID", new { ID = dto.ID, Status = (int)dto.Status });
        }

        public Task<string> DeleteReviewAsync(Guid id)
        {
            return base.TryExecuteAsync(@"DELETE Review WHERE ID = @ID", new { ID = id });
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  GetReview  -

        public async Task<ReviewDTO> GetReviewAsync(Guid id)
        {
            var query = await base.TryQueryFirstOrDefaultAsync<ReviewDTO>("SELECT * FROM Review WHERE ID = @ID", new { ID = id }).ConfigureAwait(false);
            return query.Result;
        }

        public async Task<int> GetOrderItemReviewAsync(string orderNo, string sku, Guid userId)
        {
            var query = await base.TryExecuteScalarAsync<int>(
@"SELECT COUNT(*) FROM Review WHERE OrderNo = @OrderNo AND SKU = @SKU AND User_ID = @User_ID", new
{
    OrderNo = orderNo,
    SKU = sku,
    User_ID = userId
}).ConfigureAwait(false);

            return query.Result;
        }

        public Task<PagedList<ReviewDTO>> GetReviewsAsync(GetReviewsQuery query)
        {
            var where = new StringBuilder();

            if (query.Keyword.IsPresent())
            {
                where.AppendFormat(" AND (OrderNo LIKE '%{0}%' OR User LIKE '%{0}%' OR SKU LIKE '%{0}%' OR Title LIKE '%{0}%')", query.Keyword);
            }

            string orderBy = "CreatedOnUtc";

            switch (query.OrderBy.GetValueOrDefault())
            {
                case 1:
                    orderBy = "OrderNo";
                    break;
                case 2:
                    orderBy = "User";
                    break;
                case 3:
                    orderBy = "SKU";
                    break;
            }

            orderBy += " " + query.SortDirection;

            return base.GetPagedListAsync<ReviewDTO>(query.PageSize, query.PageIndex, "Review", "*", where.ToString(), orderBy);
        }

        #endregion
    }
}
