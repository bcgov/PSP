using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Entity associating a paerson to a project.
/// </summary>
[Table("PIMS_PROJECT_PERSON")]
[Index("PersonId", Name = "PRJPER_PERSON_ID_IDX")]
[Index("ProjectId", Name = "PRJPER_PROJECT_ID_IDX")]
[Index("ProjectPersonRoleTypeCode", Name = "PRJPER_PROJECT_PERSON_ROLE_TYPE_CODE_IDX")]
[Index("PersonId", "ProjectId", "ProjectPersonRoleTypeCode", Name = "PRJPER_PROJECT_PERSON_TUC", IsUnique = true)]
public partial class PimsProjectPerson
{
    [Key]
    [Column("PROJECT_PERSON_ID")]
    public long ProjectPersonId { get; set; }

    [Column("PROJECT_ID")]
    public long ProjectId { get; set; }

    [Column("PERSON_ID")]
    public long PersonId { get; set; }

    [Column("PROJECT_PERSON_ROLE_TYPE_CODE")]
    [StringLength(20)]
    public string ProjectPersonRoleTypeCode { get; set; }

    /// <summary>
    /// Indicates if the relationship is active.
    /// </summary>
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

    [ForeignKey("PersonId")]
    [InverseProperty("PimsProjectPeople")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("PimsProjectPeople")]
    public virtual PimsProject Project { get; set; }

    [ForeignKey("ProjectPersonRoleTypeCode")]
    [InverseProperty("PimsProjectPeople")]
    public virtual PimsProjectPersonRoleType ProjectPersonRoleTypeCodeNavigation { get; set; }
}
