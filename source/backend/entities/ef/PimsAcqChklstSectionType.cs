using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table that contains the checklist sctions that are presented to the user through dynamically building the input form.
/// </summary>
[Table("PIMS_ACQ_CHKLST_SECTION_TYPE")]
public partial class PimsAcqChklstSectionType
{
    /// <summary>
    /// Checklist section code value.
    /// </summary>
    [Key]
    [Column("ACQ_CHKLST_SECTION_TYPE_CODE")]
    [StringLength(20)]
    public string AcqChklstSectionTypeCode { get; set; }

    /// <summary>
    /// Checklist section descriptive text presented to the user.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Specifies the order that the checklist sections are presented to the user.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Date the checklist section is able to be presented to the user via the input form.
    /// </summary>
    [Column("EFFECTIVE_DATE")]
    public DateOnly EffectiveDate { get; set; }

    /// <summary>
    /// Date the checklist section is removed from the input form.
    /// </summary>
    [Column("EXPIRY_DATE")]
    public DateOnly? ExpiryDate { get; set; }

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

    [InverseProperty("AcqChklstSectionTypeCodeNavigation")]
    public virtual ICollection<PimsAcqChklstItemType> PimsAcqChklstItemTypes { get; set; } = new List<PimsAcqChklstItemType>();
}
