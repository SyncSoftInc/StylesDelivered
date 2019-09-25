using OfficeOpenXml;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.Domain.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Office.Order
{
    public class EPPlusOrderExporter : IOrderExporter
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        private static readonly Lazy<IOrderItemDAL> _lazyOrderItemDAL = ObjectContainer.LazyResolve<IOrderItemDAL>();
        private IOrderItemDAL OrderItemDAL => _lazyOrderItemDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  ExportAsync  -

        public Task<byte[]> ExportAsync()
        {
            return Task.Run(async () =>
            {
                using (var stream = new MemoryStream())
                using (var excel = new ExcelPackage(stream))
                {
                    var ws = excel.Workbook.Worksheets.Add("Orders");

                    var orders = await OrderDAL.GetOrdersAsync(OrderStatusEnum.Approved).ConfigureAwait(false);
                    if (orders.IsPresent())
                    {
                        ExportOrders(ws, orders);
                    }
                    else
                    {
                        ws.Cells[1, 1].Value = "No approved orders";
                    }

                    excel.Save();
                    return stream.ToArray();
                }
            });
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Utilities  -

        private void ExportOrders(ExcelWorksheet ws, IList<OrderDTO> orders)
        {
            // titles
            ws.Cells[1, 1].Value = "ASIN";
            ws.Cells[1, 2].Value = "SKU";
            ws.Cells[1, 3].Value = "Alias";
            ws.Cells[1, 4].Value = "Size";
            ws.Cells[1, 5].Value = "Color";
            ws.Cells[1, 6].Value = "Url";
            ws.Cells[1, 7].Value = "User";
            ws.Cells[1, 8].Value = "Shipping_Email";
            ws.Cells[1, 9].Value = "Shipping_Phone";
            ws.Cells[1, 10].Value = "Shipping_Address1";
            ws.Cells[1, 11].Value = "Shipping_Address2";
            ws.Cells[1, 12].Value = "Shipping_City";
            ws.Cells[1, 13].Value = "Shipping_State";
            ws.Cells[1, 14].Value = "Shipping_ZipCode";
            ws.Cells[1, 15].Value = "Shipping_Country";
            ws.Cells[1, 16].Value = "Created Time";


            // data
            var idx = 2;
            orders.ForEach(x =>
            {
                ws.Cells[idx, 1].Value = x.ASIN;
                ws.Cells[idx, 2].Value = x.SKU;
                ws.Cells[idx, 3].Value = x.Alias;
                ws.Cells[idx, 4].Value = x.Size;
                ws.Cells[idx, 5].Value = x.Color;
                ws.Cells[idx, 6].Value = x.Url;
                ws.Cells[idx, 7].Value = x.User;
                ws.Cells[idx, 8].Value = x.Shipping_Email;
                ws.Cells[idx, 9].Value = x.Shipping_Phone;
                ws.Cells[idx, 10].Value = x.Shipping_Address1;
                ws.Cells[idx, 11].Value = x.Shipping_Address2;
                ws.Cells[idx, 12].Value = x.Shipping_City;
                ws.Cells[idx, 13].Value = x.Shipping_State;
                ws.Cells[idx, 14].Value = x.Shipping_ZipCode;
                ws.Cells[idx, 15].Value = x.Shipping_Country;
                ws.Cells[idx, 16].Value = x.CreatedOnUtc.ToLocalTime().ToString("MM'/'dd'/'yyyy hh:mm:ss tt");
                idx++;
            });

            ws.Cells.AutoFitColumns();
        }

        #endregion
    }
}
