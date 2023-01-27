namespace Pims.Api.Areas.Autocomplete.Models
{
    public class AutocompletePrediction
    {
        /// <summary>
        /// get/set - The completed term or phrase.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// get/set - An entity ID that can be used to retrieve details about this autocomplete suggestion.
        /// </summary>
        public long Id { get; set; }
    }
}
