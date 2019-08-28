using SyncSoft.ECP.DTOs;
using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.WebSite
{
    public class DataTables<TItem>
    {
        public int Draw { get; set; }
        public long RecordsTotal { get; set; }
        public long RecordsFiltered { get; set; }
        public long TotalPage { get; }
        public IList<TItem> Data { get; } = new List<TItem>();

        public DataTables(int draw, PagedList<TItem> pagedList)
        {
            this.Draw = draw;
            this.RecordsTotal = pagedList.TotalCount;
            this.RecordsFiltered = pagedList.TotalCount;
            this.Data = pagedList.Items;
            this.TotalPage = pagedList.GetTotalPage();
        }
    }
}
