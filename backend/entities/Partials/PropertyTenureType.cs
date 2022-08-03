using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyTenureType class, provides an entity for the datamodel to manage a list of property tenure types.
    /// </summary>
    public partial class PimsPropertyTenureType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify property tenure type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyTenureTypeCode; set => PropertyTenureTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PropertyTenureType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyTenureType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
