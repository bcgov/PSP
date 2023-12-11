using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyPurposeType class, provides an entity for the datamodel to manage a list of property management purposes.
    /// </summary>
    public partial class PimsPropertyPurposeType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify property management purpose type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyPurposeTypeCode; set => PropertyPurposeTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsPropertyPurposeType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyPurposeType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
