using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsLeaseTenant
    {
        public long LeaseTenantId { get; set; }
        public long LeaseId { get; set; }
        public long? PersonId { get; set; }
        public long? OrganizationId { get; set; }
        public string LessorTypeCode { get; set; }
        public string Note { get; set; }
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

        public virtual PimsLease Lease { get; set; }
        public virtual PimsLessorType LessorTypeCodeNavigation { get; set; }
        public virtual PimsOrganization Organization { get; set; }
        public virtual PimsPerson Person { get; set; }
    }
}
