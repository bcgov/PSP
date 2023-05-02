using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// AutocompleteRepository retrieves autocomplete predictions based on the supplied autocomplete request.
    /// </summary>
    public class AutocompleteRepository : BaseRepository, IAutocompleteRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AutocompleteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AutocompleteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LookupRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get organization autocomplete predictions based on the supplied autocomplete request.
        /// </summary>
        public IEnumerable<PimsOrganization> GetOrganizationPredictions(AutocompletionRequestModel request)
        {
            return this.Context.PimsOrganizations.AsNoTracking()
                .Where(o => o.IsDisabled != true)
                .Where(o => EF.Functions.Like(o.OrganizationName, $"%{request.Search}%") || EF.Functions.Like(o.OrganizationAlias, $"%{request.Search}%"))
                .OrderBy(a => a.OrganizationName)
                .Take(request.Top)
                .ToArray();
        }
        #endregion
    }
}
