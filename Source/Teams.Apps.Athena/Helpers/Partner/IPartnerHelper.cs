// <copyright file="IPartnerHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes helper methods related to Athena partners entity.
    /// </summary>
    public interface IPartnerHelper
    {
        /// <summary>
        /// Gets Athena partners by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of <see cref="PartnerDTO"/>.</returns>
        Task<IEnumerable<PartnerDTO>> GetPartnersByKeywordsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets the partners.
        /// </summary>
        /// <param name="searchParametersDTO">The advanced search parameters.</param>
        /// <returns>The collection of partners.</returns>
        Task<IEnumerable<PartnerDTO>> GetPartnersAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Gets a partner by table Id.
        /// </summary>
        /// <param name="partnerTableId">The partner table Id of the research project to fetch.</param>
        /// <param name="userAadObjectId">The user aad object Id.</param>
        /// <returns>Returns partner entity details.</returns>
        Task<PartnerDTO> GetPartnerByTableIdAsync(string partnerTableId, string userAadObjectId);

        /// <summary>
        /// Stores rating of user for a partner.
        /// </summary>
        /// <param name="partnerTableId">The partner table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="userAadObjectId">User Id who submitted rating.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task RatePartnerAsync(string partnerTableId, int rating, string userAadObjectId);
    }
}
