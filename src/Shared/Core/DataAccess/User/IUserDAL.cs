using SyncSoft.ECOM.DTOs;
using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Common;
using SyncSoft.StylesDelivered.Query.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.User
{
    public interface IUserDAL
    {
        Task<string> InsertUserAsync(UserDTO user);
        Task<string> UpdateUserAsync(UserDTO user);
        Task<string> UpdateUserProfileAsync(UserDTO user);
        Task<string> DeleteUserAsync(Guid id);
        Task<UserDTO> GetUserAsync(Guid userId);
        Task<PagedList<UserDTO>> GetUsersAsync(GetUsersQuery query);


        Task<string> InsertUserAddressAsync(AddressDTO dto);
        Task<string> UpdateUserAddressAsync(string oldHash, AddressDTO dto);
        Task<string> DeleteUserAddressAsync(AddressDTO dto);
        Task<AddressDTO> GetUserAddressAsync(Guid userId, string hash);
        Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId);
    }
}
