using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsAddress
    {
        public PimsAddress()
        {
            PimsOrganizationAddresses = new HashSet<PimsOrganizationAddress>();
            PimsPersonAddresses = new HashSet<PimsPersonAddress>();
            PimsProperties = new HashSet<PimsProperty>();
        }

        public long AddressId { get; set; }
        public short? RegionCode { get; set; }
        public short? DistrictCode { get; set; }
        public short ProvinceStateId { get; set; }
        public short? CountryId { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string StreetAddress3 { get; set; }
        public string MunicipalityName { get; set; }
        public string PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Comment { get; set; }
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

        public virtual PimsCountry Country { get; set; }
        public virtual PimsDistrict DistrictCodeNavigation { get; set; }
        public virtual PimsProvinceState ProvinceState { get; set; }
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        public virtual ICollection<PimsOrganizationAddress> PimsOrganizationAddresses { get; set; }
        public virtual ICollection<PimsPersonAddress> PimsPersonAddresses { get; set; }
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
    }
}
