// <copyright file="SpecialtyHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories.Specialty;

    /// <summary>
    /// Provides helper methods for managing specialty entity.
    /// </summary>
    public class SpecialtyHelper : ISpecialtyHelper
    {
        /// <summary>
        /// The instance of specialty repository.
        /// </summary>
        private readonly ISpecialtyRepository specialtyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialtyHelper"/> class.
        /// </summary>
        /// <param name="specialtyRepository">The instance of specialty repository.</param>
        public SpecialtyHelper(ISpecialtyRepository specialtyRepository)
        {
            this.specialtyRepository = specialtyRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SpecialtyEntity>> GetSpecialtyDetailsAsync()
        {
            return await this.specialtyRepository.GetAllAsync();
        }
    }
}