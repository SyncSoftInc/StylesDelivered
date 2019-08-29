using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.DTO.Common;
using SyncSoft.StylesDelivered.DTO.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.User
{
    public class UserDAL : ECPMySqlDAL, IUserDAL
    {
        public UserDAL(IMasterDB db) : base(db)
        {
        }


        public Task<string> InsertUserAsync(UserDTO user)
        {
            return base.TryExecuteAsync(@"INSERT INTO User(ID, Phone, Email, Status, Roles)
VALUES(@ID, @Phone, @Email, @Status, @Roles)", user);
        }

        public Task<UserDTO> GetUserAsync(Guid userId)
        {
            return base.QueryFirstOrDefaultAsync<UserDTO>("SELECT * FROM User WHERE ID = @UserID", new { UserID = userId });
        }







        public Task<string> InsertUserAddressAsync(AddressDTO dto)
        {
            return base.TryExecuteAsync(@"INSERT INTO Address(Hash, User_ID, Address1, Address2, City, State, ZipCode, Country)
VALUES(@Hash, @User_ID, @Address1, @Address2, @City, @State, @ZipCode, @Country)", dto);
        }

        public Task<AddressDTO> GetUserAddressAsync(Guid userId, string hash)
        {
            return base.QueryFirstOrDefaultAsync<AddressDTO>("SELECT * FROM Address WHERE User_ID = @UserID AND Hash = @Hash", new { UserID = userId, Hash = hash });
        }

        public Task<string> UpdateUserAddressAsync(string oldHash, AddressDTO dto)
        {
            return base.TryExecuteAsync(@"Update Address SET
  Hash = @Hash
, Address1 = @Address1
, Address2 = @Address2
, City = @City
, State = @State
, ZipCode = @ZipCode
, Country = @Country
WHERE Hash = @OldHash AND User_ID = @User_ID", new
            {
                dto.Hash,
                dto.Address1,
                dto.Address2,
                dto.City,
                dto.State,
                dto.ZipCode,
                dto.Country,
                OldHash = oldHash
            });
        }

        public Task<string> DeleteUserAddressAsync(AddressDTO dto)
        {
            return base.TryExecuteAsync("DELETE FROM Address WHERE Hash = @Hash AND User_ID = @User_ID", dto);
        }

        public Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId)
        {
            return base.QueryListAsync<AddressDTO>("SELECT * FROM Address WHERE User_ID = @UserID ORDER BY ZipCode ASC", new { UserID = userId });
        }
    }
}
