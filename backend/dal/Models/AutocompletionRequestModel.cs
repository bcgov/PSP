using System;
using System.Collections.Generic;
using Pims.Core.Extensions;

namespace Pims.Dal.Entities.Models
{
    public class AutocompletionRequestModel
    {
        /// <summary>
        /// get/set - Required. The text to search for. The search text to complete. Must be at least 1 character.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// get/set - Optional. Defaults to 5. The number of autocompleted suggestions to retrieve. The value must be a number between 1 and 100.
        /// </summary>
        public int Top { get; set; } = 5;

        /// <summary>
        /// Creates a new instance of a AutocompletionRequestModel class.
        /// </summary>
        public AutocompletionRequestModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a AutocompletionRequestModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public AutocompletionRequestModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);
            this.Search = filter.GetStringValue(nameof(this.Search));
            this.Top = filter.GetIntValue(nameof(this.Top), 5);
        }

        /// <summary>
        /// Determine if a valid filter was provided.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.Search)
                && (this.Top >= 1 && this.Top <= 100);
        }
    }
}
