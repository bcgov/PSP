using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeasePaymentFrequencyType class, provides an entity for the datamodel to manage a list of lease payment frequency types.
    /// </summary>
    public partial class PimsLeasePmtFreqType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease payment frequency type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeasePmtFreqTypeCode; set => LeasePmtFreqTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeasePaymentFrequencyType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeasePmtFreqType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
