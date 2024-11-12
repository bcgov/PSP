using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Codified values for the form types.
/// </summary>
[Table("PIMS_FORM_TYPE")]
public partial class PimsFormType
{
    /// <summary>
    /// Code value of the form type.
    /// </summary>
    [Key]
    [Column("FORM_TYPE_CODE")]
    [StringLength(20)]
    public string FormTypeCode { get; set; }

    [Column("DOCUMENT_ID")]
    public long? DocumentId { get; set; }

    /// <summary>
    /// Description of the form type.
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

    [ForeignKey("DocumentId")]
    [InverseProperty("PimsFormTypes")]
    public virtual PimsDocument Document { get; set; }

    [InverseProperty("FormTypeCodeNavigation")]
    public virtual ICollection<PimsAcquisitionFileForm> PimsAcquisitionFileForms { get; set; } = new List<PimsAcquisitionFileForm>();
}
