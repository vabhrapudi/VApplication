// <copyright file="KeywordMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides mapper methods related to keywords.
    /// </summary>
    public class KeywordMapper : IKeywordMapper
    {
        /// <inheritdoc/>
        public KeywordDTO MapForViewModel(KeywordEntity keywordEntity)
        {
            keywordEntity = keywordEntity ?? throw new ArgumentNullException(nameof(keywordEntity));

            return new KeywordDTO
            {
                KeywordId = keywordEntity.KeywordId,
                Title = keywordEntity.Title,
            };
        }
    }
}
