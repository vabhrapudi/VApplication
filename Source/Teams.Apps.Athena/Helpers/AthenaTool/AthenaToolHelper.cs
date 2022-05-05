// <copyright file="AthenaToolHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide helper method associated with Athena tool entity operation.
    /// </summary>
    public class AthenaToolHelper : IAthenaToolHelper
    {
        private readonly IAthenaToolsSearchServices athenaToolsSearchServices;
        private readonly IAthenaToolMapper athenaToolMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaToolHelper"/> class.
        /// </summary>
        /// <param name="athenaToolsSearchServices">The instance of <see cref="AthenaToolsSearchServices"/> class.</param>
        /// <param name="athenaToolMapper">The instance of <see cref="AthenaToolMapper"/> class.</param>
        public AthenaToolHelper(
            IAthenaToolsSearchServices athenaToolsSearchServices,
            IAthenaToolMapper athenaToolMapper)
        {
            this.athenaToolsSearchServices = athenaToolsSearchServices;
            this.athenaToolMapper = athenaToolMapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AthenaToolDTO>> GetAthenaToolsAsync(SearchParametersDTO searchParametersDTO)
        {
            var athenaTools = await this.athenaToolsSearchServices.GetAthenaToolsAsync(searchParametersDTO);
            return athenaTools.Select(x => this.athenaToolMapper.MapForViewModel(x));
        }
    }
}
