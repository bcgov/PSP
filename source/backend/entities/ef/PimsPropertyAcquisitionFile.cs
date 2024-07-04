using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

/// <summary>
/// Associates a property with an acquisition file.
/// </summary>
[Table("PIMS_PROPERTY_ACQUISITION_FILE")]
[Index("AcquisitionFileId", Name = "PRACQF_ACQUISITION_FILE_ID_IDX")]
[Index("PropertyId", Name = "PRACQF_PROPERTY_ID_IDX")]
[Index("PropertyId", "AcquisitionFileId", Name = "PRACQF_PROP_ACQ_TUC", IsUnique = true)]
public partial class PimsPropertyAcquisitionFile
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("PROPERTY_ACQUISITION_FILE_ID")]
    public long PropertyAcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key to the ACQUISTION_FILE table.
    /// </summary>
    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key to the PROPERTY table.
    /// </summary>
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Descriptive reference for the property associated with the acquisition file.
    /// </summary>
    [Column("PROPERTY_NAME")]
    [StringLength(500)]
    public string PropertyName { get; set; }

    /// <summary>
    /// Geospatial location (pin) of property
    /// </summary>
    [Column("LOCATION", TypeName = "geometry")]
    public Geometry Location { get; set; }

    /// <summary>
    /// Force the display order of the codes.
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

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsPropertyAcquisitionFiles")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [InverseProperty("PropertyAcquisitionFile")]
    public virtual ICollection<PimsInthldrPropInterest> PimsInthldrPropInterests { get; set; } = new List<PimsInthldrPropInterest>();

    [InverseProperty("PropertyAcquisitionFile")]
    public virtual ICollection<PimsPropAcqFlCompReq> PimsPropAcqFlCompReqs { get; set; } = new List<PimsPropAcqFlCompReq>();

    [InverseProperty("PropertyAcquisitionFile")]
    public virtual ICollection<PimsTake> PimsTakes { get; set; } = new List<PimsTake>();

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropertyAcquisitionFiles")]
    public virtual PimsProperty Property { get; set; }
}
