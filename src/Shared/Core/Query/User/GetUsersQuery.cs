using SyncSoft.ECP.Queries;

namespace SyncSoft.StylesDelivered.Query.User
{
    public class GetUsersQuery : PagedQuery
    {
        public int Draw { get; set; }

        public string Keyword { get; set; }
    }
}
