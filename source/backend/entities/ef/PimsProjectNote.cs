using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROJECT_NOTE")]
    [Index(nameof(NoteId), Name = "PRJNOT_NOTE_ID_IDX")]
    [Index(nameof(ProjectId), Name = "PRJNOT_PROJECT_ID_IDX")]
    [Index(nameof(NoteId), nameof(ProjectId), Name = "PRJNOT_PROJECT_NOTE_TUC", IsUnique = true)]
    public partial class PimsProjectNote
    {
        [Key]
        [Column("PROJECT_NOTE_ID")]
        public long ProjectNoteId { get; set; }
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }
        [Column("NOTE_ID")]
        public long NoteId { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [ForeignKey(nameof(NoteId))]
        [InverseProperty(nameof(PimsNote.PimsProjectNotes))]
        public virtual PimsNote Note { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(PimsProject.PimsProjectNotes))]
        public virtual PimsProject Project { get; set; }
    }
}
