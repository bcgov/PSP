using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseInitiatorType class, provides an entity for the datamodel to manage a list of lease types.
    /// </summary>
    public partial class PimsLeaseInitiatorType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease initiator type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseInitiatorTypeCode; set => LeaseInitiatorTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeaseInitiatorType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseInitiatorType(string id)
            : this()
        {
        }
        #endregion
    }
}
