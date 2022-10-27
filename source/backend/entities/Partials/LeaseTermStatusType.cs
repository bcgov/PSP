using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// InsuranceType class, provides an entity for the datamodel to manage a list of insurance types.
    /// </summary>
    public partial class PimsLeaseTermStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify insurance type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseTermStatusTypeCode; set => LeaseTermStatusTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a InsuranceType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseTermStatusType(string id)
            : this()
        {
            Id = id;
        }
        #endregion

        public static class LeaseTermStatusTypes
        {
            public const string EXER = "EXER";
            public const string NEXER = "NEXER";
        }
    }
}
