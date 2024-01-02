using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table that contains the disposition checklist items that are presented to the user through dynamically building the input form.
/// </summary>
[Table("PIMS_DSP_CHKLST_ITEM_TYPE")]
[Index("DspChklstSectionTypeCode", Name = "DSPCIT_DSP_CHKLST_SECTION_TYPE_CODE_IDX")]
public partial class PimsDspChklstItemType
{
    /// <summary>
    /// Disposition checklist item code value.
    /// </summary>
    [Key]
    [Column("DSP_CHKLST_ITEM_TYPE_CODE")]
    [StringLength(20)]
    public string DspChklstItemTypeCode { get; set; }

    /// <summary>
    /// Disposition Section to which the item belongs.
    /// </summary>
    [Required]
    [Column("DSP_CHKLST_SECTION_TYPE_CODE")]
    [StringLength(20)]
    public string DspChklstSectionTypeCode { get; set; }

    /// <summary>
    /// Disposition Checklist item descriptive text presented to the user.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Disposition Checklist item descriptive tooltip presented to the user.
    /// </summary>
    [Column("HINT")]
    [StringLength(200)]
    public string Hint { get; set; }

    /// <summary>
    /// Indicates if the disposition checklist item is a required field.
    /// </summary>
    [Column("IS_REQUIRED")]
    public bool? IsRequired { get; set; }

    /// <summary>
    /// Specifies the order that the disposition checklist items are presented to the user.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Date the disposition checklist item is able to be presented to the user via the input form.
    /// </summary>
    [Column("EFFECTIVE_DATE")]
    public DateOnly EffectiveDate { get; set; }

    /// <summary>
    /// Date the disposition checklist item is removed from the input form.
    /// </summary>
    [Column("EXPIRY_DATE")]
    public DateOnly? ExpiryDate { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [ForeignKey("DspChklstSectionTypeCode")]
    [InverseProperty("PimsDspChklstItemTypes")]
    public virtual PimsDspChklstSectionType DspChklstSectionTypeCodeNavigation { get; set; }

    [InverseProperty("DspChklstItemTypeCodeNavigation")]
    public virtual ICollection<PimsDispositionChecklistItem> PimsDispositionChecklistItems { get; set; } = new List<PimsDispositionChecklistItem>();
}
