namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// A codified model.
    /// </summary>
    public class CodeTypeModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify code type.
        /// </summary>
        public short Id { get; set; }

        /// <summary>
        /// get/set - The model's code type.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - The description or long name.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The display order.
        /// </summary>
        public int? DisplayOrder { get; set; }
        #endregion
    }
}
