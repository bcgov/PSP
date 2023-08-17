using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_FILE_PERSON")]
    [Index(nameof(AcquisitionFileId), Name = "ACQPER_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(AcqFlPersonProfileTypeCode), Name = "ACQPER_ACQ_FL_PERSON_PROFILE_TYPE_CODE_IDX")]
    [Index(nameof(AcquisitionFileId), nameof(AcqFlPersonProfileTypeCode), Name = "ACQPER_FILE_PROFILE_TUC", IsUnique = true)]
    [Index(nameof(PersonId), Name = "ACQPER_PERSON_ID_IDX")]
    public partial class PimsAcquisitionFilePerson
    {
        public PimsAcquisitionFilePerson()
        {
            PimsCompensationRequisitions = new HashSet<PimsCompensationRequisition>();
        }

        [Key]
        [Column("ACQUISITION_FILE_PERSON_ID")]
        public long AcquisitionFilePersonId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("PERSON_ID")]
        public long PersonId { get; set; }
        [Column("ACQ_FL_PERSON_PROFILE_TYPE_CODE")]
        [StringLength(20)]
        public string AcqFlPersonProfileTypeCode { get; set; }
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

        [ForeignKey(nameof(AcqFlPersonProfileTypeCode))]
        [InverseProperty(nameof(PimsAcqFlPersonProfileType.PimsAcquisitionFilePeople))]
        public virtual PimsAcqFlPersonProfileType AcqFlPersonProfileTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsAcquisitionFilePeople))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsAcquisitionFilePeople))]
        public virtual PimsPerson Person { get; set; }
        [InverseProperty(nameof(PimsCompensationRequisition.AcquisitionFilePerson))]
        public virtual ICollection<PimsCompensationRequisition> PimsCompensationRequisitions { get; set; }
    }
}
