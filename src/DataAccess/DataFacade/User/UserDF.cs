﻿using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.DTO.Common;
using SyncSoft.StylesDelivered.DTO.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.User
{
    public class UserDF : IUserDF
    {
        private static readonly Lazy<IUserDAL> _lazyUserDAL = ObjectContainer.LazyResolve<IUserDAL>();
        private IUserDAL UserDAL => _lazyUserDAL.Value;

        public Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId)
            => UserDAL.GetUserAddressesAsync(userId);

        public Task<UserDTO> GetUserAsync(Guid userId)
        {
            return UserDAL.GetUserAsync(userId);
        }
    }
}
