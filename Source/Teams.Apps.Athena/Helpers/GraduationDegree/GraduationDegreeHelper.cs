// <copyright file="GraduationDegreeHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories.GraduationDegree;

    /// <summary>
    /// Provides helper methods for managing graduation degree entity.
    /// </summary>
    public class GraduationDegreeHelper : IGraduationDegreeHelper
    {
        /// <summary>
        /// The instance of graduation degree repository.
        /// </summary>
        private readonly IGraduationDegreeRepository graduationDegreeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraduationDegreeHelper"/> class.
        /// </summary>
        /// <param name="graduationDegreeRepository">The instance of graduation degree repository.</param>
        public GraduationDegreeHelper(IGraduationDegreeRepository graduationDegreeRepository)
        {
            this.graduationDegreeRepository = graduationDegreeRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<GraduationDegree>> GetGraduationDegreesAsync()
        {
            return await this.graduationDegreeRepository.GetAllAsync();
        }
    }
}