using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsPersonOrganizationHist
    {
        public long PersonOrganizationHistId { get; set; }
        public DateTime EffectiveDateHist { get; set; }
        public DateTime? EndDateHist { get; set; }
        public long PersonOrganizationId { get; set; }
        public long? PersonId { get; set; }
        public long? OrganizationId { get; set; }
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
    }
}
