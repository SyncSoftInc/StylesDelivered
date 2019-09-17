using Dapper;
using SyncSoft.ECP.DTOs;
using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using SyncSoft.StylesDelivered.Query.Order;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.Order
{
    public class OrderDAL : ECPMySqlDAL, IOrderDAL
    {
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public OrderDAL(IMasterDB db) : base(db)
        {
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  CURD  -

        public async Task<string> InsertAsync(OrderDTO dto)
        {
            using (var conn = await base.CreateConnectionAsync().ConfigureAwait(false))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    await conn.ExecuteAsync("SP_InsertOrder", new
                    {
                        dto.OrderNo,
                        dto.User_ID,
                        dto.User,
                        dto.Shipping_Address1,
                        dto.Shipping_Address2,
                        dto.Shipping_City,
                        dto.Shipping_State,
                        dto.Shipping_ZipCode,
                        dto.Shipping_Country,
                        dto.Status,
                        dto.CreatedOnUtc
                    }, tran, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

                    if (dto.Items.IsPresent())
                    {
                        var parameters = dto.Items.Select(x => new DynamicParameters(x)).ToArray();
                        await conn.ExecuteAsync("SP_InsertOrderItem", parameters, tran, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                    }

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

        public async Task<string> UpdateOrderStatusAsync(string orderNo, OrderStatusEnum status)
        {
            return await base.TryExecuteAsync("UPDATE `Order` SET Status = @Status WHERE OrderNo = @OrderNo", new { OrderNo = orderNo, Status = (int)status }).ConfigureAwait(false);
        }

        public async Task<string> DeleteOrderAsync(string orderNo)
        {
            using (var conn = await base.CreateConnectionAsync().ConfigureAwait(false))
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    await base.ExecuteAsync("DELETE FROM OrderItem WHERE OrderNo = @OrderNo", new { OrderNo = orderNo }).ConfigureAwait(false);
                    await base.ExecuteAsync("DELETE FROM `Order` WHERE OrderNo = @OrderNo", new { OrderNo = orderNo }).ConfigureAwait(false);

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
        #region -  GetOrder  -

        public Task<OrderDTO> GetPendingOrderAsync(Guid userId)
        {
            return base.QueryFirstOrDefaultAsync<OrderDTO>("SELECT * FROM `Order` WHERE User_ID = @UserID AND Status = @Status", new
            {
                UserID = userId,
                Status = (int)OrderStatusEnum.Pending
            });
        }

        public Task<OrderDTO> GetOrderAsync(string orderNo)
        {
            return base.QueryFirstOrDefaultAsync<OrderDTO>("SELECT * FROM `Order` WHERE OrderNo = @OrderNo", new { OrderNo = orderNo });
        }

        public Task<PagedList<OrderDTO>> GetOrdersAsync(GetOrdersQuery query)
        {
            var where = new StringBuilder();

            if (query.Keyword.IsPresent())
            {
                where.AppendFormat(" AND (OrderNo LIKE '%{0}%' OR User LIKE '%{0}%')", query.Keyword);
            }

            string orderBy = "CreatedOnUtc";

            switch (query.OrderBy.GetValueOrDefault())
            {
                case 0:
                    orderBy = "OrderNo";
                    break;
                case 1:
                    orderBy = "User";
                    break;
            }

            orderBy += " " + query.SortDirection;

            return base.GetPagedListAsync<OrderDTO>(query.PageSize, query.PageIndex, "`Order`", "*", where.ToString(), orderBy);
        }


        #endregion
    }
}
