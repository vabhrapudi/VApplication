// <copyright file="PriorityMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The mapper methods related to priority.
    /// </summary>
    public class PriorityMapper : IPriorityMapper
    {
        private const string KeywordIdsSeparator = " ";

        /// <inheritdoc/>
        public PriorityEntity MapForCreateModel(PriorityDTO priorityDTO, string userAadId)
        {
            priorityDTO = priorityDTO ?? throw new ArgumentNullException(nameof(priorityDTO));
            userAadId = userAadId ?? throw new ArgumentNullException(nameof(userAadId));

            return new PriorityEntity
            {
                Id = Guid.NewGuid().ToString(),
                Title = priorityDTO.Title,
                Description = priorityDTO.Description,
                Keywords = string.Join(KeywordIdsSeparator, priorityDTO.Keywords),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userAadId.ToString(),
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userAadId.ToString(),
                Type = priorityDTO.Type,
            };
        }

        /// <inheritdoc/>
        public PriorityEntity MapForUpdateModel(PriorityDTO priorityDTO, PriorityEntity priorityEntity, string userAadId)
        {
            priorityDTO = priorityDTO ?? throw new ArgumentNullException(nameof(priorityDTO));
            priorityEntity = priorityEntity ?? throw new ArgumentNullException(nameof(priorityEntity));
            userAadId = userAadId ?? throw new ArgumentNullException(nameof(userAadId));

            priorityEntity.Type = priorityDTO.Type;
            priorityEntity.Title = priorityDTO.Title;
            priorityEntity.Keywords = string.Join(KeywordIdsSeparator, priorityDTO.Keywords);
            priorityEntity.Description = priorityDTO.Description;
            priorityEntity.UpdatedAt = DateTime.UtcNow;
            priorityEntity.UpdatedBy = userAadId.ToString();

            return priorityEntity;
        }

        /// <inheritdoc/>
        public PriorityDTO MapForViewModel(PriorityEntity priorityEntity)
        {
            priorityEntity = priorityEntity ?? throw new ArgumentNullException(nameof(priorityEntity));

            return new PriorityDTO
            {
                Id = priorityEntity.Id,
                Type = priorityEntity.Type,
                Title = priorityEntity.Title,
                Description = priorityEntity.Description,
                Keywords = string.IsNullOrWhiteSpace(priorityEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(priorityEntity.Keywords.Split(KeywordIdsSeparator), int.Parse),
            };
        }
    }
}
