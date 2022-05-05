// <copyright file="UserGraphServiceHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// The helper operations related to user graph service.
    /// </summary>
    public class UserGraphServiceHelper : IUserGraphServiceHelper
    {
        /// <summary>
        /// The instance of Microsoft graph service.
        /// </summary>
        private readonly IUserService userGraphService;

        /// <summary>
        /// The instance of user graph service mapper.
        /// </summary>
        private readonly IUserGraphServiceMapper userGraphServiceMapper;

        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger<UserGraphServiceHelper> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGraphServiceHelper"/> class.
        /// </summary>
        /// <param name="userGraphService">The instance of <see cref="UserService"/>.</param>
        /// <param name="userGraphServiceMapper">The instance of <see cref="UserGraphServiceMapper"/>.</param>
        /// <param name="logger">The instance of <see cref="Logger"/>.</param>
        public UserGraphServiceHelper(
            IUserService userGraphService,
            IUserGraphServiceMapper userGraphServiceMapper,
            ILogger<UserGraphServiceHelper> logger)
        {
            this.userGraphService = userGraphService;
            this.userGraphServiceMapper = userGraphServiceMapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserDetails>> GetUsersAsync(IEnumerable<string> userAADIds)
        {
            userAADIds = userAADIds.Where(userAadId => !string.IsNullOrEmpty(userAadId));

            var users = await this.userGraphService.GetUsersAsync(userAADIds);

            var usersDetails = users.Select(user => this.userGraphServiceMapper.MapToViewModel(user)).ToList();

            foreach (var user in usersDetails)
            {
                try
                {
                    user.ProfileImage = await this.userGraphService.GetUserProfilePhotoAsync(user.Id);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred while fetching profile photo of user.", user.Id);
                }
            }

            return usersDetails;
        }
    }
}