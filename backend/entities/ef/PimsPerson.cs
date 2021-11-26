using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsPerson
    {
        public PimsPerson()
        {
            PimsContactMethods = new HashSet<PimsContactMethod>();
            PimsInsuranceBctfaRiskMgmtContacts = new HashSet<PimsInsurance>();
            PimsInsuranceInsurerContacts = new HashSet<PimsInsurance>();
            PimsInsuranceMotiRiskMgmtContacts = new HashSet<PimsInsurance>();
            PimsLeaseTenants = new HashSet<PimsLeaseTenant>();
            PimsLeases = new HashSet<PimsLease>();
            PimsPersonAddresses = new HashSet<PimsPersonAddress>();
            PimsPersonOrganizations = new HashSet<PimsPersonOrganization>();
            PimsProperties = new HashSet<PimsProperty>();
            PimsUsers = new HashSet<PimsUser>();
        }

        public long PersonId { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string NameSuffix { get; set; }
        public string PreferredName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Comment { get; set; }
        public bool? IsDisabled { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserid { get; set; }
        public Guid? AppCreateUserGuid { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public Guid? AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual ICollection<PimsContactMethod> PimsContactMethods { get; set; }
        public virtual ICollection<PimsInsurance> PimsInsuranceBctfaRiskMgmtContacts { get; set; }
        public virtual ICollection<PimsInsurance> PimsInsuranceInsurerContacts { get; set; }
        public virtual ICollection<PimsInsurance> PimsInsuranceMotiRiskMgmtContacts { get; set; }
        public virtual ICollection<PimsLeaseTenant> PimsLeaseTenants { get; set; }
        public virtual ICollection<PimsLease> PimsLeases { get; set; }
        public virtual ICollection<PimsPersonAddress> PimsPersonAddresses { get; set; }
        public virtual ICollection<PimsPersonOrganization> PimsPersonOrganizations { get; set; }
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
        public virtual ICollection<PimsUser> PimsUsers { get; set; }
    }
}
