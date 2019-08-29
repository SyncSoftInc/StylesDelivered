using SyncSoft.StylesDelivered.DTO.Common;
using SyncSoft.StylesDelivered.DTO.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.User
{
    public interface IUserDAL
    {
        Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId);

        Task<string> InsertUserAsync(UserDTO user);
        Task<UserDTO> GetUserAsync(Guid userId);
        Task<string> UpdateUserProfileAsync(UserDTO user);
        Task<string> UpdateUserAsync(UserDTO user);


        Task<string> InsertUserAddressAsync(AddressDTO dto);
        Task<AddressDTO> GetUserAddressAsync(Guid userId, string hash);
        Task<string> UpdateUserAddressAsync(string oldHash, AddressDTO dto);
        Task<string> DeleteUserAddressAsync(AddressDTO dto);
    }
}
