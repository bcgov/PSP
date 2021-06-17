using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyType class, provides an entity for the datamodel to manage a list of property types.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_TYPE", "PRPTYP")]
    public class PropertyType : LookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property type.
        /// </summary>
        [Column("PROPERTY_TYPE_ID")]
        public long Id { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyType class.
        /// </summary>
        public PropertyType() { }

        /// <summary>
        /// Create a new instance of a PropertyType class.
        /// </summary>
        /// <param name="name"></param>
        public PropertyType(string name) : base(name)
        {
        }
        #endregion
    }
}
