using System.Collections.Generic;

namespace Pims.Api.Areas.Autocomplete.Models
{
    /// <summary>
    /// Represents an Autocomplete response returned by the call to AutocompleteRepository.
    /// </summary>
    public class AutocompleteResponseModel
    {
        /// <summary>
        /// get/set - The list of autocomplete predictions.
        /// </summary>
        public IList<AutocompletePrediction> Predictions { get; set; } = new List<AutocompletePrediction>();
    }
}
