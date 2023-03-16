using System.Collections.Generic;

namespace Pims.Api.Models.BatchUpdate
{
    /// <summary>
    /// Provides a wrapper for batch update replies.
    /// </summary>
    /// <typeparam name="T">The type of the items in the batch.</typeparam>
    public class Reply<T>
    {
        public Reply(IList<T> payload)
        {
            Payload = payload;
        }

        /// <summary>
        /// get/set - The updated payload.
        /// </summary>
        public IList<T> Payload { get; set; }

        /// <summary>
        /// get/set - Batch error messages if any.
        /// </summary>
        public IList<string> ErrorMessages { get; set; } = new List<string>();
    }
}
