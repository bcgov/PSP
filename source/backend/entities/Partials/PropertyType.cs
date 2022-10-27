using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyType class, provides an entity for the datamodel to manage a list of property types.
    /// </summary>
    public partial class PimsPropertyType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify property type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyTypeCode; set => PropertyTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PropertyType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
