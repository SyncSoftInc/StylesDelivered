using SyncSoft.ECOM.DTOs;
using SyncSoft.ECP.DTOs;
using SyncSoft.ECP.MySql;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.DTO.Common;
using SyncSoft.StylesDelivered.Query.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MySql.User
{
    public class UserDAL : ECPMySqlDAL, IUserDAL
    {
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public UserDAL(IMasterDB db) : base(db)
        {
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  User  -

        public Task<string> InsertUserAsync(UserDTO user)
        {
            return base.TryExecuteAsync(@"INSERT INTO User(ID, Username, Phone, Email, Status, Roles)
VALUES(@ID, @Username, @Phone, @Email, @Status, @Roles)", user);
        }

        public Task<string> UpdateUserAsync(UserDTO user)
        {
            return base.TryExecuteAsync(@"UPDATE User 
SET Email = @Email, Username = @Username, Phone = @Phone, Status = @Status, Roles = @Roles 
WHERE ID = @ID", user);
        }

        public Task<string> UpdateUserProfileAsync(UserDTO user)
        {
            return base.TryExecuteAsync("UPDATE User SET Email = @Email, Phone = @Phone WHERE ID = @ID", user);
        }

        public Task<string> DeleteUserAsync(Guid id)
        {
            return base.TryExecuteAsync("DELETE FROM User WHERE ID = @ID", new { ID = id });
        }

        public Task<UserDTO> GetUserAsync(Guid userId)
        {
            return base.QueryFirstOrDefaultAsync<UserDTO>("SELECT * FROM User WHERE ID = @UserID", new { UserID = userId });
        }

        public Task<PagedList<UserDTO>> GetUsersAsync(GetUsersQuery query)
        {
            var where = new StringBuilder();

            if (query.Keyword.IsPresent())
            {
                where.AppendFormat(" AND (Username LIKE '%{0}%' OR Email LIKE '%{0}%')", query.Keyword);
            }

            string orderBy = "ID";

            switch (query.OrderBy.GetValueOrDefault())
            {
                case 1:
                    orderBy = "Username";
                    break;
                case 2:
                    orderBy = "Email";
                    break;
            }

            orderBy += " " + query.SortDirection;

            return base.GetPagedListAsync<UserDTO>(query.PageSize, query.PageIndex, "User", "*", where.ToString(), orderBy);
        }


        #endregion
        // *******************************************************************************************************************************
        #region -  User Address  -

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

        #endregion
    }
}
