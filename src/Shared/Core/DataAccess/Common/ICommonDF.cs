using SyncSoft.StylesDelivered.DTO.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Common
{
    public interface ICommonDF
    {
        Task<IList<StateDTO>> GetStatesAsync(string countryCode);
    }
}
