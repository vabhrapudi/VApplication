// <copyright file="IUserRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for User Data Repository.
    /// </summary>
    public interface IUserRepository : IRepository<UserEntity>
    {
        /// <summary>
        /// Gets user details by email address.
        /// </summary>
        /// <param name="emailAddress">Email address of the user.</param>
        /// <returns>Returns user details if the user with same email address already exists.</returns>
        Task<UserEntity> GetUserDetailsByEmailAddressAsync(string emailAddress);

        /// <summary>
        /// Gets user details by user Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>Returns user details.</returns>
        Task<UserEntity> GetUserDetailsByUserIdAsync(string userId);

        /// <summary>
        /// Gets user details by external user Id.
        /// </summary>
        /// <param name="externalUserId">External User Id.</param>
        /// <returns>Returns user details.</returns>
        Task<UserEntity> GetUserDetailsByExternalUserIdAsync(int externalUserId);
    }
}