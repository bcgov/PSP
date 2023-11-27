using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan
{
    /// <summary>
    /// Represents a query results to the mayan system with type T.
    /// </summary>
    /// <typeparam name="T">The type of the result objects wrapped by this QueryResult.</typeparam>
    public class QueryResult<T>
    {
        /// <summary>
        /// get/set - Total number of results.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// get/set - The results of the query.
        /// </summary>
        [JsonPropertyName("results")]
        public IList<T> Results { get; set; }
    }
}
