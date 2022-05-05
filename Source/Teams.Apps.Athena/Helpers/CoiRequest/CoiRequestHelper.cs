// <copyright file="CoiRequestHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods associated with COI entity operations.
    /// </summary>
    public class CoiRequestHelper : ICoiRequestHelper
    {
        private readonly ICoiMapper coiMapper;

        private readonly ICoiRepository coiRepository;

        private readonly IAdaptiveCardHelper adaptiveCardHelper;

        private readonly ICoiSearchService coiSearchService;

        private readonly IFilterQueryHelper filterQueryHelper;

        private readonly IUserGraphServiceHelper userGraphServiceHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoiRequestHelper"/> class.
        /// </summary>
        /// <param name="coiMapper">The instance of <see cref="ICoiMapper"/>.</param>
        /// <param name="coiRepository">The instance of <see cref="ICoiRepository"/>.</param>
        /// <param name="adaptiveCardHelper">The instance of <see cref="IAdaptiveCardHelper"/>.</param>
        /// <param name="coiSearchService">The instance of <see cref="CoiSearchService"/>.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/>.</param>
        /// <param name="userGraphServiceHelper">The instance of <see cref="UserGraphServiceHelper"/>.</param>
        public CoiRequestHelper(
            ICoiMapper coiMapper,
            ICoiRepository coiRepository,
            IAdaptiveCardHelper adaptiveCardHelper,
            ICoiSearchService coiSearchService,
            IFilterQueryHelper filterQueryHelper,
            IUserGraphServiceHelper userGraphServiceHelper)
        {
            this.coiMapper = coiMapper;
            this.coiRepository = coiRepository;
            this.adaptiveCardHelper = adaptiveCardHelper;
            this.coiSearchService = coiSearchService;
            this.filterQueryHelper = filterQueryHelper;
            this.userGraphServiceHelper = userGraphServiceHelper;
        }

        /// <inheritdoc/>
        public async Task<CoiEntityDTO> CreateCoiRequestAsync(CoiEntityDTO coiRequestDetails, Guid userAadId, string upn, string userName)
        {
            if (coiRequestDetails == null)
            {
                throw new ArgumentNullException(nameof(coiRequestDetails));
            }

            var coiRequest = await this.GetCoiRequestAsync(coiRequestDetails.CoiName.Trim());

            // Request can not be created if active COIs with the same name already exists.
            if (coiRequest != null)
            {
                return null;
            }

            var coiRequestToCreate = this.coiMapper.MapForCreateModel(coiRequestDetails, userAadId, upn);

            var newCoiRequest = await this.coiRepository.CreateOrUpdateAsync(coiRequestToCreate);

            if (newCoiRequest != null)
            {
                try
                {
                    await this.coiSearchService.RunIndexerOnDemandAsync();
                }
                catch (Exception ex)
                {
                    // Log error
                }

                try
                {
                    await this.adaptiveCardHelper.SendNewCoiRequestCardToAdminTeamAsync(newCoiRequest, userName);
                }
                catch (Exception ex)
                {
                    // Log error
                }

                try
                {
                    await this.adaptiveCardHelper.SendCoiRequestCardToCreatorAsync(newCoiRequest);
                }
                catch (Exception ex)
                {
                    // Log error
                }
            }

            return this.coiMapper.MapForViewModel(newCoiRequest);
        }

        /// <inheritdoc/>
        public async Task<CoiEntityDTO> CreateDraftCoiRequestAsync(DraftCoiEntityDTO draftCoiRequestDetails, Guid userAadId, string upn)
        {
            if (draftCoiRequestDetails == null)
            {
                throw new ArgumentNullException(nameof(draftCoiRequestDetails));
            }

            var coiRequest = await this.GetCoiRequestAsync(draftCoiRequestDetails.CoiName.Trim());

            // Request can not be created if active COIs with the same name already exists.
            if (coiRequest != null)
            {
                return null;
            }

            var draftCoiRequestToCreate = this.coiMapper.MapForCreateDraftModel(draftCoiRequestDetails, userAadId, upn);

            var draftCoiRequest = await this.coiRepository.CreateOrUpdateAsync(draftCoiRequestToCreate);

            await this.coiSearchService.RunIndexerOnDemandAsync();

            return this.coiMapper.MapForViewModel(draftCoiRequest);
        }

        /// <inheritdoc/>
        public async Task DeleteCoiRequestsAsync(IEnumerable<string> coiRequestsIds, Guid userAadId)
        {
            if (coiRequestsIds.IsNullOrEmpty())
            {
                throw new ArgumentException("The community of interest Ids must be provided in order to delete requests.", nameof(coiRequestsIds));
            }

            var coiRequestsToBeDeleted = await this.coiRepository.GetActiveCoiRequestsAsync(coiRequestsIds, userAadId);

            // Only the requests with status 'Draft' or 'Pending' can be deleted.
            coiRequestsToBeDeleted = coiRequestsToBeDeleted
                .Where(coiRequest => coiRequest.Status == (int)CoiRequestStatus.Draft || coiRequest.Status == (int)CoiRequestStatus.Pending);

            if (!coiRequestsToBeDeleted.Any())
            {
                return;
            }

            foreach (var coiRequest in coiRequestsToBeDeleted)
            {
                coiRequest.IsDeleted = true;
                coiRequest.UpdatedOn = DateTime.UtcNow;
            }

            await this.coiRepository.BatchInsertOrMergeAsync(coiRequestsToBeDeleted);
            await this.coiSearchService.RunIndexerOnDemandAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CoiEntityDTO>> GetActiveCoiRequestsAsync(string searchText, int pageNumber, CoiSortColumn sortColumn, SortOrder sortOrder, IEnumerable<int> statusFilterValues, Guid userAadId)
        {
            var searchServiceParameters = new SearchParametersDTO
            {
                PageCount = (int)pageNumber,
                SearchString = searchText?.Trim().EscapeSpecialCharacters(),
                CoiSortColumn = sortColumn,
                SortOrder = sortOrder,
                Filter = this.filterQueryHelper.GetActiveCoiRequestsFilterCondition(statusFilterValues, userAadId),
            };

            var activeCoiRequests = await this.coiSearchService.GetCommunityOfInterestsAsync(searchServiceParameters);

            var activeCoiRequestsDTOs = activeCoiRequests
                .Select(coiRequest => this.coiMapper.MapForViewModel(coiRequest));

            return activeCoiRequestsDTOs;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CoiRequestViewDTO>> GetCoiRequestsPendingForApprovalAsync(string searchText, int pageNumber, CoiSortColumn sortColumn, SortOrder sortOrder, List<int> selectedStatusFilter)
        {
            var searchServiceParameters = new SearchParametersDTO
            {
                PageCount = (int)pageNumber,
                SearchString = searchText?.Trim().EscapeSpecialCharacters(),
                CoiSortColumn = sortColumn,
                SortOrder = sortOrder,
                Filter = this.filterQueryHelper.GetFilterCondition(nameof(CommunityOfInterestEntity.Status), selectedStatusFilter.Any() ? selectedStatusFilter : new List<int> { (int)RequestStatus.Pending, (int)RequestStatus.Approved, (int)RequestStatus.Rejected }),
            };

            var requests = await this.coiSearchService.GetCommunityOfInterestsAsync(searchServiceParameters);

            var pendingCoiRequestsDTOs = requests
                .Select(coiRequest => this.coiMapper.MapForRequestApprovalViewModel(coiRequest)).ToList();

            var details = await this.userGraphServiceHelper.GetUsersAsync(pendingCoiRequestsDTOs.Where(request => !string.IsNullOrEmpty(request.CreatedBy)).Select(request => request.CreatedBy));

            foreach (var request in pendingCoiRequestsDTOs)
            {
                if (!string.IsNullOrEmpty(request.CreatedBy))
                {
                    request.CreatedBy = details.Where(user => user.Id == request.CreatedBy)?.FirstOrDefault()?.DisplayName;
                }
            }

            return pendingCoiRequestsDTOs;
        }

        /// <inheritdoc/>
        public async Task<CoiEntityDTO> GetCoiRequestAsync(Guid coiRequestId)
        {
            var coiRequest = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, coiRequestId.ToString());

            if (coiRequest == null || coiRequest.IsDeleted)
            {
                return null;
            }

            return this.coiMapper.MapForViewModel(coiRequest);
        }

        /// <inheritdoc/>
        public async Task<CoiEntityDTO> SubmitDraftCoiRequestAsync(CoiEntityDTO draftCoiRequest, Guid userAadId, string upn, string userName)
        {
            if (draftCoiRequest == null)
            {
                throw new ArgumentNullException(nameof(draftCoiRequest));
            }

            var coiRequest = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, draftCoiRequest.TableId);

            // End-user can update the request only when the request status is 'Draft'.
            if (coiRequest == null || coiRequest.Status != (int)CoiRequestStatus.Draft)
            {
                return null;
            }

            var existingCoiRequest = await this.GetCoiRequestAsync(draftCoiRequest.CoiName.Trim());

            // Request can not be created if active COIs with the same name already exists.
            if (existingCoiRequest != null && existingCoiRequest.TableId != coiRequest.TableId)
            {
                return null;
            }

            this.coiMapper.MapForUpdateModel(draftCoiRequest, coiRequest);
            coiRequest.Status = (int)CoiRequestStatus.Pending;

            var updatedCoiRequest = await this.coiRepository.InsertOrMergeAsync(coiRequest);

            if (updatedCoiRequest != null)
            {
                await this.coiSearchService.RunIndexerOnDemandAsync();
                await this.adaptiveCardHelper.SendNewCoiRequestCardToAdminTeamAsync(updatedCoiRequest, userName);
                await this.adaptiveCardHelper.SendCoiRequestCardToCreatorAsync(updatedCoiRequest);
            }

            return this.coiMapper.MapForViewModel(updatedCoiRequest);
        }

        /// <inheritdoc/>
        public async Task<CoiEntityDTO> UpdateDraftCoiRequestAsync(DraftCoiEntityDTO draftCoiRequestDetails, Guid userAadId, string upn)
        {
            if (draftCoiRequestDetails == null)
            {
                throw new ArgumentNullException(nameof(draftCoiRequestDetails));
            }

            var coiRequest = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, draftCoiRequestDetails.TableId);

            // End-user can update the request only when the request status is 'Draft'.
            if (coiRequest == null || coiRequest.Status != (int)CoiRequestStatus.Draft)
            {
                return null;
            }

            var existingCoiRequest = await this.GetCoiRequestAsync(draftCoiRequestDetails.CoiName.Trim());

            // Request can not be created if active COIs with the same name already exists.
            if (existingCoiRequest != null && existingCoiRequest.TableId != coiRequest.TableId)
            {
                return null;
            }

            this.coiMapper.MapForUpdateModel(draftCoiRequestDetails, coiRequest);

            var updatedCoiRequest = await this.coiRepository.InsertOrMergeAsync(coiRequest);

            await this.coiSearchService.RunIndexerOnDemandAsync();

            return this.coiMapper.MapForViewModel(updatedCoiRequest);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestAsync(IEnumerable<KeywordEntity> keywords, bool fetchPublicOnly)
        {
            var keywordList = new List<string>();
            keywordList = keywords.Select(u => u.Title).ToList();

            var searchServiceParameters = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = this.filterQueryHelper.GetApprovedCoiRequestsFilterCondition(keywordList),
            };

            if (fetchPublicOnly == true)
            {
                searchServiceParameters.Filter = TableQuery.CombineFilters(searchServiceParameters.Filter, TableOperators.And, $"{nameof(CommunityOfInterestEntity.Type)} eq {(int)CoiTeamType.Public}");
            }

            var approvedCoiRequests = await this.coiSearchService.GetCommunityOfInterestsAsync(searchServiceParameters);

            var approvedCoiRequestsDTOs = approvedCoiRequests
                .Select(coiRequest => this.coiMapper.MapForViewModel(coiRequest));

            return approvedCoiRequestsDTOs;
        }

        /// <summary>
        /// Validates whether a COI request with same name already exists.
        /// </summary>
        /// <param name="name">The COI name.</param>
        /// <returns>Returns true if the COI request already exists. Else returns false.</returns>
        private async Task<CommunityOfInterestEntity> GetCoiRequestAsync(string name)
        {
            var searchServiceParameters = new SearchParametersDTO
            {
                PageCount = 0,
                Filter = this.filterQueryHelper.GetFilterCondition(nameof(CommunityOfInterestEntity.CoiName), new List<string> { name.Trim() }),
            };

            var newsArticles = await this.coiSearchService.GetCommunityOfInterestsAsync(searchServiceParameters);

            return newsArticles.FirstOrDefault();
        }
    }
}
