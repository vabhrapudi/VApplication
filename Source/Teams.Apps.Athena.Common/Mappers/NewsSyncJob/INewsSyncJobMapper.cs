// <copyright file="INewsSyncJobMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Mappers
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes mapper methods related to news sync job.
    /// </summary>
    public interface INewsSyncJobMapper
    {
        /// <summary>
        /// Maps news from news json model to news entity model in case of new record.
        /// </summary>
        /// <param name="newsJsonModel">The news json model.</param>
        /// <returns>The news entity model.</returns>
        NewsEntity MapForCreateModel(NewsJsonModel newsJsonModel);

        /// <summary>
        /// Maps news from news json model to news entity model in case of existing record for update.
        /// </summary>
        /// <param name="newsEntity">The newx entity model.</param>
        /// <param name="newsJsonModel">The news json model.</param>
        /// <returns>The news entity model.</returns>
        NewsEntity MapForUpdateModel(NewsEntity newsEntity, NewsJsonModel newsJsonModel);
    }
}
