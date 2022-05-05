// <copyright file="QuickAccessMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// A model class that contains methods related to quick access entity model mappings.
    /// </summary>
    public class QuickAccessMapper : IQuickAccessMapper
    {
        /// <inheritdoc/>
        public QuickAccessEntity MapForCreateModel(QuickAccessItemCreateDTO quickAccessItem, string userId)
        {
            return new QuickAccessEntity
            {
                QuickAccessItemId = Guid.NewGuid().ToString(),
                UserId = userId,
                TaxonomyId = quickAccessItem.TaxonomyId,
                ParentId = quickAccessItem.ParentId,
                NodeTypeId = quickAccessItem.NodeTypeId,
            };
        }
    }
}