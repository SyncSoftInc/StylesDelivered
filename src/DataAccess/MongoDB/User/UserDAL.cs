using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.DTO.Common;
using SyncSoft.StylesDelivered.DTO.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.MongoDB.User
{
    public class UserDAL : IUserDAL
    {
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        private IStylesDeliveredDB DB { get; }

        #endregion
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        public UserDAL(IStylesDeliveredDB db)
        {
            DB = db;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        public async Task<string> UpdateUserAddressesAsync(Guid userId, IList<AddressDTO> addresses)
        {
            return await DB.Users.TryFindOneAndUpdateAsync(x => x.ID == userId, Builders<UserDTO>.Update
                .Set(x => x.Addresses, addresses)
            ).ConfigureAwait(false);
        }

        public async Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId)
        {
            var aggregate = DB.Users.Aggregate()
                .Match("{ ID: CSUUID('" + userId + "') }")
                .Project<UserDTO>("{Addresses: 1}");

            var user = await aggregate.TryFirstOrDefault().ConfigureAwait(false);
            return user?.Addresses;
        }

        public Task<UserDTO> GetUserAsync(Guid userId)
        {
            return DB.Users.Find(x => x.ID == userId).FirstOrDefaultAsync();
        }

        public Task InsertUserAsync(UserDTO user)
        {
            return DB.Users.InsertOneAsync(user);
        }

        #endregion
    }
}
