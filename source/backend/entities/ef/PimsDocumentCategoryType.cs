﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// The volume unit used for measuring Properties.
/// </summary>
[Table("PIMS_DOCUMENT_CATEGORY_TYPE")]
public partial class PimsDocumentCategoryType
{
    /// <summary>
    /// The code value category of the document.
    /// </summary>
    [Key]
    [Column("DOCUMENT_CATEGORY_TYPE_CODE")]
    [StringLength(20)]
    public string DocumentCategoryTypeCode { get; set; }

    /// <summary>
    /// Translation of the code value into a description that can be displayed to the user.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code value is still active or is now disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Order in which to display the code values, if required.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

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

    [InverseProperty("DocumentCategoryTypeCodeNavigation")]
    public virtual ICollection<PimsDocumentCategorySubtype> PimsDocumentCategorySubtypes { get; set; } = new List<PimsDocumentCategorySubtype>();
}
