// <copyright file="HomeMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The mapper methods related to home tab.
    /// </summary>
    public class HomeMapper : IHomeMapper
    {
        /// <inheritdoc/>
        public DailyBriefingHomeArticleDTO MapForViewModel(ResearchProjectDTO researchProjectDTO)
        {
            researchProjectDTO = researchProjectDTO ?? throw new ArgumentNullException(nameof(researchProjectDTO));

            return new DailyBriefingHomeArticleDTO
            {
                ResourceId = researchProjectDTO.TableId,
                Title = researchProjectDTO.Title,
                Description = researchProjectDTO.Abstract,
                UpdatedOn = researchProjectDTO.LastUpdate,
                ArticleUrl = null,
                NodeTypeId = researchProjectDTO.NodeTypeId,
            };
        }

        /// <inheritdoc/>
        public DailyBriefingHomeArticleDTO MapForViewModel(ResearchRequestViewDTO researchRequestViewDTO)
        {
            researchRequestViewDTO = researchRequestViewDTO ?? throw new ArgumentNullException(nameof(researchRequestViewDTO));

            return new DailyBriefingHomeArticleDTO
            {
                ResourceId = researchRequestViewDTO.TableId,
                Title = researchRequestViewDTO.Title,
                Description = researchRequestViewDTO.Description,
                UpdatedOn = researchRequestViewDTO.LastUpdate,
                ArticleUrl = null,
                NodeTypeId = researchRequestViewDTO.NodeTypeId,
            };
        }

        /// <inheritdoc/>
        public DailyBriefingHomeArticleDTO MapForViewModel(ResearchProposalViewDTO researchProposalViewDTO)
        {
            researchProposalViewDTO = researchProposalViewDTO ?? throw new ArgumentNullException(nameof(researchProposalViewDTO));

            return new DailyBriefingHomeArticleDTO
            {
                ResourceId = researchProposalViewDTO.TableId,
                Title = researchProposalViewDTO.Title,
                Description = researchProposalViewDTO.Description,
                UpdatedOn = researchProposalViewDTO.LastUpdate,
                ArticleUrl = null,
                NodeTypeId = researchProposalViewDTO.NodeTypeId,
            };
        }

        /// <inheritdoc/>
        public DailyBriefingHomeArticleDTO MapForViewModel(EventDTO eventDTO)
        {
            eventDTO = eventDTO ?? throw new ArgumentNullException(nameof(eventDTO));

            return new DailyBriefingHomeArticleDTO
            {
                ResourceId = eventDTO.TableId,
                Title = eventDTO.Title,
                Description = eventDTO.Description,
                UpdatedOn = eventDTO.LastUpdate,
                ArticleUrl = eventDTO.WebSite,
                NodeTypeId = eventDTO.NodeTypeId,
            };
        }

        /// <inheritdoc/>
        public DailyBriefingHomeArticleDTO MapForViewModel(CoiEntityDTO coiEntityDTO)
        {
            coiEntityDTO = coiEntityDTO ?? throw new ArgumentNullException(nameof(coiEntityDTO));

            return new DailyBriefingHomeArticleDTO
            {
                ResourceId = coiEntityDTO.TableId,
                Title = coiEntityDTO.CoiName,
                Description = coiEntityDTO.CoiDescription,
                UpdatedOn = coiEntityDTO.UpdatedOn,
                ArticleUrl = coiEntityDTO.WebSite,
                NodeTypeId = coiEntityDTO.NodeTypeId,
            };
        }

        /// <inheritdoc/>
        public DailyBriefingHomeArticleDTO MapForViewModel(NewsEntityDTO newsEntityDTO)
        {
            newsEntityDTO = newsEntityDTO ?? throw new ArgumentNullException(nameof(newsEntityDTO));

            return new DailyBriefingHomeArticleDTO
            {
                ResourceId = newsEntityDTO.TableId,
                Title = newsEntityDTO.Title,
                Description = newsEntityDTO.Abstract,
                UpdatedOn = newsEntityDTO.PublishedDate,
                ArticleUrl = newsEntityDTO.ExternalLink,
                NodeTypeId = newsEntityDTO.NodeTypeId,
            };
        }

        /// <inheritdoc/>
        public DailyBriefingHomeArticleDTO MapForViewModel(AthenaInfoResourceDTO athenaInfoResourceDTO)
        {
            athenaInfoResourceDTO = athenaInfoResourceDTO ?? throw new ArgumentNullException(nameof(athenaInfoResourceDTO));

            return new DailyBriefingHomeArticleDTO
            {
                ResourceId = athenaInfoResourceDTO.TableId,
                Title = athenaInfoResourceDTO.Title,
                Description = athenaInfoResourceDTO.Description,
                UpdatedOn = athenaInfoResourceDTO.LastUpdate,
                ArticleUrl = athenaInfoResourceDTO.Website,
                NodeTypeId = athenaInfoResourceDTO.NodeTypeId,
            };
        }
    }
}
