using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyImprovement class, provides the many-to-many relationship between leases and tenants.
    /// </summary>
    public partial class PimsPropertyImprovement : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyImprovementId; set => this.PropertyImprovementId = value; }
        #endregion

        #region Constructors
        public PimsPropertyImprovement()
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyImprovement object, initializes with specified arguments.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="improvementType"></param>
        public PimsPropertyImprovement(PimsLease lease, PimsPropertyImprovementType improvementType)
            : this()
        {
            this.LeaseId = lease?.LeaseId ?? throw new ArgumentNullException(nameof(lease));
            this.Lease = lease;
            this.PropertyImprovementTypeCode = improvementType.Id ?? throw new ArgumentNullException(nameof(improvementType));
            this.PropertyImprovementTypeCodeNavigation = improvementType;
        }
        #endregion
    }
}
