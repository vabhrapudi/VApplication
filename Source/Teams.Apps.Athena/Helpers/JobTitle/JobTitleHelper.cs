// <copyright file="JobTitleHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provide helper method associated with job title operation.
    /// </summary>
    public class JobTitleHelper : IJobTitleHelper
    {
        private readonly IJobTitleBlobRepository jobTitleBlobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobTitleHelper"/> class.
        /// </summary>
        /// <param name="jobTitleBlobRepository">The instance of <see cref="JobTitleBlobRepository"/> class.</param>
        public JobTitleHelper(IJobTitleBlobRepository jobTitleBlobRepository)
        {
            this.jobTitleBlobRepository = jobTitleBlobRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<JobTitle>> GetJobTitlesAsync()
        {
            return await this.jobTitleBlobRepository.GetBlobJsonFileContentAsync(JobTitleBlobMetadata.FileName);
        }
    }
}
