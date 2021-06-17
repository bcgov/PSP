using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyClassificationClassification class, provides an entity for the datamodel to manage a list of property classifications.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_CLASSIFICATION", "PRPCLS")]
    public class PropertyClassification : LookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to property classification.
        /// </summary>
        [Column("PROPERTY_CLASSIFICATION_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Whether this classification is generally visible.
        /// </summary>
        [Column("IS_VISIBLE")]
        public bool IsVisible { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyClassification class.
        /// </summary>
        public PropertyClassification() { }

        /// <summary>
        /// Create a new instance of a PropertyClassification class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isVisible"></param>
        public PropertyClassification(string name, bool isVisible = true) : base(name)
        {
            this.IsVisible = isVisible;
        }
        #endregion
    }
}
