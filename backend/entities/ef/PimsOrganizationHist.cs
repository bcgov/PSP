using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsOrganizationHist
    {
        public long OrganizationHistId { get; set; }
        public DateTime EffectiveDateHist { get; set; }
        public DateTime? EndDateHist { get; set; }
        public long OrganizationId { get; set; }
        public long? PrntOrganizationId { get; set; }
        public short? RegionCode { get; set; }
        public short? DistrictCode { get; set; }
        public string OrganizationTypeCode { get; set; }
        public string OrgIdentifierTypeCode { get; set; }
        public string OrganizationIdentifier { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAlias { get; set; }
        public string IncorporationNumber { get; set; }
        public string Website { get; set; }
        public string Comment { get; set; }
        public bool IsDisabled { get; set; }
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
