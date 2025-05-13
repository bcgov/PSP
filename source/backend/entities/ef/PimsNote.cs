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
