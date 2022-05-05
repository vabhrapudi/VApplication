// <copyright file="AdaptiveCardService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Services.AdaptiveCard
{
    using System;
    using System.IO;
    using AdaptiveCards;
    using AdaptiveCards.Templating;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Athena.Models;
    using Teams.Apps.Athena.Bot;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Resources;

    /// <summary>
    /// This service provides methods to create adaptive cards.
    /// </summary>
    public class AdaptiveCardService : IAdaptiveCardService
    {
        private const int DefaultCardCacheDurationInHour = 12;

        private const string NewNewsArticleRequestCardCacheKey = "_new_news_article_request_card";
        private const string NewCoiRequestCardCacheKey = "_new_coi_request_card";

        /// <summary>
        /// Provides welcome card cache.
        /// </summary>
        private const string WelcomeCardCacheKey = "_welcome-card-for-user";

        /// <summary>
        /// The current cultures' string localizer.
        /// </summary>
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// Information about the web hosting environment an application is running in.
        /// </summary>
        private readonly IWebHostEnvironment webHostEnvironment;

        /// <summary>
        /// A set of key/value application configuration properties for Activity settings.
        /// </summary>
        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// Memory cache instance to store and retrieve adaptive card payload.
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveCardService"/> class.
        /// </summary>
        /// <param name="localizer">The current cultures' string localizer.</param>
        /// <param name="webHostEnvironment">Information about the web hosting environment an application is running in.</param>
        /// <param name="botOptions">A set of key/value application configuration properties.</param>
        /// <param name="memoryCache">The instance of <see cref="IMemoryCache"/>.</param>
        public AdaptiveCardService(
            IStringLocalizer<Strings> localizer,
            IWebHostEnvironment webHostEnvironment,
            IOptions<BotSettings> botOptions,
            IMemoryCache memoryCache)
        {
            this.localizer = localizer;
            this.webHostEnvironment = webHostEnvironment;
            this.botOptions = botOptions;
            this.memoryCache = memoryCache;
            this.localizer = localizer;
        }

        /// <inheritdoc/>
        public Attachment GetWelcomeCard()
        {
            var cardPayload = this.GetCardPayload(WelcomeCardCacheKey, "\\welcome-card-for-user.json");

            var welcomeCard = new WelcomeCard
            {
                WelcomeCardTitle = this.localizer.GetString("WelcomeCardTitle"),
                WelcomeCardContentLine1 = this.localizer.GetString("WelcomeCardContentLine1"),
                WelcomeCardContentPoint1 = this.localizer.GetString("WelcomeCardContentPoint1"),
                WelcomeCardContentPoint2 = this.localizer.GetString("WelcomeCardContentPoint2"),
                WelcomeCardContentPoint3 = this.localizer.GetString("WelcomeCardContentPoint3"),
                WelcomeCardContentLine2 = this.localizer.GetString("WelcomeCardContentLine2"),
                SettingsButtonText = this.localizer.GetString("SettingsButtonText"),
                SettingsButtonUrl = $"https://teams.microsoft.com/l/entity/{this.botOptions.Value.UserManifestId}/UserEntity",
            };

            var cardTemplate = new AdaptiveCardTemplate(cardPayload);
            var cardJson = cardTemplate.Expand(welcomeCard);

            return this.GetAdaptiveCardAttachment(cardJson);
        }

        /// <inheritdoc/>
        public Attachment GetNewNewsArticleRequestCard(NewsEntity newsArticleRequestDetails, string createdByName = null)
        {
            if (newsArticleRequestDetails == null)
            {
                throw new ArgumentNullException(nameof(newsArticleRequestDetails));
            }

            var cardPayload = this.GetCardPayload(NewNewsArticleRequestCardCacheKey, "\\new-news-article-request-card.json");

            var cardTemplate = new AdaptiveCardTemplate(cardPayload);

            var newNewsArticleRequestCard = new NewNewsArticleRequestCard
            {
                NewsRequestAdaptiveCardTitle = this.localizer.GetString("NewsRequestAdaptiveCardTitle"),
                AdaptiveCardTitleLabel = this.localizer.GetString("AdaptiveCardTitleLabel"),
                AdaptiveCardDateLabel = this.localizer.GetString("AdaptiveCardDateLabel"),
                AdaptiveCardStatusLabel = this.localizer.GetString("AdaptiveCardStatusLabel"),
                AdaptiveCardViewDetailsButtonLabel = this.localizer.GetString("AdaptiveCardViewDetailsButtonLabel"),
                Title = newsArticleRequestDetails.Title,
                CreatedOn = newsArticleRequestDetails.CreatedAt.ToZuluTimeFormatWithoutMilliseconds(),
                Status = this.GetLocalizedRequestStatus((NewsArticleRequestStatus)newsArticleRequestDetails.Status),
                NewsTableId = newsArticleRequestDetails.TableId,
                Command = createdByName == null ? BotCommand.ViewReadonlyNewsArticleRequestDetails : BotCommand.ViewNewsArticleRequestDetails,
                CreatedByName = createdByName,
                AdminComment = newsArticleRequestDetails.Status == (int)RequestStatus.Rejected ? newsArticleRequestDetails.AdminComment : null,
            };

            var cardJson = cardTemplate.Expand(newNewsArticleRequestCard);

            return this.GetAdaptiveCardAttachment(cardJson);
        }

        /// <inheritdoc/>
        public Attachment GetNewCoiRequestCard(CommunityOfInterestEntity coiRequestDetails, string createdByName = null)
        {
            if (coiRequestDetails == null)
            {
                throw new ArgumentNullException(nameof(coiRequestDetails));
            }

            var cardPayload = this.GetCardPayload(NewCoiRequestCardCacheKey, "\\new-coi-request-card.json");

            var cardTemplate = new AdaptiveCardTemplate(cardPayload);

            var newCoiRequestCard = new NewCoiRequestCard
            {
                CoiRequestAdaptiveCardTitle = this.localizer.GetString("CoiRequestAdaptiveCardTitle"),
                AdaptiveCardTitleLabel = this.localizer.GetString("AdaptiveCardTitleLabel"),
                AdaptiveCardTypeLabel = this.localizer.GetString("AdaptiveCardTypeLabel"),
                AdaptiveCardDateLabel = this.localizer.GetString("AdaptiveCardDateLabel"),
                AdaptiveCardStatusLabel = this.localizer.GetString("AdaptiveCardStatusLabel"),
                AdaptiveCardViewDetailsButtonLabel = this.localizer.GetString("AdaptiveCardViewDetailsButtonLabel"),
                Title = coiRequestDetails.CoiName,
                Type = this.GetLocalizedCoiType((CoiTeamType)coiRequestDetails.Type),
                CreatedOn = coiRequestDetails.CreatedOn != null ? coiRequestDetails.CreatedOn?.ToZuluTimeFormatWithoutMilliseconds() : null,
                Status = this.GetLocalizedRequestStatus((CoiRequestStatus)coiRequestDetails.Status),
                CoiTableId = coiRequestDetails.TableId,
                Command = createdByName == null ? BotCommand.ViewReadonlyCoiRequestDetails : BotCommand.ViewCoiRequestDetails,
                CreatedByName = createdByName,
                AdminComment = coiRequestDetails.Status == (int)RequestStatus.Rejected ? coiRequestDetails.AdminComment : null,
            };

            var cardJson = cardTemplate.Expand(newCoiRequestCard);

            return this.GetAdaptiveCardAttachment(cardJson);
        }

        /// <summary>
        /// Get card payload from memory.
        /// </summary>
        /// <param name="cardCacheKey">Card cache key.</param>
        /// <param name="jsonTemplateFileName">File name for JSON adaptive card template.</param>
        /// <returns>Returns adaptive card payload in JSON format.</returns>
        private string GetCardPayload(string cardCacheKey, string jsonTemplateFileName)
        {
            bool isCardPayloadExistsInCache = this.memoryCache.TryGetValue(cardCacheKey, out string cardPayload);

            if (!isCardPayloadExistsInCache)
            {
                // If cache duration is not specified then by default card cache duration will be set.
                var cacheDurationInHour = TimeSpan.FromHours(this.botOptions.Value.CardCacheDurationInHour);
                cacheDurationInHour = cacheDurationInHour.Hours <= 0 ? TimeSpan.FromHours(DefaultCardCacheDurationInHour) : cacheDurationInHour;

                var cardJsonFilePath = Path.Combine(this.webHostEnvironment.ContentRootPath, $".\\Cards\\{jsonTemplateFileName}");
                cardPayload = File.ReadAllText(cardJsonFilePath);

                this.memoryCache.Set(cardCacheKey, cardPayload, cacheDurationInHour);
            }

            return cardPayload;
        }

        /// <summary>
        /// Gets an card attachment.
        /// </summary>
        /// <param name="cardJson">The card JSON.</param>
        /// <returns>The adaptive card attachment.</returns>
        private Attachment GetAdaptiveCardAttachment(string cardJson)
        {
            AdaptiveCard card = AdaptiveCard.FromJson(cardJson).Card;

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };

            return adaptiveCardAttachment;
        }

        /// <summary>
        /// Gets the localized COI team type name.
        /// </summary>
        /// <param name="coiTeamType">The type of COI team.</param>
        /// <returns>The localized COI team type name.</returns>
        private string GetLocalizedCoiType(CoiTeamType coiTeamType)
        {
            return coiTeamType switch
            {
                CoiTeamType.Private => this.localizer.GetString("CoiTypePrivate"),
                CoiTeamType.Public => this.localizer.GetString("CoiTypePublic"),
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Gets the localized request status.
        /// </summary>
        /// <param name="newsArticleRequestStatus">The status of news article request.</param>
        /// <returns>The localized news article request status.</returns>
        private string GetLocalizedRequestStatus(NewsArticleRequestStatus newsArticleRequestStatus)
        {
            return newsArticleRequestStatus switch
            {
                NewsArticleRequestStatus.Draft => this.localizer.GetString("RequestStatusDraft"),
                NewsArticleRequestStatus.Pending => this.localizer.GetString("RequestStatusPending"),
                NewsArticleRequestStatus.Approved => this.localizer.GetString("RequestStatusApproved"),
                NewsArticleRequestStatus.Rejected => this.localizer.GetString("RequestStatusRejected"),
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Gets the localized request status.
        /// </summary>
        /// <param name="coiRequestStatus">The status of COI request.</param>
        /// <returns>The localized COI request status.</returns>
        private string GetLocalizedRequestStatus(CoiRequestStatus coiRequestStatus)
        {
            return coiRequestStatus switch
            {
                CoiRequestStatus.Draft => this.localizer.GetString("RequestStatusDraft"),
                CoiRequestStatus.Pending => this.localizer.GetString("RequestStatusPending"),
                CoiRequestStatus.Approved => this.localizer.GetString("RequestStatusApproved"),
                CoiRequestStatus.Rejected => this.localizer.GetString("RequestStatusRejected"),
                _ => string.Empty,
            };
        }
    }
}