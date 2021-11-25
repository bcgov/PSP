using System;
using System.Collections.Generic;
using System.Linq;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// Lease class, provides an entity for the datamodel to manage leases.
    /// </summary>
    public partial class PimsLease : IBaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - A collection of properties.
        /// </summary>
        public ICollection<PimsProperty> GetProperties() => PimsPropertyLeases?.Select(pl => pl.Property).ToArray();

        /// <summary>
        /// get/set - A collection of Persons associated to this Lease
        /// </summary>
        public ICollection<PimsPerson> GetPersons() => PimsLeaseTenants?.Where(lt => lt.Person != null).Select(lt => lt.Person).ToArray();

        /// <summary>
        /// get/set - A collection of Organizations associated to this Lease
        /// </summary>
        public ICollection<PimsOrganization> GetOrganizations() => PimsLeaseTenants?.Where(lt => lt.Organization != null).Select(lt => lt.Organization).ToArray();

        /// <summary>
        /// get/set - A collection of Improvements associated to this Lease
        /// </summary>
        public ICollection<PimsPropertyImprovement> GetImprovements() => PimsPropertyLeases?.SelectMany(pl => pl.PimsPropertyImprovements).ToArray();
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a Lease class.
        /// </summary>
        /// <param name="purposeType"></param>
        /// <param name="statusType"></param>
        /// <param name="paymentFrequencyType"></param>
        public PimsLease(PimsLeasePurposeType purposeType, PimsLeasePmtFreqType paymentFrequencyType)
        {
            this.LeasePurposeTypeCode = purposeType?.LeasePurposeTypeCode ?? throw new ArgumentNullException(nameof(purposeType));
            this.LeasePurposeTypeCodeNavigation = purposeType;
            this.LeasePmtFreqTypeCode = paymentFrequencyType?.LeasePmtFreqTypeCode ?? throw new ArgumentNullException(nameof(paymentFrequencyType));
            this.LeasePmtFreqTypeCodeNavigation = paymentFrequencyType;
        }
        #endregion
    }
}
