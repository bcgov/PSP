using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table to contain all applicable notes for the PIMS PSP system.
/// </summary>
[Table("PIMS_NOTE")]
public partial class PimsNote
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("NOTE_ID")]
    public long NoteId { get; set; }

    /// <summary>
    /// Contents of the note.
    /// </summary>
    [Required]
    [Column("NOTE_TXT")]
    [StringLength(4000)]
    public string NoteTxt { get; set; }

    /// <summary>
    /// Indicatesd if this note is system-generated.
    /// </summary>
    [Column("IS_SYSTEM_GENERATED")]
    public bool IsSystemGenerated { get; set; }

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

    [InverseProperty("Note")]
    public virtual PimsAcquisitionFileNote PimsAcquisitionFileNote { get; set; }

    [InverseProperty("Note")]
    public virtual ICollection<PimsDispositionFileNote> PimsDispositionFileNotes { get; set; } = new List<PimsDispositionFileNote>();

    [InverseProperty("Note")]
    public virtual ICollection<PimsLeaseNote> PimsLeaseNotes { get; set; } = new List<PimsLeaseNote>();

    [InverseProperty("Note")]
    public virtual ICollection<PimsManagementFileNote> PimsManagementFileNotes { get; set; } = new List<PimsManagementFileNote>();

    [InverseProperty("Note")]
    public virtual ICollection<PimsProjectNote> PimsProjectNotes { get; set; } = new List<PimsProjectNote>();

    [InverseProperty("Note")]
    public virtual ICollection<PimsPropertyNote> PimsPropertyNotes { get; set; } = new List<PimsPropertyNote>();

    [InverseProperty("Note")]
    public virtual ICollection<PimsResearchFileNote> PimsResearchFileNotes { get; set; } = new List<PimsResearchFileNote>();
}
