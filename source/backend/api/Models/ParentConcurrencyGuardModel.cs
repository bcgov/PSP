namespace Pims.Api.Models
{
    /// <summary>
    /// ParentConcurrencyGuardModel class, provides a model that represents a single page of items.
    /// </summary>
    /// <typeparam name="T">The type of the guarded object.</typeparam>
    public class ParentConcurrencyGuardModel<T>
    {
        /// <summary>
        /// get/set - The model wrapped by this concurrency guard.
        /// </summary>
        public T Payload { get; set; }

        /// <summary>
        /// get/set - The id of the parent entity.
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// get/set - The row version of the parent entity.
        /// </summary>
        public long ParentRowVersion { get; set; }
    }
}
