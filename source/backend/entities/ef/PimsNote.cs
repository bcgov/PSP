using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_NOTE")]
    public partial class PimsNote
    {
        public PimsNote()
        {
            PimsLeaseNotes = new HashSet<PimsLeaseNote>();
            PimsProjectNotes = new HashSet<PimsProjectNote>();
            PimsResearchFileNotes = new HashSet<PimsResearchFileNote>();
        }

        [Key]
        [Column("NOTE_ID")]
        public long NoteId { get; set; }
        [Required]
        [Column("NOTE_TXT")]
        [StringLength(4000)]
        public string NoteTxt { get; set; }
        [Required]
        [Column("IS_SYSTEM_GENERATED")]
        public bool? IsSystemGenerated { get; set; }
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
        [InverseProperty(nameof(PimsLeaseNote.Note))]
        public virtual ICollection<PimsLeaseNote> PimsLeaseNotes { get; set; }
        [InverseProperty(nameof(PimsProjectNote.Note))]
        public virtual ICollection<PimsProjectNote> PimsProjectNotes { get; set; }
        [InverseProperty(nameof(PimsResearchFileNote.Note))]
        public virtual ICollection<PimsResearchFileNote> PimsResearchFileNotes { get; set; }
    }
}
