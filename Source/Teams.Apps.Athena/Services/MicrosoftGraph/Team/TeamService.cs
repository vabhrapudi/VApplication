// <copyright file="TeamService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Services.MicrosoftGraph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph;
    using Teams.Apps.Athena.Authentication;
    using AthenaContants = Teams.Apps.Athena.Constants.Constants;

    /// <summary>
    /// This class provides the services related to Microsoft Teams team.
    /// </summary>
    public class TeamService : ITeamService
    {
        private const string MicrosoftTeamMemberKey = "user@odata.bind";
        private const string MicrosoftTeamTemplateKey = "template@odata.bind";
        private const string MicrosoftTeamOwnerRole = "owner";

        private readonly ILogger<TeamService> teamServiceLogger;

        private readonly IGraphServiceClient graphServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamService"/> class.
        /// </summary>
        /// <param name="teamServiceLogger">Logs errors and warnings.</param>
        /// <param name="graphServiceClient">The instance of <see cref="GraphServiceClient"/>.</param>
        public TeamService(
            ILogger<TeamService> teamServiceLogger,
            IGraphServiceClient graphServiceClient)
        {
            this.teamServiceLogger = teamServiceLogger;
            this.graphServiceClient = graphServiceClient;
        }

        /// <inheritdoc/>
        public async Task<string> CreateTeamAsync(string displayName, string description, TeamVisibilityType teamVisibilityType, Guid teamOwnerUserAadId)
        {
            displayName = displayName ?? throw new ArgumentNullException(nameof(displayName));

            if (teamOwnerUserAadId == Guid.Empty)
            {
                this.teamServiceLogger.LogError("Failed to create Microsoft Teams team as empty team owner user AAD Id was received.");
                throw new ArgumentException("Empty team owner user AAD Id was provided.", nameof(teamOwnerUserAadId));
            }

            try
            {
                var team = new Team
                {
                    DisplayName = displayName,
                    Description = description,
                    Members = new TeamMembersCollectionPage
                    {
                        new AadUserConversationMember
                        {
                            Roles = new List<string>
                            {
                                MicrosoftTeamOwnerRole,
                            },
                            AdditionalData = new Dictionary<string, object>
                            {
                                { MicrosoftTeamMemberKey, $"https://graph.microsoft.com/v1.0/users('{teamOwnerUserAadId}')" },
                            },
                        },
                    },
                    AdditionalData = new Dictionary<string, object>
                    {
                        { MicrosoftTeamTemplateKey, "https://graph.microsoft.com/v1.0/teamsTemplates('standard')" },
                    },
                    Visibility = teamVisibilityType,
                };

                string teamId = null;
                var httpRequestMessage = this.graphServiceClient.Teams.Request().GetHttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.Content = this.graphServiceClient.HttpProvider.Serializer.SerializeAsJsonContent(team);
                httpRequestMessage.Headers.Add(AthenaContants.PermissionTypeKey, GraphPermissionType.Application.ToString());

                var httpResponseMessage = await this.graphServiceClient.HttpProvider.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.Headers.TryGetValues("Location", out var headerValues))
                    {
                        teamId = headerValues?.First().Split('\'', StringSplitOptions.RemoveEmptyEntries)[1];
                    }
                }

                return teamId;
            }
#pragma warning disable CA1031 // Need to log exception details.
            catch (Exception ex)
#pragma warning restore CA1031 // Need to log exception details.
            {
                this.teamServiceLogger.LogError(ex, $"Failed to create Microsoft Teams team for user {teamOwnerUserAadId}.");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task AddMemberToTeamAsync(string teamId, Guid userAadId)
        {
            try
            {
                var conversationMember = new AadUserConversationMember
                {
                    Roles = new List<string>()
                {
                    "member",
                },
                    AdditionalData = new Dictionary<string, object>()
                {
                    {
                        "user@odata.bind", $"https://graph.microsoft.com/v1.0/users('{userAadId.ToString()}')"
                    },
                },
                };

                await this.graphServiceClient.Teams[teamId].Members.Request()
                    .Header(Athena.Constants.Constants.PermissionTypeKey, GraphPermissionType.Application.ToString())
                    .AddAsync(conversationMember);
            }
            catch (Exception ex)
            {
                this.teamServiceLogger.LogError(ex, $"Failed to add user {userAadId} in team {teamId}");
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetPrimaryChannelIdOfTeamAsync(string teamId)
        {
            try
            {
                var channel = await this.graphServiceClient.Teams[teamId].PrimaryChannel
                     .Request()
                     .GetAsync();

                if (channel != null)
                {
                    return channel.Id;
                }

                return null;
            }
            catch (Exception ex)
            {
                this.teamServiceLogger.LogError(ex, $"Failed to get channel Id of team {teamId}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RemoveMemberFromTeamAsync(string teamId, Guid userAadId)
        {
            try
            {
                var teamMembersResponse = await this.graphServiceClient.Teams[teamId].Members.Request()
                .Filter($"(microsoft.graph.aadUserConversationMember/userId eq '{userAadId}')")
                .Header(Athena.Constants.Constants.PermissionTypeKey, GraphPermissionType.Application.ToString())
                .GetAsync();

                await this.graphServiceClient.Teams[teamId].Members[teamMembersResponse.FirstOrDefault()?.Id]
                    .Request().Header(Athena.Constants.Constants.PermissionTypeKey, GraphPermissionType.Application.ToString())
                    .DeleteAsync();
            }
            catch (Exception ex)
            {
                this.teamServiceLogger.LogError(ex, $"Failed to remove user {userAadId} from team {teamId}");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetTeamOwnersAsync(string teamId)
        {
            try
            {
                var owners = await this.graphServiceClient.Groups[teamId].Owners
                    .Request()
                    .Select("id")
                    .GetAsync();

                if (owners == null)
                {
                    return Enumerable.Empty<User>();
                }

                var listOfOwners = new List<User>();

                listOfOwners.AddRange(owners
                    .OfType<User>());

                while (owners.NextPageRequest != null)
                {
                    owners = await owners.NextPageRequest.GetAsync();

                    listOfOwners.AddRange(owners
                    .OfType<User>());
                }

                return listOfOwners;
            }
            catch (Exception ex)
            {
                this.teamServiceLogger.LogError(ex, $"Failed to get team owners of team {teamId}.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AadUserConversationMember>> GetTeamMembersAsync(string teamId)
        {
            try
            {
                var members = await this.graphServiceClient.Teams[teamId].Members
                    .Request()
                    .GetAsync();

                if (members == null)
                {
                    return Enumerable.Empty<AadUserConversationMember>();
                }

                var listOfMembers = new List<AadUserConversationMember>();

                listOfMembers.AddRange(members
                    .OfType<AadUserConversationMember>());

                while (members.NextPageRequest != null)
                {
                    members = await members.NextPageRequest.GetAsync();

                    listOfMembers.AddRange(members
                    .OfType<AadUserConversationMember>());
                }

                return listOfMembers;
            }
            catch (Exception ex)
            {
                this.teamServiceLogger.LogError(ex, $"Failed to get team members of team {teamId}.");
                return Enumerable.Empty<AadUserConversationMember>();
            }
        }
    }
}
