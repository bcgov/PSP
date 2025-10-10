using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_ADDRESS")]
[Index("CountryId", Name = "ADDRSS_COUNTRY_ID_IDX")]
[Index("DistrictCode", Name = "ADDRSS_DISTRICT_CODE_IDX")]
[Index("MunicipalityName", Name = "ADDRSS_MUNICIPALITY_NAME_IDX")]
[Index("ProvinceStateId", Name = "ADDRSS_PROVINCE_STATE_ID_IDX")]
[Index("RegionCode", Name = "ADDRSS_REGION_CODE_IDX")]
[Index("StreetAddress1", Name = "ADDRSS_STREET_ADDRESS_1_IDX")]
public partial class PimsAddress
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("ADDRESS_ID")]
    public long AddressId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_REGION table.
    /// </summary>
    [Column("REGION_CODE")]
    public short? RegionCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_DISTRICT table.
    /// </summary>
    [Column("DISTRICT_CODE")]
    public short? DistrictCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROVINCE_STATE table.
    /// </summary>
    [Column("PROVINCE_STATE_ID")]
    public short? ProvinceStateId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_COUNTRY table.
    /// </summary>
    [Column("COUNTRY_ID")]
    public short? CountryId { get; set; }

    /// <summary>
    /// Line 1 of the street address.
    /// </summary>
    [Column("STREET_ADDRESS_1")]
    [StringLength(200)]
    public string StreetAddress1 { get; set; }

    /// <summary>
    /// Line 2 of the street address.
    /// </summary>
    [Column("STREET_ADDRESS_2")]
    [StringLength(200)]
    public string StreetAddress2 { get; set; }

    /// <summary>
    /// Line 3 of the street address.
    /// </summary>
    [Column("STREET_ADDRESS_3")]
    [StringLength(200)]
    public string StreetAddress3 { get; set; }

    /// <summary>
    /// Name of the address&apos; municipality.
    /// </summary>
    [Column("MUNICIPALITY_NAME")]
    [StringLength(200)]
    public string MunicipalityName { get; set; }

    /// <summary>
    /// Postal code for the address.
    /// </summary>
    [Column("POSTAL_CODE")]
    [StringLength(20)]
    public string PostalCode { get; set; }

    /// <summary>
    /// Other country not listed in drop-down list.
    /// </summary>
    [Column("OTHER_COUNTRY")]
    [StringLength(200)]
    public string OtherCountry { get; set; }

    /// <summary>
    /// Latitude of the address.
    /// </summary>
    [Column("LATITUDE", TypeName = "numeric(8, 6)")]
    public decimal? Latitude { get; set; }

    /// <summary>
    /// longitude of the address.
    /// </summary>
    [Column("LONGITUDE", TypeName = "numeric(9, 6)")]
    public decimal? Longitude { get; set; }

    /// <summary>
    /// Comments on the address.
    /// </summary>
    [Column("COMMENT")]
    [StringLength(2000)]
    public string Comment { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("PimsAddresses")]
    public virtual PimsCountry Country { get; set; }

    [ForeignKey("DistrictCode")]
    [InverseProperty("PimsAddresses")]
    public virtual PimsDistrict DistrictCodeNavigation { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<PimsAcquisitionOwner> PimsAcquisitionOwners { get; set; } = new List<PimsAcquisitionOwner>();

    [InverseProperty("Address")]
    public virtual ICollection<PimsOrganizationAddress> PimsOrganizationAddresses { get; set; } = new List<PimsOrganizationAddress>();

    [InverseProperty("Address")]
    public virtual ICollection<PimsPersonAddress> PimsPersonAddresses { get; set; } = new List<PimsPersonAddress>();

    [InverseProperty("Address")]
    public virtual ICollection<PimsProperty> PimsProperties { get; set; } = new List<PimsProperty>();

    [ForeignKey("ProvinceStateId")]
    [InverseProperty("PimsAddresses")]
    public virtual PimsProvinceState ProvinceState { get; set; }

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsAddresses")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }
}
