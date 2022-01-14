using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IAutocompleteService interface, retrieves autocomplete predictions based on the supplied autocomplete request.
    /// </summary>
    public interface IAutocompleteService : IService
    {
        IEnumerable<PimsOrganization> GetOrganizationPredictions(AutocompletionRequestModel request);
    }
}
