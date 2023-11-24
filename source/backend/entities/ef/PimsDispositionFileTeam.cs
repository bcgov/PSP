using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DISPOSITION_FILE_TEAM")]
    [Index(nameof(DispositionFileId), Name = "DSPFTM_DISPOSITION_FILE_ID_IDX")]
    [Index(nameof(DspFlTeamProfileTypeCode), Name = "DSPFTM_DSP_FL_TEAM_PROFILE_TYPE_CODE_IDX")]
    [Index(nameof(DispositionFileId), nameof(DspFlTeamProfileTypeCode), Name = "DSPFTM_FILE_PROFILE_TUC", IsUnique = true)]
    [Index(nameof(OrganizationId), Name = "DSPFTM_ORGANIZATION_ID_IDX")]
    [Index(nameof(PersonId), Name = "DSPFTM_PERSON_ID_IDX")]
    [Index(nameof(PrimaryContactId), Name = "DSPFTM_PRIMARY_CONTACT_ID_IDX")]
    public partial class PimsDispositionFileTeam
    {
        [Key]
        [Column("DISPOSITION_FILE_TEAM_ID")]
        public long DispositionFileTeamId { get; set; }
        [Column("DISPOSITION_FILE_ID")]
        public long DispositionFileId { get; set; }
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }
        [Column("PRIMARY_CONTACT_ID")]
        public long? PrimaryContactId { get; set; }
        [Required]
        [Column("DSP_FL_TEAM_PROFILE_TYPE_CODE")]
        [StringLength(20)]
        public string DspFlTeamProfileTypeCode { get; set; }
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

        [ForeignKey(nameof(DispositionFileId))]
        [InverseProperty(nameof(PimsDispositionFile.PimsDispositionFileTeams))]
        public virtual PimsDispositionFile DispositionFile { get; set; }
        [ForeignKey(nameof(DspFlTeamProfileTypeCode))]
        [InverseProperty(nameof(PimsDspFlTeamProfileType.PimsDispositionFileTeams))]
        public virtual PimsDspFlTeamProfileType DspFlTeamProfileTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsDispositionFileTeams))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsDispositionFileTeamPeople))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(PrimaryContactId))]
        [InverseProperty(nameof(PimsPerson.PimsDispositionFileTeamPrimaryContacts))]
        public virtual PimsPerson PrimaryContact { get; set; }
    }
}
