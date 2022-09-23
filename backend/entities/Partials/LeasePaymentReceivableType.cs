using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeasePaymentRvblType class, provides an entity for the datamodel to manage a list of lease payment rvbl types.
    /// </summary>
    public partial class PimsLeasePayRvblType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease payment rvbl type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeasePayRvblTypeCode; set => LeasePayRvblTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeasePaymentRvblType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeasePayRvblType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
