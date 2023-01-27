using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseType class, provides an entity for the datamodel to manage a list of lease types.
    /// </summary>
    public partial class PimsLeaseLicenseType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseLicenseTypeCode; set => LeaseLicenseTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeaseType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseLicenseType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
