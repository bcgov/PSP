namespace Pims.Api.Models.Concepts
{
    public class DispositionFilePropertyModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The descriptive name of the property for this disposition file.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// get/set - The order to display the relationship.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// get/set - The relationship's property.
        /// </summary>
        public PropertyModel Property { get; set; }

        /// <summary>
        /// get/set - The relationship's property id.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - The relationship's disposition file.
        /// </summary>
        public FileModel File { get; set; }

        /// <summary>
        /// get/set - The relationship's disposition file id.
        /// </summary>
        public long FileId { get; set; }

        #endregion
    }
}
