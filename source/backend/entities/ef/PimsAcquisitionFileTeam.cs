using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_FILE_TEAM")]
    [Index(nameof(AcquisitionFileId), nameof(AcqFlTeamProfileTypeCode), Name = "ACQNTM_FILE_PROFILE_TUC", IsUnique = true)]
    [Index(nameof(OrganizationId), Name = "ACQNTM_ORGANIZATION_ID_IDX")]
    [Index(nameof(PrimaryContactId), Name = "ACQNTM_PRIMARY_CONTACT_ID_IDX")]
    [Index(nameof(AcquisitionFileId), Name = "ACQPER_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(AcqFlTeamProfileTypeCode), Name = "ACQPER_ACQ_FL_PERSON_PROFILE_TYPE_CODE_IDX")]
    [Index(nameof(PersonId), Name = "ACQPER_PERSON_ID_IDX")]
    public partial class PimsAcquisitionFileTeam
    {
        public PimsAcquisitionFileTeam()
        {
            PimsCompensationRequisitions = new HashSet<PimsCompensationRequisition>();
        }

        [Key]
        [Column("ACQUISITION_FILE_TEAM_ID")]
        public long AcquisitionFileTeamId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }
        [Column("PRIMARY_CONTACT_ID")]
        public long? PrimaryContactId { get; set; }
        [Column("ACQ_FL_TEAM_PROFILE_TYPE_CODE")]
        [StringLength(20)]
        public string AcqFlTeamProfileTypeCode { get; set; }
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

        [ForeignKey(nameof(AcqFlTeamProfileTypeCode))]
        [InverseProperty(nameof(PimsAcqFlTeamProfileType.PimsAcquisitionFileTeams))]
        public virtual PimsAcqFlTeamProfileType AcqFlTeamProfileTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsAcquisitionFileTeams))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsAcquisitionFileTeams))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsAcquisitionFileTeamPeople))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(PrimaryContactId))]
        [InverseProperty(nameof(PimsPerson.PimsAcquisitionFileTeamPrimaryContacts))]
        public virtual PimsPerson PrimaryContact { get; set; }
        [InverseProperty(nameof(PimsCompensationRequisition.AcquisitionFileTeam))]
        public virtual ICollection<PimsCompensationRequisition> PimsCompensationRequisitions { get; set; }
    }
}
