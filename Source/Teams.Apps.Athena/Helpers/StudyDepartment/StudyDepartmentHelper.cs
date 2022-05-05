// <copyright file="StudyDepartmentHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories.StudyDepartment;

    /// <summary>
    /// Provides helper methods for study department entity.
    /// </summary>
    public class StudyDepartmentHelper : IStudyDepartmentHelper
    {
        /// <summary>
        /// The instance of study department repository.
        /// </summary>
        private readonly IStudyDepartmentRepository studyDepartmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudyDepartmentHelper"/> class.
        /// </summary>
        /// <param name="studyDepartmentRepository">The instance of study department repository.</param>
        public StudyDepartmentHelper(IStudyDepartmentRepository studyDepartmentRepository)
        {
            this.studyDepartmentRepository = studyDepartmentRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StudyDepartmentEntity>> GetStudyDepartmentsAsync()
        {
            return await this.studyDepartmentRepository.GetAllAsync();
        }
    }
}