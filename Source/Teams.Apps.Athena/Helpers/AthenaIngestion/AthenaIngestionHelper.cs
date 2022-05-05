// <copyright file="AthenaIngestionHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    /*using System.Linq;*/
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Mappers;
    /*using Teams.Apps.Athena.Models;*/

    /// <summary>
    /// The helper methods related to Athena ingestion.
    /// </summary>
    public class AthenaIngestionHelper : IAthenaIngestionHelper
    {
        private readonly IUserRepository userRepository;
        private readonly IAthenaInfoResourcesRepository athenaInfoResourcesRepository;
        private readonly ICoiRepository coiRepository;
        private readonly IUserBlobRepository userBlobRepository;
        private readonly IAthenaInfoResourceBlobRepository athenaInfoResourceBlobRepository;
        private readonly ICommunityOfInterestBlobRepository communityOfInterestBlobRepository;
        private readonly IAthenaResearchProjectBlobRepository athenaResearchProjectBlobRepository;
        private readonly IAthenaResearchProposalsBlobRepository athenaResearchProposalBlobRepository;
        private readonly IResearchProposalsRepository researchProposalRepository;
        private readonly IResearchProposalsSearchService researchProposalsSearchService;
        private readonly ISponsorBlobRepository sponsorBlobRepository;
        private readonly ISponsorRepository sponsorRepository;
        private readonly ISponsorsSearchService sponsorSearchService;
        private readonly IPartnerBlobRepository partnerBlobRepository;
        private readonly IPartnersRepository partnersRepository;
        private readonly IPartnersSearchService partnersSearchService;
        private readonly IResearchRequestBlobRepository researchRequestBlobRepository;
        private readonly IResearchRequestsRepository researchRequestRepository;
        private readonly IResearchRequestsSearchService researchRequestSearchService;
        private readonly IAthenaEventsBlobRepository athenaEventsBlobRepository;
        private readonly IEventsRepository eventsRepository;
        private readonly IAthenaEventsSearchService eventsSearchService;
        private readonly IAthenaToolsBlobRepository athenaToolsBlobRepository;
        private readonly IAthenaToolsRepository athenaToolsRepository;
        private readonly ICoiSearchService coiSearchService;
        private readonly IAthenaInfoResourcesSearchService athenaInfoResourcesSearchService;
        private readonly IResearchProjectsSearchService researchProjectsSearchService;
        private readonly IAthenaIngestionRepository athenaIngestionRepository;

        private readonly IAthenaFileNamesBlobRepository athenaFileNamesBlobRepository;

        private readonly IAthenaIngestionMapper athenaIngestionMapper;
        private readonly IResearchProjectsRepository researchProjectsRepository;
        private readonly IUsersSearchService usersSearchService;

        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger<AthenaIngestionHelper> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaIngestionHelper"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/> class.</param>
        /// <param name="userRepository">The instance of <see cref="UserRepository"/> class.</param>
        /// <param name="coiRepository">The instance of <see cref="CoiRepository"/> class.</param>
        /// <param name="athenaInfoResourcesRepository">The instance of <see cref="athenaInfoResourcesRepository"/> class.</param>
        /// <param name="athenaIngestionMapper">The instance of <see cref="athenaIngestionMapper"/> class.</param>
        /// <param name="userBlobRepository">The instance of <see cref="userBlobRepository"/> class.</param>
        /// <param name="communityOfInterestBlobRepository">The instance of <see cref="communityOfInterestBlobRepository"/> class.</param>
        /// <param name="athenaInfoResourceBlobRepository">The instance of <see cref="athenaInfoResourceBlobRepository"/> class.</param>
        /// <param name="athenaResearchProjectBlobRepository">The instance of <see cref="athenaResearchProjectBlobRepository"/> class.</param>
        /// <param name="researchProjectsRepository">The instance of <see cref="researchProjectsRepository"/> class.</param>
        /// <param name="usersSearchService">The instance of <see cref="usersSearchService"/> class.</param>
        /// <param name="athenaIngestionRepository">The instance of <see cref="AthenaIngestionRepository"/> class.</param>
        /// <param name="athenaFileNamesBlobRepository">The instance of <see cref="AthenaFileNamesBlobRepository"/> class.</param>
        /// <param name="athenaIngestionMapper">The instance of <see cref="AthenaIngestionMapper"/> class.</param>
        /// <param name="athenaResearchProposalBlobRepository">The instance of <see cref="athenaResearchProposalBlobRepository"/> class.</param>
        /// <param name="researchProposalRepository">The instance of <see cref="researchProposalRepository"/> class.</param>
        /// <param name="researchProposalsSearchService">The instance of <see cref="researchProposalsSearchService"/> class.</param>
        /// <param name="sponsorBlobRepository">The instance of <see cref="sponsorBlobRepository"/> class.</param>
        /// <param name="sponsorRepository">The instance of <see cref="sponsorRepository"/> class.</param>
        /// <param name="sponsorSearchService">The instance of <see cref="sponsorSearchService"/> class.</param>
        /// <param name="partnerBlobRepository">The instance of <see cref="partnerBlobRepository"/> class.</param>
        /// <param name="partnersRepository">The instance of <see cref="partnersRepository"/> class.</param>
        /// <param name="partnersSearchService">The instance of <see cref="partnersSearchService"/> class.</param>
        /// <param name="researchRequestSearchService">The instance of <see cref="researchRequestSearchService"/> class.</param>
        /// <param name="researchRequestRepository">The instance of <see cref="researchRequestRepository"/> class.</param>
        /// <param name="researchRequestBlobRepository">The instance of <see cref="researchRequestBlobRepository"/> class.</param>
        /// <param name="athenaEventsBlobRepository">The instance of <see cref="AthenaBotActivityHandlerHelper"/> class.</param>
        /// <param name="eventsRepository">The instance of <see cref="eventsRepository"/> class.</param>
        /// <param name="eventsSearchService">The instance of <see cref="eventsSearchService"/> class.</param>
        /// <param name="athenaToolsRepository">The instance of <see cref="athenaToolsRepository"/> class.</param>
        /// <param name="athenaToolsBlobRepository">The instance of <see cref="athenaToolsBlobRepository"/> class.</param>
        /// <param name="coiSearchService">The instance of <see cref="coiSearchService"/> class.</param>
        /// <param name="athenaInfoResourcesSearchService">The instance of <see cref="athenaInfoResourcesSearchService"/> class.</param>
        /// <param name="researchProjectsSearchService">The instance of <see cref="researchProjectsSearchService"/> class.</param>
        public AthenaIngestionHelper(
            ILogger<AthenaIngestionHelper> logger,
            IUserRepository userRepository,
            IUserBlobRepository userBlobRepository,
            IAthenaIngestionMapper athenaIngestionMapper,
            IAthenaInfoResourcesRepository athenaInfoResourcesRepository,
            ICoiRepository coiRepository,
            IAthenaInfoResourceBlobRepository athenaInfoResourceBlobRepository,
            ICommunityOfInterestBlobRepository communityOfInterestBlobRepository,
            IAthenaResearchProjectBlobRepository athenaResearchProjectBlobRepository,
            IResearchProjectsRepository researchProjectsRepository,
            IUsersSearchService usersSearchService,
            IAthenaIngestionRepository athenaIngestionRepository,
            IAthenaFileNamesBlobRepository athenaFileNamesBlobRepository,
            IAthenaResearchProposalsBlobRepository athenaResearchProposalBlobRepository,
            IResearchProposalsRepository researchProposalRepository,
            IResearchProposalsSearchService researchProposalsSearchService,
            ISponsorBlobRepository sponsorBlobRepository,
            ISponsorRepository sponsorRepository,
            ISponsorsSearchService sponsorSearchService,
            IPartnerBlobRepository partnerBlobRepository,
            IPartnersRepository partnersRepository,
            IPartnersSearchService partnersSearchService,
            IResearchRequestBlobRepository researchRequestBlobRepository,
            IResearchRequestsRepository researchRequestRepository,
            IResearchRequestsSearchService researchRequestSearchService,
            IAthenaEventsBlobRepository athenaEventsBlobRepository,
            IEventsRepository eventsRepository,
            IAthenaEventsSearchService eventsSearchService,
            IAthenaToolsBlobRepository athenaToolsBlobRepository,
            IAthenaToolsRepository athenaToolsRepository,
            ICoiSearchService coiSearchService,
            IAthenaInfoResourcesSearchService athenaInfoResourcesSearchService,
            IResearchProjectsSearchService researchProjectsSearchService)
        {
            this.athenaIngestionRepository = athenaIngestionRepository;
            this.athenaFileNamesBlobRepository = athenaFileNamesBlobRepository;
            this.athenaIngestionMapper = athenaIngestionMapper;
            this.athenaInfoResourcesRepository = athenaInfoResourcesRepository;
            this.coiRepository = coiRepository;
            this.communityOfInterestBlobRepository = communityOfInterestBlobRepository;
            this.athenaInfoResourceBlobRepository = athenaInfoResourceBlobRepository;
            this.athenaResearchProjectBlobRepository = athenaResearchProjectBlobRepository;
            this.researchProjectsRepository = researchProjectsRepository;
            this.usersSearchService = usersSearchService;
            this.userBlobRepository = userBlobRepository;
            this.userRepository = userRepository;
            this.researchProposalsSearchService = researchProposalsSearchService;
            this.researchProposalRepository = researchProposalRepository;
            this.athenaResearchProposalBlobRepository = athenaResearchProposalBlobRepository;
            this.sponsorBlobRepository = sponsorBlobRepository;
            this.sponsorRepository = sponsorRepository;
            this.sponsorSearchService = sponsorSearchService;
            this.partnerBlobRepository = partnerBlobRepository;
            this.partnersRepository = partnersRepository;
            this.partnersSearchService = partnersSearchService;
            this.researchRequestBlobRepository = researchRequestBlobRepository;
            this.researchRequestRepository = researchRequestRepository;
            this.researchRequestSearchService = researchRequestSearchService;
            this.athenaEventsBlobRepository = athenaEventsBlobRepository;
            this.eventsRepository = eventsRepository;
            this.eventsSearchService = eventsSearchService;
            this.athenaToolsBlobRepository = athenaToolsBlobRepository;
            this.athenaToolsRepository = athenaToolsRepository;
            this.coiSearchService = coiSearchService;
            this.athenaInfoResourcesSearchService = athenaInfoResourcesSearchService;
            this.researchProjectsSearchService = researchProjectsSearchService;
        }

        /// <inheritdoc/>
        public async Task AddUpdateEntity(string entityName, string path)
        {
            try
            {
                switch (entityName)
                {
                    case "AthenaUsers":
                        try
                        {
                            await this.AddUsersAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting user data.");
                            throw;
                        }

                    case "AthenaInfoResource":
                        try
                        {
                            await this.AddInfoResourceAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting InfoResources data.");
                            throw;
                        }

                    case "CommunitOfInterest":
                        try
                        {
                            await this.AddCommunitiesAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting CommunitiesOfInterest data.");
                            throw;
                        }

                    case "AthenaResearchProjects":
                        try
                        {
                            await this.AddResearchProjectsAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting ResearchProjects data.");
                            throw;
                        }

                    case "AthenaResearchProposals":
                        try
                        {
                            await this.AddResearchProposalsAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting ResearchProposals data.");
                            throw;
                        }

                    case "AthenaSponsors":
                        try
                        {
                            await this.AddSponsorsAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting Sponsors data.");
                            throw;
                        }

                    case "AthenaPartners":
                        try
                        {
                            await this.AddPartnersAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting Partners data.");
                            throw;
                        }

                    case "AthenaResearchRequests":
                        try
                        {
                            await this.AddAthenaResearchRequestsAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting ResearchRequests data.");
                            throw;
                        }

                    case "AthenaEvents":
                        try
                        {
                            await this.AddAthenaEventsAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting AthenaEvents data.");
                            throw;
                        }

                    case "AthenaTools":
                        try
                        {
                            await this.AddAthenaToolsAsync(path);

                            var athenaIngestionDetails = await this.athenaIngestionRepository.GetAthenaIngestionByEntityNameAsync(entityName);
                            var athenaIngestionEntity = this.athenaIngestionMapper.MapForUpdateModel(entityName, path, athenaIngestionDetails);
                            await this.athenaIngestionRepository.InsertOrMergeAsync(athenaIngestionEntity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Error occurred while inserting AthenaTools data.");
                            throw;
                        }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while switching entities.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddUsersAsync(string path)
        {
            try
            {
                var userJson = await this.userBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                if (userJson == null)
                {
                    throw new Exception($"Not a valid Json file as required field is not provided");
                }

                var jsonDataBatches = userJson.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.userRepository.GetUserDetailsByExternalUserIdAsync(jsonRecord.UserId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateUserSyncModel(jsonRecord, existingRecord);
                                await this.userRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update user data with user Id: {jsonRecord.UserId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddUserSyncModel(jsonRecord);
                                await this.userRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert user data with user Id: {jsonRecord.UserId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.usersSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddInfoResourceAsync(string path)
        {
            try
            {
                var infoResourceJson = await this.athenaInfoResourceBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = infoResourceJson.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.athenaInfoResourcesRepository.GetInfoResourceByResourceIdAsync(jsonRecord.InfoResourceId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateInfoResourceSyncModel(jsonRecord, existingRecord);
                                await this.athenaInfoResourcesRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update athena info resource data with InfoResource Id: {jsonRecord.InfoResourceId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddInfoResourceSyncModel(jsonRecord);
                                await this.athenaInfoResourcesRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert athena info resource data with InfoResource Id: {jsonRecord.InfoResourceId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.athenaInfoResourcesSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to deserialize the info resource Json File. The exception details: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddCommunitiesAsync(string path)
        {
            try
            {
                var communityJsons = await this.communityOfInterestBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = communityJsons.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.coiRepository.GetCommunityByIdAsync(jsonRecord.CoiId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateCommunitySyncModel(jsonRecord, existingRecord);
                                await this.coiRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update community of interest data with coi Id: {jsonRecord.CoiId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddCommunitySyncModel(jsonRecord);
                                await this.coiRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert community of interest data with coi Id: {jsonRecord.CoiId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.coiSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise community of interest Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddResearchProjectsAsync(string path)
        {
            try
            {
                var researchProjectJsons = await this.athenaResearchProjectBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = researchProjectJsons.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.researchProjectsRepository.GetResearchProjectByProjectIdAsync(jsonRecord.ResearchProjectId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateResearchProjectSyncModel(jsonRecord, existingRecord);
                                await this.researchProjectsRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update research project data with researchProject Id: {jsonRecord.ResearchProjectId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddResearchProjectSyncModel(jsonRecord);
                                await this.researchProjectsRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert research project data with researchProject Id: {jsonRecord.ResearchProjectId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.researchProjectsSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddResearchProposalsAsync(string path)
        {
            try
            {
                var researchProposalJsons = await this.athenaResearchProposalBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = researchProposalJsons.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.researchProposalRepository.GetResearchProposalByIdAsync(jsonRecord.ResearchProposalId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateResearchProposalsSyncModel(jsonRecord, existingRecord);
                                await this.researchProposalRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update research proposal data with researchProposal Id: {jsonRecord.ResearchProposalId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddResearchProposalsSyncModel(jsonRecord);
                                await this.researchProposalRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert research proposal data with researchProposal Id: {jsonRecord.ResearchProposalId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.researchProposalsSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise research proposal Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddSponsorsAsync(string path)
        {
            try
            {
                var sponsorJsons = await this.sponsorBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = sponsorJsons.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.sponsorRepository.GetSponsorByIdAsync(jsonRecord.SponsorId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateSponsorsSyncModel(jsonRecord, existingRecord);
                                await this.sponsorRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update Sponsor data with Sponsor Id: {jsonRecord.SponsorId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddSponsorsSyncModel(jsonRecord);
                                await this.sponsorRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert Sponsor data with Sponsor Id: {jsonRecord.SponsorId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.sponsorSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise Sponsor Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddPartnersAsync(string path)
        {
            try
            {
                var sponsorJsons = await this.partnerBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = sponsorJsons.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.partnersRepository.GetPartnerDetailsByPartnerIdAsync(jsonRecord.PartnerId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdatePartnersSyncModel(jsonRecord, existingRecord);
                                await this.partnersRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update Partner data with Partner Id: {jsonRecord.PartnerId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddPartnersSyncModel(jsonRecord);
                                await this.partnersRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert Partner data with Partner Id: {jsonRecord.PartnerId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.partnersSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise Partner Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddAthenaResearchRequestsAsync(string path)
        {
            try
            {
                var researchRequestJsons = await this.researchRequestBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = researchRequestJsons.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.researchRequestRepository.GetResearchRequestDetailsByIdAsync(jsonRecord.ResearchRequestId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateResearchRequestSyncModel(jsonRecord, existingRecord);
                                await this.researchRequestRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update research request data with ResearchRequest Id: {jsonRecord.ResearchRequestId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddResearchRequestSyncModel(jsonRecord);
                                await this.researchRequestRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert research request data with ResearchRequest Id: {jsonRecord.ResearchRequestId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.researchRequestSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddAthenaEventsAsync(string path)
        {
            try
            {
                var eventJsons = await this.athenaEventsBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = eventJsons.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.eventsRepository.GetEventDetailsByEventIdAsync(jsonRecord.EventId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateEventSyncModel(jsonRecord, existingRecord);
                                await this.eventsRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update Event data with Event Id: {jsonRecord.EventId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddEventSyncModel(jsonRecord);
                                await this.eventsRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert Event data with Event Id: {jsonRecord.EventId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                await this.eventsSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise Event Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task AddAthenaToolsAsync(string path)
        {
            try
            {
                var toolJsons = await this.athenaToolsBlobRepository.GetBlobJsonFileContentByUrlAsync(path);
                var jsonDataBatches = toolJsons.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.athenaToolsRepository.GetAthenaToolByIdAsync(jsonRecord.ToolId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForUpdateAthenaToolSyncModel(jsonRecord, existingRecord);
                                await this.athenaToolsRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update Tool data with Tool Id: {jsonRecord.ToolId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.athenaIngestionMapper.MapForAddAthenaToolSyncModel(jsonRecord);
                                await this.athenaToolsRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert Tool data with Tool Id: {jsonRecord.ToolId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise Athena tool Json. {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AthenaIngestionEntity>> GetAthenaIngestionDetailsAsync()
        {
            return await this.athenaIngestionRepository.GetAllAsync();
            /*return records.Select(x => this.athenaIngestionMapper.MapForViewModel(x));*/
        }
    }
}
