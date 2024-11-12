using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Codified values for the project/person role.  A given person is able to have multiple roles in the project.
/// </summary>
[Table("PIMS_PROJECT_PERSON_ROLE_TYPE")]
public partial class PimsProjectPersonRoleType
{
    /// <summary>
    /// Code value of the project/person role.
    /// </summary>
    [Key]
    [Column("PROJECT_PERSON_ROLE_TYPE_CODE")]
    [StringLength(20)]
    public string ProjectPersonRoleTypeCode { get; set; }

    /// <summary>
    /// Description of the project/person role.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code value is inactive.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Designates a preferred presentation order of the code descriptions.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("ProjectPersonRoleTypeCodeNavigation")]
    public virtual ICollection<PimsProjectPerson> PimsProjectPeople { get; set; } = new List<PimsProjectPerson>();
}
