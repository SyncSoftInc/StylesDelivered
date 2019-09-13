using Dapper;
using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Data;
using System.Linq;
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

        public async Task InsertAsync(OrderDTO dto)
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
                        dto.Shipping_Address1,
                        dto.Shipping_Address2,
                        dto.Shipping_City,
                        dto.Shipping_State,
                        dto.Shipping_ZipCode,
                        dto.Shipping_Country,
                        dto.Status,
                    }, tran, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

                    if (dto.Items.IsPresent())
                    {
                        var parameters = dto.Items.Select(x => new DynamicParameters(x)).ToArray();
                        await conn.ExecuteAsync("SP_InsertOrderItem", parameters, tran, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }

        public Task DeleteOrderAsync(string orderNo)
        {
            return base.ExecuteAsync("DELETE FROM Order WHERE OrderNo = @OrderNo", new { OrderNo = orderNo });
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

        #endregion
    }
}
