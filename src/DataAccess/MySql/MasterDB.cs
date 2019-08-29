using SyncSoft.App.MySql;
using SyncSoft.StylesDelivered.DataAccess;

namespace SyncSoft.StylesDelivered.MySql
{
    public class MasterDB : MySqlDatabase, IMasterDB
    {
        public MasterDB(string connStrOrName) : base(connStrOrName)
        {
        }
    }
}
