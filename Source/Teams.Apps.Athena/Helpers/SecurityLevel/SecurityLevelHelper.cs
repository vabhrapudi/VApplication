// <copyright file="SecurityLevelHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provides helper methods associated with security levels operations.
    /// </summary>
    public class SecurityLevelHelper : ISecurityLevelHelper
    {
        private readonly ISecurityLevelBlobRepository securityLevelBlobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityLevelHelper"/> class.
        /// </summary>
        /// <param name="securityLevelBlobRepository">The instance of <see cref="ISecurityLevelBlobRepository"/> class.</param>
        public SecurityLevelHelper(
            ISecurityLevelBlobRepository securityLevelBlobRepository)
        {
            this.securityLevelBlobRepository = securityLevelBlobRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SecurityLevels>> GetSecurityLevelsAsync()
        {
            return await this.securityLevelBlobRepository.GetBlobJsonFileContentAsync(SecurityLevelBlobMetadata.FileName);
        }
    }
}