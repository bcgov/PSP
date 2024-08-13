using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseStakeholderType class, provides an entity for the datamodel to manage a list of Lease Stakeholder types.
    /// </summary>
    public partial class PimsLeaseStakeholderType : IBaseEntity, ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify the Lease Stakeholder type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseStakeholderTypeCode; set => LeaseStakeholderTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsLeaseStakeholderType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseStakeholderType(string id)
            : this()
        {
            Id = id;
        }

        public PimsLeaseStakeholderType()
        {
        }
        #endregion
    }
}
