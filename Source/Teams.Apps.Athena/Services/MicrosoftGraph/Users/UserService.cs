// <copyright file="UserService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Services.MicrosoftGraph
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Microsoft.Graph;
    using Microsoft.Teams.Athena.Models;
    using Teams.Apps.Athena.Common.Extensions;

    /// <summary>
    /// Provides the operations related Microsoft Graph.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// MS Graph batch limit is 20.
        /// https://docs.microsoft.com/en-us/graph/known-issues#json-batching.
        /// </summary>
        private const int BatchSplitSize = 20;

        /// <summary>
        /// The memory cache key for storing AAD user details in memory cache.
        /// </summary>
        private const string AadUserMemoryCacheKey = "AadUser_{0}";

        private readonly IOptions<BotSettings> botOptions;

        private readonly IGraphServiceClient delegatedGraphClient;

        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="botOptions">The Bot options.</param>
        /// <param name="graphServiceClient">The instance of <see cref="GraphServiceClient"/>.</param>
        /// <param name="memoryCache">The instance of <see cref="MemoryCache"/>.</param>
        public UserService(
            IOptions<BotSettings> botOptions,
            IGraphServiceClient graphServiceClient,
            IMemoryCache memoryCache)
        {
            this.botOptions = botOptions;
            this.delegatedGraphClient = graphServiceClient ?? throw new ArgumentNullException(nameof(graphServiceClient));
            this.memoryCache = memoryCache;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetUsersAsync(IEnumerable<string> userAADIds)
        {
            var users = new List<User>();
            var userObjectIdsBatches = userAADIds.Split(BatchSplitSize);

            foreach (var userObjectIdsBatch in userObjectIdsBatches)
            {
                var batchIds = new List<string>();

                using var batchRequestContent = new BatchRequestContent();
                foreach (var userObjectId in userObjectIdsBatch)
                {
                    var userDetailsAvailableInCache = this.memoryCache.TryGetValue<User>(string.Format(CultureInfo.InvariantCulture, AadUserMemoryCacheKey, userObjectId), out User userDetails);

                    if (userDetailsAvailableInCache)
                    {
                        users.Add(userDetails);
                        continue;
                    }

                    var userRequest = this.delegatedGraphClient
                        .Users[userObjectId.ToString()]
                        .Request();

                    batchIds.Add(batchRequestContent.AddBatchRequestStep(userRequest));
                }

                if (batchIds.Count > 0)
                {
                    var response = await this.delegatedGraphClient.Batch.Request().PostAsync(batchRequestContent);

                    for (int i = 0; i < batchIds.Count; i++)
                    {
                        try
                        {
                            var user = await response.GetResponseByIdAsync<User>(batchIds[i]);

                            if (user != null)
                            {
                                this.memoryCache.Set(string.Format(CultureInfo.InvariantCulture, AadUserMemoryCacheKey, user.Id), user, TimeSpan.FromDays(this.botOptions.Value.AadUserDetailsCacheDurationInDays));
                                users.Add(user);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }

            return users;
        }

        /// <inheritdoc/>
        public async Task<string> GetUserProfilePhotoAsync(string userAADId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userAADId))
                {
                    var userProfilePhoto = await this.delegatedGraphClient
                           .Users[userAADId]
                           .Photo
                           .Content
                           .Request()
                           .GetAsync();

                    byte[] userProfilePhotoByteArray;
                    byte[] buffer = new byte[16 * 1024];

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        int read;
                        while ((read = userProfilePhoto.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            memoryStream.Write(buffer, 0, read);
                        }

                        userProfilePhotoByteArray = memoryStream.ToArray();
                    }

                    return $"data:image/jpeg;base64,{Convert.ToBase64String(userProfilePhotoByteArray)}";
                }

                return null;
            }
#pragma warning disable CA1031 // Handled general exception.
            catch
#pragma warning restore CA1031 // Handled general exception.
            {
                return null;
            }
        }
    }
}