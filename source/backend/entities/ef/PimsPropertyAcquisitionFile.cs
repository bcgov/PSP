using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
    [Key]
    [Column("PROPERTY_ACQUISITION_FILE_ID")]
    public long PropertyAcquisitionFileId { get; set; }

    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Descriptive reference for the property associated with the acquisition file.
    /// </summary>
    [Column("PROPERTY_NAME")]
    [StringLength(500)]
    public string PropertyName { get; set; }

    /// <summary>
    /// Force the display order of the codes.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

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

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsPropertyAcquisitionFiles")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [InverseProperty("PropertyAcquisitionFile")]
    public virtual ICollection<PimsInthldrPropInterest> PimsInthldrPropInterests { get; set; } = new List<PimsInthldrPropInterest>();

    [InverseProperty("PropertyAcquisitionFile")]
    public virtual ICollection<PimsTake> PimsTakes { get; set; } = new List<PimsTake>();

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropertyAcquisitionFiles")]
    public virtual PimsProperty Property { get; set; }
}
