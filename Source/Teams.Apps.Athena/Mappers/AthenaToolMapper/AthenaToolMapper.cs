// <copyright file="AthenaToolMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related Athena tool entity model mappings.
    /// </summary>
    public class AthenaToolMapper : IAthenaToolMapper
    {
        private const string KeywordsSeparator = " ";

        /// <inheritdoc/>
        public AthenaToolDTO MapForViewModel(AthenaToolEntity athenaToolEntity)
        {
            if (athenaToolEntity == null)
            {
                throw new ArgumentNullException(nameof(athenaToolEntity));
            }

            return new AthenaToolDTO
            {
                TableId = athenaToolEntity.TableId,
                ToolId = athenaToolEntity.ToolId,
                NodeTypeId = athenaToolEntity.NodeTypeId,
                SecurityLevel = athenaToolEntity.SecurityLevel,
                Description = athenaToolEntity.Description,
                Title = athenaToolEntity.Title,
                Keywords = string.IsNullOrWhiteSpace(athenaToolEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(athenaToolEntity.Keywords.Split(KeywordsSeparator), int.Parse),
                SubmitterId = athenaToolEntity.SubmitterId,
                Manufacturer = athenaToolEntity.Manufacturer,
                UsageLicensing = athenaToolEntity.UsageLicensing,
                UserComments = athenaToolEntity.UserComments,
                Website = athenaToolEntity.Website,
            };
        }
    }
}
