﻿// <copyright file="ICommunityOfInterestBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Community Of Interest blob repository.
    /// </summary>
    public interface ICommunityOfInterestBlobRepository : IBlobBaseRepository<IEnumerable<CommunityOfInterestJson>>
    {
    }
}
