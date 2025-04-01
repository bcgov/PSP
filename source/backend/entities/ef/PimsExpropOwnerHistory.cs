using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Entity continaing the details regarding actions involving a property owner associated with an expropriation.
/// </summary>
[Table("PIMS_EXPROP_OWNER_HISTORY")]
[Index("AcquisitionFileId", Name = "XPOWNH_ACQUISITION_FILE_ID_IDX")]
[Index("ExpropOwnerHistoryTypeCode", Name = "XPOWNH_EXPROP_OWNER_HISTORY_TYPE_CODE_IDX")]
public partial class PimsExpropOwnerHistory
{
    /// <summary>
    /// Unique auto-generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("EXPROP_OWNER_HISTORY_ID")]
    public long ExpropOwnerHistoryId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQUISITION_FILE file.
    /// </summary>
    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table.
    /// </summary>
    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ORGANIZATION table.
    /// </summary>
    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table.
    /// </summary>
    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_EXPROP_OWNER_HISTORY_TYPE file.
    /// </summary>
    [Required]
    [Column("EXPROP_OWNER_HISTORY_TYPE_CODE")]
    [StringLength(20)]
    public string ExpropOwnerHistoryTypeCode { get; set; }

    /// <summary>
    /// Date of the expropriation owner event.
    /// </summary>
    [Column("EVENT_DT", TypeName = "datetime")]
    public DateTime? EventDt { get; set; }

    /// <summary>
    /// Indicates if the row is inactive.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

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
    [InverseProperty("PimsExpropOwnerHistories")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("ExpropOwnerHistoryTypeCode")]
    [InverseProperty("PimsExpropOwnerHistories")]
    public virtual PimsExpropOwnerHistoryType ExpropOwnerHistoryTypeCodeNavigation { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsExpropOwnerHistories")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsExpropOwnerHistoryPeople")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsExpropOwnerHistoryPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }
}
