namespace Pims.Api.Models.Base
{
    /// <summary>
    /// Provides a type model.
    /// </summary>
    /// <typeparam name="T">The actual type of this TypeModel.</typeparam>
    public class CodeTypeModel<T>
        where T : notnull
    {
        /// <summary>
        /// get/set - Primary key to identify type.
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// get/set - A description of the type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether this code is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The type display order.
        /// </summary>
        public int? DisplayOrder { get; set; }
    }
}
