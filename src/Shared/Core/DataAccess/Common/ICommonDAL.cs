using SyncSoft.StylesDelivered.DTO.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Common
{
    public interface ICommonDAL
    {
        Task<IList<StateDTO>> GetStatesAsync(string countryCode);
    }
}
