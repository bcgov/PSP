using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IAutocompleteRepository interface, retrieves autocomplete predictions based on the supplied autocomplete request.
    /// </summary>
    public interface IAutocompleteRepository : IRepository
    {
        IEnumerable<PimsOrganization> GetOrganizationPredictions(AutocompletionRequestModel request);
    }
}
