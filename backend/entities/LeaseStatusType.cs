
using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseStatusType class, provides an entity for the datamodel to manage a list of lease status types.
    /// </summary>
    public partial class PimsLeaseStatusType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease status type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseStatusTypeCode; set => LeaseStatusTypeCode = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseStatusType(string id):this()
        {
            Id = id;
        }
        #endregion
    }

    public static class PimsLeaseStatusTypes
    {
        public const string PAID = "PAID";
        public const string OVERPAID = "OVERPAID";
        public const string PARTIAL = "PARTIAL";
        public const string UNPAID = "UNPAID";
    }
}
