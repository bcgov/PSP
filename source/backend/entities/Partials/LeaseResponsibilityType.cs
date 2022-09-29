using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseResponsibilityType class, provides an entity for the datamodel to manage a list of lease types.
    /// </summary>
    public partial class PimsLeaseResponsibilityType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseResponsibilityTypeCode; set => LeaseResponsibilityTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeaseResponsibilityType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseResponsibilityType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
