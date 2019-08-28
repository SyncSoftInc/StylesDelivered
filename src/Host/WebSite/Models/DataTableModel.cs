using Microsoft.AspNetCore.Mvc;

namespace SyncSoft.StylesDelivered.WebSite.Models
{
    public class DataTableModel
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        [FromQuery(Name = "search[value]")]
        public string Keyword { get; set; }
        [FromQuery(Name = "order[0][column]")]
        public int? OrderBy { get; set; }
        [FromQuery(Name = "order[0][dir]")]
        public string SortDirection { get; set; }

        public int PageIndex => Length > 0 ? this.Start / this.Length + 1 : 1;

        public int PageSize => Length;
    }
}
