// <copyright file="IAthenaPriorityTypesBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Athena priority types blob repository.
    /// </summary>
    public interface IAthenaPriorityTypesBlobRepository : IBlobBaseRepository<IEnumerable<PriorityType>>
    {
    }
}
