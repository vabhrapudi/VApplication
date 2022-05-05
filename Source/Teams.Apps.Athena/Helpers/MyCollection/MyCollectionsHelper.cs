// <copyright file="MyCollectionsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Models.Enums;
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// Provides helper methods for managing my collection entity.
    /// </summary>
    public class MyCollectionsHelper : IMyCollectionsHelper
    {
        private const int MaxNumberOfCollectionsPerUser = 15;

        private const int MaxNumberOfItemsPerCollection = 10;

        /// <summary>
        /// The instance of repository accessors to access repositories.
        /// </summary>
        private readonly IMyCollectionsRepository myCollectionsRepository;

        /// <summary>
        /// The instance of Microsoft graph service.
        /// </summary>
        private readonly IUserService userGraphService;

        /// <summary>
        /// The instance of user graph service mapper.
        /// </summary>
        private readonly IUserGraphServiceMapper userGraphServiceMapper;

        /// <summary>
        /// The instance of my collection model mapper.
        /// </summary>
        private readonly IMyCollectionsMapper myCollectionMapper;

        /// <summary>
        /// The instance of news repository.
        /// </summary>
        private readonly INewsRepository newsRepository;

        /// <summary>
        /// The instance of user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// The instance of COI repository.
        /// </summary>
        private readonly ICoiRepository coiRepository;

        /// <summary>
        /// The instance of research request repository.
        /// </summary>
        private readonly IResearchRequestsRepository researchRequestsRepository;

        /// <summary>
        /// The instance of research project repository.
        /// </summary>
        private readonly IResearchProjectsRepository researchProjectsRepository;

        /// <summary>
        /// The instance of research proposal repository.
        /// </summary>
        private readonly IResearchProposalsRepository researchProposalsRepository;

        /// <summary>
        /// The instance of events repository.
        /// </summary>
        private readonly IEventsRepository eventsRepository;

        /// <summary>
        /// The instance of partners repository.
        /// </summary>
        private readonly IPartnersRepository partnersRepository;

        /// <summary>
        /// The instance of sponsor repository.
        /// </summary>
        private readonly ISponsorRepository sponsorRepository;

        /// <summary>
        /// The instance of <see cref="AthenaNewsSourcesBlobRepository"/> class.
        /// </summary>
        private readonly IAthenaNewsSourcesBlobRepository athenaNewsSourcesBlobRepository;

        /// <summary>
        /// The instance of <see cref="AthenaResearchSourcesBlobRepository"/> class.
        /// </summary>
        private readonly IAthenaResearchSourcesBlobRepository athenaResearchSourcesBlobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyCollectionsHelper"/> class.
        /// </summary>
        /// <param name="myCollectionsRepository">The instance of <see cref="MyCollectionsRepository"/></param>
        /// <param name="myCollectionMapper">The instance of <see cref="MyCollectionsMapper"/></param>
        /// <param name="newsRepository">The instance of <see cref="NewsRepository"/>.</param>
        /// <param name="userGraphService">The instance of <see cref="UserService"/>.</param>
        /// <param name="userGraphServiceMapper">The instance of <see cref="UserGraphServiceMapper"/>.</param>
        /// <param name="userRepository">The instance of <see cref="UserRepository"/></param>
        /// <param name="coiRepository">The instance of <see cref="CoiRepository"/></param>
        /// <param name="researchRequestsRepository">The instance of <see cref="ResearchRequestsRepository"/></param>
        /// <param name="researchProjectsRepository">The instance of <see cref="ResearchProjectsRepository"/></param>
        /// <param name="researchProposalsRepository">The instance of <see cref="ResearchProposalsRepository"/></param>
        /// <param name="eventsRepository">The instance of <see cref="EventsRepository"/></param>
        /// <param name="partnersRepository">The instance of <see cref="PartnersRepository"/></param>
        /// <param name="sponsorRepository">The instance of <see cref="SponsorRepository"/></param>
        /// <param name="athenaNewsSourcesBlobRepository">The instance of <see cref="AthenaNewsSourcesBlobRepository"/> class.</param>
        /// <param name="athenaResearchSourcesBlobRepository">The instance of <see cref="AthenaResearchSourcesBlobRepository"/> class.</param>
        public MyCollectionsHelper(
            IMyCollectionsRepository myCollectionsRepository,
            IMyCollectionsMapper myCollectionMapper,
            INewsRepository newsRepository,
            IUserService userGraphService,
            IUserGraphServiceMapper userGraphServiceMapper,
            IUserRepository userRepository,
            ICoiRepository coiRepository,
            IResearchRequestsRepository researchRequestsRepository,
            IResearchProjectsRepository researchProjectsRepository,
            IResearchProposalsRepository researchProposalsRepository,
            IEventsRepository eventsRepository,
            IPartnersRepository partnersRepository,
            ISponsorRepository sponsorRepository,
            IAthenaNewsSourcesBlobRepository athenaNewsSourcesBlobRepository,
            IAthenaResearchSourcesBlobRepository athenaResearchSourcesBlobRepository)
        {
            this.myCollectionsRepository = myCollectionsRepository;
            this.myCollectionMapper = myCollectionMapper;
            this.newsRepository = newsRepository;
            this.userGraphService = userGraphService;
            this.userGraphServiceMapper = userGraphServiceMapper;
            this.userRepository = userRepository;
            this.coiRepository = coiRepository;
            this.researchRequestsRepository = researchRequestsRepository;
            this.researchProjectsRepository = researchProjectsRepository;
            this.researchProposalsRepository = researchProposalsRepository;
            this.eventsRepository = eventsRepository;
            this.partnersRepository = partnersRepository;
            this.sponsorRepository = sponsorRepository;
            this.athenaNewsSourcesBlobRepository = athenaNewsSourcesBlobRepository;
            this.athenaResearchSourcesBlobRepository = athenaResearchSourcesBlobRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MyCollectionsItemsViewDTO>> GetCollectionItemsByIdAsync(string collectionId)
        {
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            var responseResult = new List<MyCollectionsItemsViewDTO>();
            var collectionEntity = await this.myCollectionsRepository.GetAsync(MyCollectionsTableMetadata.MyCollectionsPartition, collectionId);
            if (collectionEntity != null)
            {
                if (!string.IsNullOrEmpty(collectionEntity.Items))
                {
                    var collectionItem = JsonConvert.DeserializeObject<List<Item>>(collectionEntity.Items).GroupBy(item => item.ItemType).ToList();
                    var itemList = new List<string>();
                    foreach (var item in collectionItem)
                    {
                        itemList = item.Select(u => u.ItemId).ToList();
                        switch (item.Key)
                        {
                            case (int)CollectionItemType.News:
                                var newsRowFilter = this.newsRepository.GetRowKeysFilter(itemList);
                                var newsEntity = await this.newsRepository.GetWithFilterAsync(newsRowFilter);
                                var newsUserIds = newsEntity.Where(u => !string.IsNullOrEmpty(u.CreatedBy)).Select(u => u.CreatedBy).ToList();
                                var newsUsers = await this.userGraphService.GetUsersAsync(newsUserIds);
                                var newsUserDetails = newsUsers.Select(user => this.userGraphServiceMapper.MapToViewModel(user)).ToList();
                                foreach (var news in newsEntity)
                                {
                                    string newsSourceTitle = null;
                                    var newsSource = await this.athenaNewsSourcesBlobRepository.GetNewsSourceById(news.NewsSourceId);
                                    if (newsSource != null)
                                    {
                                        newsSourceTitle = newsSource.Title;
                                    }

                                    responseResult.Add(this.myCollectionMapper.MapForNewsCollectionItemViewModel(news, collectionId, newsSourceTitle));
                                }

                                break;

                            case (int)CollectionItemType.COI:
                                var coiRowFilter = this.coiRepository.GetRowKeysFilter(itemList);
                                var coiEntity = await this.coiRepository.GetWithFilterAsync(coiRowFilter);
                                var coiUserIds = coiEntity.Where(u => !string.IsNullOrEmpty(u.CreatedByObjectId)).Select(u => u.CreatedByObjectId).ToList();
                                var coiUsers = await this.userGraphService.GetUsersAsync(coiUserIds);
                                var coiUserDetails = coiUsers.Select(user => this.userGraphServiceMapper.MapToViewModel(user)).ToList();
                                foreach (var coi in coiEntity)
                                {
                                    responseResult.Add(this.myCollectionMapper.MapForCoiCollectionItemViewModel(coi, collectionId));
                                }

                                break;

                            case (int)CollectionItemType.User:
                                var usersRowFilter = this.userRepository.GetRowKeysFilter(itemList);
                                var usersEntity = await this.userRepository.GetWithFilterAsync(usersRowFilter);
                                foreach (var user in usersEntity)
                                {
                                    var userName = user.UserDisplayName;
                                    string userProfilePhoto = null;
                                    if (!string.IsNullOrEmpty(user.UserId))
                                    {
                                        userProfilePhoto = await this.userGraphService.GetUserProfilePhotoAsync(user.UserId);
                                    }

                                    responseResult.Add(this.myCollectionMapper.MapForUserCollectionItemViewModel(user, collectionId, userName, userProfilePhoto));
                                }

                                break;

                            case (int)CollectionItemType.ResearchProject:
                                var researchProjectRowFilter = this.researchProjectsRepository.GetRowKeysFilter(itemList);
                                var researchProjectsEntity = await this.researchProjectsRepository.GetWithFilterAsync(researchProjectRowFilter);
                                foreach (var researchProject in researchProjectsEntity)
                                {
                                    string researchSourceTitle = null;
                                    var researchSource = await this.athenaResearchSourcesBlobRepository.GetResearchSourceById(researchProject.ResearchSourceId);
                                    if (researchSource != null)
                                    {
                                        researchSourceTitle = researchSource.Title;
                                    }

                                    responseResult.Add(this.myCollectionMapper.MapForResearchProjectCollectionItemViewModel(researchProject, collectionId, researchSourceTitle));
                                }

                                break;

                            case (int)CollectionItemType.ResearchRequest:
                                var researchRequestRowFilter = this.researchRequestsRepository.GetRowKeysFilter(itemList);
                                var researchRequestEntity = await this.researchRequestsRepository.GetWithFilterAsync(researchRequestRowFilter);
                                foreach (var researchRequest in researchRequestEntity)
                                {
                                    string researchSourceTitle = null;
                                    var researchSource = await this.athenaResearchSourcesBlobRepository.GetResearchSourceById(researchRequest.ResearchSourceId);
                                    if (researchSource != null)
                                    {
                                        researchSourceTitle = researchSource.Title;
                                    }

                                    responseResult.Add(this.myCollectionMapper.MapForResearchRequestCollectionItemViewModel(researchRequest, collectionId, researchSourceTitle));
                                }

                                break;

                            case (int)CollectionItemType.ResearchProposal:
                                var researchProposalRowFilter = this.researchProposalsRepository.GetRowKeysFilter(itemList);
                                var researchProposalsEntity = await this.researchProposalsRepository.GetWithFilterAsync(researchProposalRowFilter);
                                foreach (var researchProposal in researchProposalsEntity)
                                {
                                    string researchSourceTitle = null;
                                    var researchSource = await this.athenaResearchSourcesBlobRepository.GetResearchSourceById(researchProposal.ResearchSourceId);
                                    if (researchSource != null)
                                    {
                                        researchSourceTitle = researchSource.Title;
                                    }

                                    responseResult.Add(this.myCollectionMapper.MapForResearchProposalCollectionItemViewModel(researchProposal, collectionId, researchSourceTitle));
                                }

                                break;

                            case (int)CollectionItemType.Event:
                                var eventsRowFilter = this.eventsRepository.GetRowKeysFilter(itemList);
                                var eventsEntity = await this.eventsRepository.GetWithFilterAsync(eventsRowFilter);
                                foreach (var eventEntity in eventsEntity)
                                {
                                    responseResult.Add(this.myCollectionMapper.MapForEventCollectionItemViewModel(eventEntity, collectionId));
                                }

                                break;

                            case (int)CollectionItemType.Partner:
                                var partnersRowFilter = this.partnersRepository.GetRowKeysFilter(itemList);
                                var partnersEntity = await this.partnersRepository.GetWithFilterAsync(partnersRowFilter);
                                foreach (var partnerEntity in partnersEntity)
                                {
                                    responseResult.Add(this.myCollectionMapper.MapForPartnerCollectionItemViewModel(partnerEntity, collectionId));
                                }

                                break;

                            case (int)CollectionItemType.Sponsor:
                                var sponsorsRowFilter = this.sponsorRepository.GetRowKeysFilter(itemList);
                                var sponsorsEntity = await this.sponsorRepository.GetWithFilterAsync(sponsorsRowFilter);
                                foreach (var sponsorEntity in sponsorsEntity)
                                {
                                    responseResult.Add(this.myCollectionMapper.MapForSponsorCollectionItemViewModel(sponsorEntity, collectionId));
                                }

                                break;
                        }
                    }
                }
            }

            return responseResult;
        }

        /// <inheritdoc/>
        public async Task<MyCollectionsViewDTO> GetCollectionByIdAsync(string collectionId)
        {
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            var collectionEntity = await this.myCollectionsRepository.GetAsync(MyCollectionsTableMetadata.MyCollectionsPartition, collectionId);
            return this.myCollectionMapper.MapForViewModel(collectionEntity);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MyCollectionsViewDTO>> GetAllCollectionsAsync(string userId)
        {
            var collections = new List<MyCollectionsViewDTO>();
            try
            {
                var response = await this.myCollectionsRepository.GetCollectionsByUserIdAsync(userId);
                foreach (var collection in response)
                {
                    collections.Add(this.myCollectionMapper.MapForViewModel(collection));
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return collections;
        }

        /// <inheritdoc/>
        public async Task<MyCollectionsEntity> GetSingleCollectionsByIdAsync(string collectionId)
        {
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return await this.myCollectionsRepository.GetAsync(MyCollectionsTableMetadata.MyCollectionsPartition, collectionId);
        }

        /// <inheritdoc/>
        public async Task<MyCollectionsViewDTO> CreateCollectionAsync(MyCollectionsCreateDTO myCollectionsCreateDTO, string userId)
        {
            myCollectionsCreateDTO = myCollectionsCreateDTO ?? throw new ArgumentNullException(nameof(myCollectionsCreateDTO));

            var collection = await this.myCollectionsRepository.GetMyCollectionAsync(myCollectionsCreateDTO.Name.Trim());

            // Collection can not be created if collection with the same name already exists.
            if (collection != null)
            {
                return null;
            }

            var createCollectionDetails = this.myCollectionMapper.MapForCreateModel(myCollectionsCreateDTO, userId);
            await this.myCollectionsRepository.CreateOrUpdateAsync(createCollectionDetails);
            return this.myCollectionMapper.MapForViewModel(createCollectionDetails);
        }

        /// <inheritdoc/>
        public async Task<MyCollectionsViewDTO> UpdateCollectionAsync(MyCollectionsUpdateDTO myCollectionsUpdateDTO, MyCollectionsEntity myCollectionsEntity)
        {
            myCollectionsUpdateDTO = myCollectionsUpdateDTO ?? throw new ArgumentNullException(nameof(myCollectionsUpdateDTO));
            myCollectionsEntity = myCollectionsEntity ?? throw new ArgumentNullException(nameof(myCollectionsEntity));
            if (myCollectionsUpdateDTO.Name != myCollectionsEntity.Name)
            {
                var existingCollection = await this.myCollectionsRepository.GetMyCollectionAsync(myCollectionsUpdateDTO.Name.Trim(), myCollectionsUpdateDTO.CollectionId);
                if (existingCollection != null)
                {
                    return null;
                }
            }

            myCollectionsEntity.Items = JsonConvert.SerializeObject(myCollectionsUpdateDTO.Items);
            var updateCollectionDetails = this.myCollectionMapper.MapForUpdateModel(myCollectionsUpdateDTO, myCollectionsEntity);
            await this.myCollectionsRepository.CreateOrUpdateAsync(updateCollectionDetails);

            return this.myCollectionMapper.MapForViewModel(updateCollectionDetails);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCollectionAsync(string collectionId)
        {
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            var delecteCollection = await this.myCollectionsRepository.GetAsync(MyCollectionsTableMetadata.MyCollectionsPartition, collectionId);
            await this.myCollectionsRepository.DeleteAsync(delecteCollection);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> AddItemsAsync(string collectionId, List<Item> items)
        {
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            List<Item> list = new List<Item>();

            var existingCollection = await this.myCollectionsRepository.GetAsync(MyCollectionsTableMetadata.TableName, collectionId);
            if (existingCollection == null)
            {
                return false;
            }

            if (existingCollection.Items == null)
            {
                list = items;
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<Item>>(existingCollection.Items);
                list.AddRange(items);
            }

            existingCollection.Items = JsonConvert.SerializeObject(list);

            if (list.Count > MaxNumberOfItemsPerCollection)
            {
                return false;
            }

            await this.myCollectionsRepository.InsertOrMergeAsync(existingCollection);
            this.myCollectionMapper.MapForViewModel(existingCollection);

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> IsCollectionsUnderLimit(string userId)
        {
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            var userCollection = await this.myCollectionsRepository.GetCollectionsByUserIdAsync(userId);

            return userCollection.Count() <= MaxNumberOfCollectionsPerUser;
        }
    }
}