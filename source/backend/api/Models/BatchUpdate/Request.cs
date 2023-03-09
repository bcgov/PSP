using System.Collections.Generic;

namespace Pims.Api.Models.BatchUpdate
{
    /// <summary>
    /// Provides a model for batch update requests.
    /// </summary>
    /// <typeparam name="T">The type of the objects within the batch.</typeparam>
    public class Request<T>
    {
        /// <summary>
        /// get/set - The batch request payload.
        /// </summary>
        public IList<EntryModification<T>> Payload { get; set; }
    }
}
