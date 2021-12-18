using System.Collections.Generic;

namespace Pims.Api.Models.BatchUpdate
{
    /// <summary>
    /// Provides a model for batch update requests.
    /// </summary>
    public class Request<T>
    {
        /// <summary>
        /// get/set - The insurance's Id
        /// </summary>
        public IList<EntryModification<T>> Payload { get; set; }
    }
}
