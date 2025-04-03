using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Lease Renewal class, provides an entity for the datamodel to manage leases renewals.
    /// </summary>
    public partial class PimsLeaseRenewal : StandardIdentityBaseAppEntity<long>, IEquatable<PimsLeaseRenewal>,  IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => LeaseRenewalId; set => LeaseRenewalId = value; }

        public override bool Equals(object obj) => Equals(obj as PimsLeaseRenewal);

        public bool Equals(PimsLeaseRenewal other)
        {
            if (other is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return IsExercised == other.IsExercised && CommencementDt == other.CommencementDt && ExpiryDt == other.ExpiryDt;
        }

        public override int GetHashCode() => (IsExercised, CommencementDt, ExpiryDt).GetHashCode();

        #endregion
    }
}
