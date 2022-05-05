// <copyright file="OrganizationHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories.Organization;

    /// <summary>
    /// Provides helper methods for managing organization entity.
    /// </summary>
    public class OrganizationHelper : IOrganizationHelper
    {
        /// <summary>
        /// The instance of organization repository.
        /// </summary>
        private readonly IOrganizationRepository organizationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationHelper"/> class.
        /// </summary>
        /// <param name="organizationRepository">The instance of organization repository.</param>
        public OrganizationHelper(IOrganizationRepository organizationRepository)
        {
            this.organizationRepository = organizationRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OrganizationEntity>> GetOrganizationsAsync()
        {
            return await this.organizationRepository.GetAllAsync();
        }
    }
}