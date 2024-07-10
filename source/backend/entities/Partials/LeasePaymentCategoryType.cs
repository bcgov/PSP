using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeasePaymentCategoryType class, provides an entity for the datamodel to manage a list of lease payment category types.
    /// </summary>
    public partial class PimsLeasePaymentCategoryType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease payment frequency type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeasePaymentCategoryTypeCode; set => LeasePaymentCategoryTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeasePaymentFrequencyType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeasePaymentCategoryType(string id)
            : this()
        {
            Id = id;
        }

        public PimsLeasePaymentCategoryType()
        {
        }
        #endregion
    }
}
