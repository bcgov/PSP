using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Description of the consultation type required for (currently) a lease or license.
/// </summary>
[Table("PIMS_CONSULTATION_TYPE")]
public partial class PimsConsultationType
{
    /// <summary>
    /// Code value of the consultation type.
    /// </summary>
    [Key]
    [Column("CONSULTATION_TYPE_CODE")]
    [StringLength(20)]
    public string ConsultationTypeCode { get; set; }

    /// <summary>
    /// Description of the consultation type.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Additional descriptive text of the consultation type.
    /// </summary>
    [Column("OTHER_DESCRIPTION")]
    [StringLength(200)]
    public string OtherDescription { get; set; }

    /// <summary>
    /// Onscreen display order of the consultation types.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the consultation type is active.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("ConsultationTypeCodeNavigation")]
    public virtual ICollection<PimsLeaseConsultation> PimsLeaseConsultations { get; set; } = new List<PimsLeaseConsultation>();
}
