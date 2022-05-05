// <copyright file="IAthenaIngestionRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes repository operations related to Athena ingestion.
    /// </summary>
    public interface IAthenaIngestionRepository : IRepository<AthenaIngestionEntity>
    {
        /// <summary>
        /// Gets Athena ingestion details by entity name.
        /// </summary>
        /// <param name="entityName">Entity name.</param>
        /// <returns>Returns Athena ingestion details.</returns>
        Task<AthenaIngestionEntity> GetAthenaIngestionByEntityNameAsync(string entityName);
    }
}
