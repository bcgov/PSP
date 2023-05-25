using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ADDRESS")]
    [Index(nameof(CountryId), Name = "ADDRSS_COUNTRY_ID_IDX")]
    [Index(nameof(DistrictCode), Name = "ADDRSS_DISTRICT_CODE_IDX")]
    [Index(nameof(MunicipalityName), Name = "ADDRSS_MUNICIPALITY_NAME_IDX")]
    [Index(nameof(ProvinceStateId), Name = "ADDRSS_PROVINCE_STATE_ID_IDX")]
    [Index(nameof(RegionCode), Name = "ADDRSS_REGION_CODE_IDX")]
    [Index(nameof(StreetAddress1), Name = "ADDRSS_STREET_ADDRESS_1_IDX")]
    public partial class PimsAddress
    {
        public PimsAddress()
        {
            PimsAcquisitionOwners = new HashSet<PimsAcquisitionOwner>();
            PimsOrganizationAddresses = new HashSet<PimsOrganizationAddress>();
            PimsPersonAddresses = new HashSet<PimsPersonAddress>();
            PimsProperties = new HashSet<PimsProperty>();
        }

        [Key]
        [Column("ADDRESS_ID")]
        public long AddressId { get; set; }
        [Column("REGION_CODE")]
        public short? RegionCode { get; set; }
        [Column("DISTRICT_CODE")]
        public short? DistrictCode { get; set; }
        [Column("PROVINCE_STATE_ID")]
        public short? ProvinceStateId { get; set; }
        [Column("COUNTRY_ID")]
        public short? CountryId { get; set; }
        [Column("STREET_ADDRESS_1")]
        [StringLength(200)]
        public string StreetAddress1 { get; set; }
        [Column("STREET_ADDRESS_2")]
        [StringLength(200)]
        public string StreetAddress2 { get; set; }
        [Column("STREET_ADDRESS_3")]
        [StringLength(200)]
        public string StreetAddress3 { get; set; }
        [Column("MUNICIPALITY_NAME")]
        [StringLength(200)]
        public string MunicipalityName { get; set; }
        [Column("POSTAL_CODE")]
        [StringLength(20)]
        public string PostalCode { get; set; }
        [Column("OTHER_COUNTRY")]
        [StringLength(200)]
        public string OtherCountry { get; set; }
        [Column("LATITUDE", TypeName = "numeric(8, 6)")]
        public decimal? Latitude { get; set; }
        [Column("LONGITUDE", TypeName = "numeric(9, 6)")]
        public decimal? Longitude { get; set; }
        [Column("COMMENT")]
        [StringLength(2000)]
        public string Comment { get; set; }
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long ConcurrencyControlNumber { get; set; }
        [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppCreateTimestamp { get; set; }
        [Required]
        [Column("APP_CREATE_USERID")]
        [StringLength(30)]
        public string AppCreateUserid { get; set; }
        [Column("APP_CREATE_USER_GUID")]
        public Guid? AppCreateUserGuid { get; set; }
        [Required]
        [Column("APP_CREATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppCreateUserDirectory { get; set; }
        [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppLastUpdateTimestamp { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string AppLastUpdateUserid { get; set; }
        [Column("APP_LAST_UPDATE_USER_GUID")]
        public Guid? AppLastUpdateUserGuid { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppLastUpdateUserDirectory { get; set; }
        [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime DbCreateTimestamp { get; set; }
        [Required]
        [Column("DB_CREATE_USERID")]
        [StringLength(30)]
        public string DbCreateUserid { get; set; }
        [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime DbLastUpdateTimestamp { get; set; }
        [Required]
        [Column("DB_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string DbLastUpdateUserid { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(PimsCountry.PimsAddresses))]
        public virtual PimsCountry Country { get; set; }
        [ForeignKey(nameof(DistrictCode))]
        [InverseProperty(nameof(PimsDistrict.PimsAddresses))]
        public virtual PimsDistrict DistrictCodeNavigation { get; set; }
        [ForeignKey(nameof(ProvinceStateId))]
        [InverseProperty(nameof(PimsProvinceState.PimsAddresses))]
        public virtual PimsProvinceState ProvinceState { get; set; }
        [ForeignKey(nameof(RegionCode))]
        [InverseProperty(nameof(PimsRegion.PimsAddresses))]
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsAcquisitionOwner.Address))]
        public virtual ICollection<PimsAcquisitionOwner> PimsAcquisitionOwners { get; set; }
        [InverseProperty(nameof(PimsOrganizationAddress.Address))]
        public virtual ICollection<PimsOrganizationAddress> PimsOrganizationAddresses { get; set; }
        [InverseProperty(nameof(PimsPersonAddress.Address))]
        public virtual ICollection<PimsPersonAddress> PimsPersonAddresses { get; set; }
        [InverseProperty(nameof(PimsProperty.Address))]
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
    }
}
