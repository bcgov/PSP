using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

[Table("PIMS_PROPERTY_LEASE")]
[Index("LeaseId", Name = "PROPLS_LEASE_ID_IDX")]
[Index("PropertyId", Name = "PROPLS_PROPERTY_ID_IDX")]
[Index("LeaseId", "PropertyId", Name = "PROPLS_PROPERTY_LEASE_TUC", IsUnique = true)]
public partial class PimsPropertyLease
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("PROPERTY_LEASE_ID")]
    public long PropertyLeaseId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY table.
    /// </summary>
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS AREA_UNIT_TYPE table.
    /// </summary>
    [Column("AREA_UNIT_TYPE_CODE")]
    [StringLength(20)]
    public string AreaUnitTypeCode { get; set; }

    /// <summary>
    /// Property/lease name
    /// </summary>
    [Column("NAME")]
    [StringLength(250)]
    public string Name { get; set; }

    /// <summary>
    /// Leased area measurement
    /// </summary>
    [Column("LEASE_AREA")]
    public float? LeaseArea { get; set; }

    /// <summary>
    /// Geospatial location (pin) of property
    /// </summary>
    [Column("LOCATION", TypeName = "geometry")]
    public Geometry Location { get; set; }

    /// <summary>
    /// Specifies the display order of the property (PSP-10521).
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

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
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
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

    [ForeignKey("AreaUnitTypeCode")]
    [InverseProperty("PimsPropertyLeases")]
    public virtual PimsAreaUnitType AreaUnitTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsPropertyLeases")]
    public virtual PimsLease Lease { get; set; }

    [InverseProperty("PropertyLease")]
    public virtual ICollection<PimsPropLeaseCompReq> PimsPropLeaseCompReqs { get; set; } = new List<PimsPropLeaseCompReq>();

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropertyLeases")]
    public virtual PimsProperty Property { get; set; }
}
