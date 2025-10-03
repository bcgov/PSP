using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code and description of a project.
/// </summary>
[Table("PIMS_PROJECT")]
[Index("BusinessFunctionCodeId", Name = "PROJCT_BUSINESS_FUNCTION_CODE_ID_IDX")]
[Index("Code", Name = "PROJCT_CODE_IDX")]
[Index("CostTypeCodeId", Name = "PROJCT_COST_TYPE_CODE_ID_IDX")]
[Index("Description", "Code", Name = "PROJCT_DESCRIPTION_CODE_TUC", IsUnique = true)]
[Index("ProjectStatusTypeCode", Name = "PROJCT_PROJECT_STATUS_CODE_IDX")]
[Index("RegionCode", Name = "PROJCT_REGION_CODE_IDX")]
[Index("WorkActivityCodeId", Name = "PROJCT_WORK_ACTIVITY_CODE_ID_IDX")]
public partial class PimsProject
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROJECT_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("PROJECT_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ProjectStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_BUSINESS_FUNCTION table.
    /// </summary>
    [Column("BUSINESS_FUNCTION_CODE_ID")]
    public long? BusinessFunctionCodeId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_COST_TYPE table.
    /// </summary>
    [Column("COST_TYPE_CODE_ID")]
    public long? CostTypeCodeId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_WORK_ACTIVITY table.
    /// </summary>
    [Column("WORK_ACTIVITY_CODE_ID")]
    public long? WorkActivityCodeId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_REGION table.
    /// </summary>
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    /// <summary>
    /// Project number.
    /// </summary>
    [Column("CODE")]
    [StringLength(20)]
    public string Code { get; set; }

    /// <summary>
    /// Project description.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Descriptive note relevant to the project.
    /// </summary>
    [Column("NOTE")]
    [StringLength(2000)]
    public string Note { get; set; }

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

    [ForeignKey("BusinessFunctionCodeId")]
    [InverseProperty("PimsProjects")]
    public virtual PimsBusinessFunctionCode BusinessFunctionCode { get; set; }

    [ForeignKey("CostTypeCodeId")]
    [InverseProperty("PimsProjects")]
    public virtual PimsCostTypeCode CostTypeCode { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<PimsAcquisitionFile> PimsAcquisitionFiles { get; set; } = new List<PimsAcquisitionFile>();

    [InverseProperty("AlternateProject")]
    public virtual ICollection<PimsCompensationRequisition> PimsCompensationRequisitions { get; set; } = new List<PimsCompensationRequisition>();

    [InverseProperty("Project")]
    public virtual ICollection<PimsDispositionFile> PimsDispositionFiles { get; set; } = new List<PimsDispositionFile>();

    [InverseProperty("Project")]
    public virtual ICollection<PimsLease> PimsLeases { get; set; } = new List<PimsLease>();

    [InverseProperty("Project")]
    public virtual ICollection<PimsManagementFile> PimsManagementFiles { get; set; } = new List<PimsManagementFile>();

    [InverseProperty("Project")]
    public virtual ICollection<PimsProjectDocument> PimsProjectDocuments { get; set; } = new List<PimsProjectDocument>();

    [InverseProperty("Project")]
    public virtual ICollection<PimsProjectNote> PimsProjectNotes { get; set; } = new List<PimsProjectNote>();

    [InverseProperty("Project")]
    public virtual ICollection<PimsProjectPerson> PimsProjectPeople { get; set; } = new List<PimsProjectPerson>();

    [InverseProperty("Project")]
    public virtual ICollection<PimsProjectProduct> PimsProjectProducts { get; set; } = new List<PimsProjectProduct>();

    [InverseProperty("Project")]
    public virtual ICollection<PimsResearchFileProject> PimsResearchFileProjects { get; set; } = new List<PimsResearchFileProject>();

    [ForeignKey("ProjectStatusTypeCode")]
    [InverseProperty("PimsProjects")]
    public virtual PimsProjectStatusType ProjectStatusTypeCodeNavigation { get; set; }

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsProjects")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }

    [ForeignKey("WorkActivityCodeId")]
    [InverseProperty("PimsProjects")]
    public virtual PimsWorkActivityCode WorkActivityCode { get; set; }
}
