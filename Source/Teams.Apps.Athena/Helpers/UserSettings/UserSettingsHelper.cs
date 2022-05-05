// <copyright file="UserSettingsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Newtonsoft.Json;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Constants;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// Provides helper methods for managing user entity.
    /// </summary>
    public class UserSettingsHelper : IUserSettingsHelper
    {
        /// <summary>
        /// The instance of user entity model repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// The instance of user COI model repository.
        /// </summary>
        private readonly ICoiRepository coiRepository;

        /// <summary>
        /// The instance of user model mapper.
        /// </summary>
        private readonly IUserSettingsMapper userSettingsMapper;

        private readonly ITeamService teamService;

        private readonly IUsersSearchService usersSearchService;

        private readonly IFilterQueryHelper filterQueryHelper;

        private readonly IUserBotConversationRepository userBotConversationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsHelper"/> class.
        /// </summary>
        /// <param name="userRepository">The instance of user repository accessors.</param>
        /// <param name="userSettingsMapper">The instance of user entity model mapper.</param>
        /// <param name="teamService">The instance of team service.</param>
        /// <param name="coiRepository">The instance of user COI model repository.</param>
        /// <param name="usersSearchService">The instance of <see cref="UsersSearchService"/> class.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="userBotConversationRepository">The instance of <see cref="UserBotConversationRepository"/> class.</param>
        public UserSettingsHelper(
            IUserRepository userRepository,
            IUserSettingsMapper userSettingsMapper,
            ITeamService teamService,
            ICoiRepository coiRepository,
            IUsersSearchService usersSearchService,
            IFilterQueryHelper filterQueryHelper,
            IUserBotConversationRepository userBotConversationRepository)
        {
            this.userRepository = userRepository;
            this.userSettingsMapper = userSettingsMapper;
            this.teamService = teamService;
            this.coiRepository = coiRepository;
            this.usersSearchService = usersSearchService;
            this.filterQueryHelper = filterQueryHelper;
            this.userBotConversationRepository = userBotConversationRepository;
        }

        /// <inheritdoc/>
        public async Task<UserSettingsViewDTO> CreateUserAsync(UserSettingsCreateDTO userSettingsCreateDTO, string userId)
        {
            userSettingsCreateDTO = userSettingsCreateDTO ?? throw new ArgumentNullException(nameof(userSettingsCreateDTO), "User details cannot be null.");

            var userCreateDetails = this.userSettingsMapper.MapForCreateModel(userSettingsCreateDTO, userId);

            userCreateDetails.UserDisplayName = userSettingsCreateDTO.LastName + ", " + userSettingsCreateDTO.FirstName + " " + userSettingsCreateDTO.MiddleName;

            await this.userRepository.CreateOrUpdateAsync(userCreateDetails);

            if (!string.IsNullOrEmpty(userSettingsCreateDTO.CommunityOfInterests))
            {
                var selectedCOIs = JsonConvert.DeserializeObject<List<JoinedCoiDTO>>(userSettingsCreateDTO.CommunityOfInterests);
                foreach (var coi in selectedCOIs)
                {
                    var coiEntity = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, coi.TableId);
                    if (coiEntity != null)
                    {
                        await this.teamService.AddMemberToTeamAsync(coiEntity.TeamId, Guid.Parse(userId));
                    }
                }
            }

            await this.usersSearchService.RunIndexerOnDemandAsync();

            return this.userSettingsMapper.MapForViewModel(userCreateDetails);
        }

        /// <inheritdoc/>
        public async Task<UserSettingsViewDTO> UpdateUserAsync(UserSettingsUpdateDTO userSettingsUpdateDTO, UserEntity userEntity)
        {
            userSettingsUpdateDTO = userSettingsUpdateDTO ?? throw new ArgumentNullException(nameof(userSettingsUpdateDTO));
            userEntity = userEntity ?? throw new ArgumentNullException(nameof(userEntity));

            var userUpdateDetails = this.userSettingsMapper.MapForUpdateModel(userSettingsUpdateDTO, userEntity);

            userUpdateDetails.UserDisplayName = userSettingsUpdateDTO.LastName + ", " + userSettingsUpdateDTO.FirstName + " " + userSettingsUpdateDTO.MiddleName;

            var existingCOIs = new List<JoinedCoiDTO>();
            var existingCOIDetails = await this.userRepository.GetAsync(UserTableMetadata.UserPartitionKey, userEntity.TableId);

            if (!string.IsNullOrEmpty(existingCOIDetails.CommunityOfInterests))
            {
                existingCOIs = JsonConvert.DeserializeObject<List<JoinedCoiDTO>>(existingCOIDetails.CommunityOfInterests);
            }

            await this.userRepository.CreateOrUpdateAsync(userUpdateDetails);

            if (!string.IsNullOrEmpty(userSettingsUpdateDTO.CommunityOfInterests))
            {
                var selectedCOIs = JsonConvert.DeserializeObject<List<JoinedCoiDTO>>(userSettingsUpdateDTO.CommunityOfInterests);

                var deletedCoi = existingCOIs.Except(selectedCOIs);
                if (deletedCoi.Any())
                {
                    foreach (var coi in deletedCoi)
                    {
                        var coiEntity = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, coi.TableId);
                        if (coiEntity != null)
                        {
                            await this.teamService.RemoveMemberFromTeamAsync(coiEntity.TeamId, Guid.Parse(userEntity.UserId));
                        }
                    }
                }

                var newCOIs = selectedCOIs.Except(existingCOIs);
                if (newCOIs.Any())
                {
                    foreach (var coi in newCOIs)
                    {
                        var coiEntity = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, coi.TableId);
                        if (coiEntity != null)
                        {
                            await this.teamService.AddMemberToTeamAsync(coiEntity.TeamId, Guid.Parse(userEntity.UserId));
                        }
                    }
                }
            }
            else if (existingCOIs.Any())
            {
                foreach (var coi in existingCOIs)
                {
                    var coiEntity = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, coi.TableId);
                    if (coiEntity != null)
                    {
                        await this.teamService.RemoveMemberFromTeamAsync(coiEntity.TeamId, Guid.Parse(userEntity.UserId));
                    }
                }
            }

            await this.usersSearchService.RunIndexerOnDemandAsync();

            return this.userSettingsMapper.MapForViewModel(userUpdateDetails);
        }

        /// <inheritdoc/>
        public async Task<UserSettingsViewDTO> GetUserByIdAsync(string userAadId)
        {
            userAadId = userAadId ?? throw new ArgumentNullException(nameof(userAadId));
            var response = await this.userRepository.GetUserDetailsByUserIdAsync(userAadId);

            if (response == null)
            {
                return null;
            }

            return this.userSettingsMapper.MapForViewModel(response);
        }

        /// <inheritdoc/>
        public async Task<UserSettingsViewDTO> GetUserDetailsByEmailAdressAsync(string emailAddress)
        {
            emailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
            var response = await this.userRepository.GetUserDetailsByEmailAddressAsync(emailAddress);

            if (response == null)
            {
                return null;
            }

            return this.userSettingsMapper.MapForViewModel(response);
        }

        /// <inheritdoc/>
        public async Task<UserEntity> GetUserItemByIdAsync(string tableId)
        {
            tableId = tableId ?? throw new ArgumentNullException(nameof(tableId));
            var response = await this.userRepository.GetAsync(UserTableMetadata.UserPartitionKey, tableId);

            if (response == null)
            {
                return null;
            }

            return response;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserSettingsViewDTO>> GetUsersByKeywordIds(IEnumerable<int> keywordIds)
        {
            var userFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(UserEntity.Keywords), keywordIds);

            var userSearchParametersDto = new SearchParametersDTO
            {
                Filter = userFilter,
            };

            var users = await this.usersSearchService.GetUsersAsync(userSearchParametersDto);
            return users.Select(x => this.userSettingsMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserSettingsViewDTO>> GetUsersAsync(SearchParametersDTO searchParametersDTO)
        {
            var users = await this.usersSearchService.GetUsersAsync(searchParametersDTO);
            return users.Select(x => this.userSettingsMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task DeleteUserSettingsAsync(string userAadId)
        {
            userAadId = userAadId ?? throw new ArgumentNullException(userAadId);

            var userIdFilter = this.filterQueryHelper.GetFilterCondition(nameof(UserEntity.UserId), new[] { userAadId });
            var sourceAthenaFilter = this.filterQueryHelper.GetFilterCondition(nameof(UserEntity.ExternalUserId), new[] { Constants.SourceAthena });
            var filter = this.filterQueryHelper.CombineFilters(userIdFilter, sourceAthenaFilter, TableOperators.And);

            var users = await this.userRepository.GetWithFilterAsync(filter);

            if (users.IsNullOrEmpty())
            {
                return;
            }

            await this.userRepository.DeleteAsync(users.First());
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserBotConversationEntity>> GetAthenaUsersAsync()
        {
            return await this.userBotConversationRepository.GetAllAsync();
        }
    }
}