using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROJECT")]
    [Index(nameof(BusinessFunctionCodeId), Name = "PROJCT_BUSINESS_FUNCTION_CODE_ID_IDX")]
    [Index(nameof(Code), Name = "PROJCT_CODE_IDX")]
    [Index(nameof(CostTypeCodeId), Name = "PROJCT_COST_TYPE_CODE_ID_IDX")]
    [Index(nameof(Description), nameof(Code), Name = "PROJCT_DESCRIPTION_CODE_TUC", IsUnique = true)]
    [Index(nameof(ProjectStatusTypeCode), Name = "PROJCT_PROJECT_STATUS_CODE_IDX")]
    [Index(nameof(RegionCode), Name = "PROJCT_REGION_CODE_IDX")]
    [Index(nameof(WorkActivityCodeId), Name = "PROJCT_WORK_ACTIVITY_CODE_ID_IDX")]
    public partial class PimsProject
    {
        public PimsProject()
        {
            PimsAcquisitionFiles = new HashSet<PimsAcquisitionFile>();
            PimsLeases = new HashSet<PimsLease>();
            PimsProducts = new HashSet<PimsProduct>();
            PimsProjectDocuments = new HashSet<PimsProjectDocument>();
            PimsProjectNotes = new HashSet<PimsProjectNote>();
            PimsProjectPeople = new HashSet<PimsProjectPerson>();
            PimsResearchFileProjects = new HashSet<PimsResearchFileProject>();
        }

        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("PROJECT_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string ProjectStatusTypeCode { get; set; }
        [Column("BUSINESS_FUNCTION_CODE_ID")]
        public long? BusinessFunctionCodeId { get; set; }
        [Column("COST_TYPE_CODE_ID")]
        public long? CostTypeCodeId { get; set; }
        [Column("WORK_ACTIVITY_CODE_ID")]
        public long? WorkActivityCodeId { get; set; }
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Column("CODE")]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
        [Column("NOTE")]
        [StringLength(2000)]
        public string Note { get; set; }
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

        [ForeignKey(nameof(BusinessFunctionCodeId))]
        [InverseProperty(nameof(PimsBusinessFunctionCode.PimsProjects))]
        public virtual PimsBusinessFunctionCode BusinessFunctionCode { get; set; }
        [ForeignKey(nameof(CostTypeCodeId))]
        [InverseProperty(nameof(PimsCostTypeCode.PimsProjects))]
        public virtual PimsCostTypeCode CostTypeCode { get; set; }
        [ForeignKey(nameof(ProjectStatusTypeCode))]
        [InverseProperty(nameof(PimsProjectStatusType.PimsProjects))]
        public virtual PimsProjectStatusType ProjectStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(RegionCode))]
        [InverseProperty(nameof(PimsRegion.PimsProjects))]
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        [ForeignKey(nameof(WorkActivityCodeId))]
        [InverseProperty(nameof(PimsWorkActivityCode.PimsProjects))]
        public virtual PimsWorkActivityCode WorkActivityCode { get; set; }
        [InverseProperty(nameof(PimsAcquisitionFile.Project))]
        public virtual ICollection<PimsAcquisitionFile> PimsAcquisitionFiles { get; set; }
        [InverseProperty(nameof(PimsLease.Project))]
        public virtual ICollection<PimsLease> PimsLeases { get; set; }
        [InverseProperty(nameof(PimsProduct.ParentProject))]
        public virtual ICollection<PimsProduct> PimsProducts { get; set; }
        [InverseProperty(nameof(PimsProjectDocument.Project))]
        public virtual ICollection<PimsProjectDocument> PimsProjectDocuments { get; set; }
        [InverseProperty(nameof(PimsProjectNote.Project))]
        public virtual ICollection<PimsProjectNote> PimsProjectNotes { get; set; }
        [InverseProperty(nameof(PimsProjectPerson.Project))]
        public virtual ICollection<PimsProjectPerson> PimsProjectPeople { get; set; }
        [InverseProperty(nameof(PimsResearchFileProject.Project))]
        public virtual ICollection<PimsResearchFileProject> PimsResearchFileProjects { get; set; }
    }
}
