using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsOrganization
    {
        public PimsOrganization()
        {
            InversePrntOrganization = new HashSet<PimsOrganization>();
            PimsAccessRequestOrganizations = new HashSet<PimsAccessRequestOrganization>();
            PimsContactMethods = new HashSet<PimsContactMethod>();
            PimsInsurances = new HashSet<PimsInsurance>();
            PimsLeaseTenants = new HashSet<PimsLeaseTenant>();
            PimsOrganizationAddresses = new HashSet<PimsOrganizationAddress>();
            PimsPersonOrganizations = new HashSet<PimsPersonOrganization>();
            PimsProperties = new HashSet<PimsProperty>();
            PimsPropertyOrganizations = new HashSet<PimsPropertyOrganization>();
            PimsUserOrganizations = new HashSet<PimsUserOrganization>();
        }

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

        public virtual PimsDistrict DistrictCodeNavigation { get; set; }
        public virtual PimsOrgIdentifierType OrgIdentifierTypeCodeNavigation { get; set; }
        public virtual PimsOrganizationType OrganizationTypeCodeNavigation { get; set; }
        public virtual PimsOrganization PrntOrganization { get; set; }
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        public virtual ICollection<PimsOrganization> InversePrntOrganization { get; set; }
        public virtual ICollection<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; }
        public virtual ICollection<PimsContactMethod> PimsContactMethods { get; set; }
        public virtual ICollection<PimsInsurance> PimsInsurances { get; set; }
        public virtual ICollection<PimsLeaseTenant> PimsLeaseTenants { get; set; }
        public virtual ICollection<PimsOrganizationAddress> PimsOrganizationAddresses { get; set; }
        public virtual ICollection<PimsPersonOrganization> PimsPersonOrganizations { get; set; }
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
        public virtual ICollection<PimsPropertyOrganization> PimsPropertyOrganizations { get; set; }
        public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; }
    }
}
