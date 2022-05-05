// <copyright file="IKeywordMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to keywords.
    /// </summary>
    public interface IKeywordMapper
    {
        /// <summary>
        /// Map for view model.
        /// </summary>
        /// <param name="keywordEntity">The keyword entity model.</param>
        /// <returns>The keyword view model.</returns>
        KeywordDTO MapForViewModel(KeywordEntity keywordEntity);
    }
}
