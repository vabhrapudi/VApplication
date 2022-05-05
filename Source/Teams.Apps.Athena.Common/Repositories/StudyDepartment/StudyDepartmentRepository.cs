// <copyright file="StudyDepartmentRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.StudyDepartment
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the study department data stored in the table storage.
    /// </summary>
    public class StudyDepartmentRepository : BaseRepository<StudyDepartmentEntity>, IStudyDepartmentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudyDepartmentRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public StudyDepartmentRepository(
            ILogger<StudyDepartmentRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: StudyDepartmentTableNames.TableName,
                  defaultPartitionKey: StudyDepartmentTableNames.StudyDepartmentPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}