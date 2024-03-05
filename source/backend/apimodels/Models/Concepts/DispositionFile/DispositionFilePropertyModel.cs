using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Property;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFilePropertyModel : BaseConcurrentModel
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
        public DispositionFileModel File { get; set; }

        /// <summary>
        /// get/set - The relationship's disposition file id.
        /// </summary>
        public long FileId { get; set; }

        #endregion
    }
}
