using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Common;
using SyncSoft.StylesDelivered.DTO.User;
using SyncSoft.StylesDelivered.Query.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.User
{
    public interface IUserDF
    {
        Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId);
        Task<UserDTO> GetUserAsync(Guid userId);
        Task<PagedList<UserDTO>> GetUserAsync(GetUsersQuery query);
    }
}
