using Pims.Api.Constants;

namespace Pims.Api.Models.BatchUpdate
{
    /// <summary>
    /// Model that wraps a modification entry.
    /// </summary>
    public class EntryModification<T>
    {
        /// <summary>
        /// get/set - The entry to be modified.
        /// </summary>
        public T Entry { get; set; }

        /// <summary>
        /// get/set - The update operation type.
        /// </summary>
        public UpdateType Operation { get; set; }
    }
}
