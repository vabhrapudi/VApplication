// <copyright file="ISponsorHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The helper methods provider for sponsor.
    /// </summary>
    public interface ISponsorHelper
    {
        /// <summary>
        /// Gets the sponsors by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of <see cref="SponsorEntity"/>.</returns>
        Task<IEnumerable<SponsorDTO>> GetSponsorsByKeywordsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets the sponsors.
        /// </summary>
        /// <param name="searchParametersDTO">Advanced search parameters.</param>
        /// <returns>The collection of sponsors.</returns>
        Task<IEnumerable<SponsorDTO>> GetSponsorsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Gets a sponsor by table Id.
        /// </summary>
        /// <param name="sponsorTableId">The sponsor table Id of the research project to fetch.</param>
        /// <param name="userAadObjectId">The user aad object Id.</param>
        /// <returns>Returns sponsor entity details.</returns>
        Task<SponsorDTO> GetSponsorByTableIdAsync(string sponsorTableId, string userAadObjectId);

        /// <summary>
        /// Stores rating of user for a sponsor.
        /// </summary>
        /// <param name="sponsorTableId">The sponsor table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="userAadObjectId">User Id who submitted rating.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task RateSponsorAsync(string sponsorTableId, int rating, string userAadObjectId);
    }
}