// <copyright file="UserRequestType.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents the request type received from user.
    /// </summary>
    public enum UserRequestType
    {
        /// <summary>
        /// Represents the news article request from user.
        /// </summary>
        NewsArticleRequest,

        /// <summary>
        /// Represents the COI request from user.
        /// </summary>
        CoiRequest,
    }
}