// <copyright file="PartnersRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The repository for managing table operations related to partners.
    /// </summary>
    public class PartnersRepository : BaseRepository<PartnerEntity>, IPartnersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartnersRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public PartnersRepository(
            ILogger<PartnersRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 PartnersTableMetadata.TableName,
                 PartnersTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<PartnerEntity> GetPartnerDetailsByPartnerIdAsync(int partnerId)
        {
            var partnerIdFilter = TableQuery.GenerateFilterConditionForInt(
                          nameof(PartnerEntity.PartnerId),
                          QueryComparisons.Equal,
                          partnerId);
            var partner = await this.GetWithFilterAsync(partnerIdFilter);
            return partner.FirstOrDefault();
        }
    }
}
