using SyncSoft.ECP.DTOs;
using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Query.Order;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.Order
{
    public class OrderItemDAL : ECPMySqlDAL, IOrderItemDAL
    {
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public OrderItemDAL(IMasterDB db) : base(db)
        {
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  GetOrderItems  -

        public Task<PagedList<OrderItemDTO>> GetOrderItemsAsync(GetOrderItemsQuery query)
        {
            var where = new StringBuilder();

            where.AppendFormat($" AND OrderNo = '{query.OrderNo}'");
            if (query.Keyword.IsPresent())
            {
                where.AppendFormat(" AND (SKU LIKE '%{0}%' OR ASIN LIKE '%{0}%')", query.Keyword);
            }

            string orderBy = "OrderNo";

            //switch (query.OrderBy.GetValueOrDefault())
            //{
            //    case 1:
            //        orderBy = "User_ID";
            //        break;
            //}

            orderBy += " " + query.SortDirection;

            return base.GetPagedListAsync<OrderItemDTO>(query.PageSize, query.PageIndex, "OrderItem", "*", where.ToString(), orderBy);
        }

        #endregion
    }
}
