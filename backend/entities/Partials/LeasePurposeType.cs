using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeasePurposeType class, provides an entity for the datamodel to manage a list of lease purpose types.
    /// </summary>
    public partial class PimsLeasePurposeType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease purpose type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeasePurposeTypeCode; set => LeasePurposeTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeasePurposeType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeasePurposeType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
