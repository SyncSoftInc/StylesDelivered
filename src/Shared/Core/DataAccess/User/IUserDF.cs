using SyncSoft.StylesDelivered.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.User
{
    public interface IUserDF
    {
        Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId);
    }
}
