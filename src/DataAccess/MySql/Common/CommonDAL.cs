using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Common;
using SyncSoft.StylesDelivered.DTO.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.Common
{
    public class CommonDAL : ECPMySqlDAL, ICommonDAL
    {
        public CommonDAL(IMasterDB db) : base(db)
        {

        }

        public Task<IList<StateDTO>> GetStatesAsync(string countryCode)
        {
            return base.QueryListAsync<StateDTO>("SELECT * FROM State WHERE Country = @Country ORDER BY Code ASC", new
            {
                Country = countryCode
            });
        }
    }
}
