using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code and description of a project.
/// </summary>
[Table("PIMS_PRODUCT")]
[Index("Code", Name = "PRODCT_CODE_IDX")]
public partial class PimsProduct
{
    /// <summary>
    /// System-generated primary key.
    /// </summary>
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    /// <summary>
    /// Product number.
    /// </summary>
    [Required]
    [Column("CODE")]
    [StringLength(20)]
    public string Code { get; set; }

    /// <summary>
    /// Product description.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Product start date.
    /// </summary>
    [Column("START_DATE", TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Estimate cost of the product.
    /// </summary>
    [Column("COST_ESTIMATE", TypeName = "money")]
    public decimal? CostEstimate { get; set; }

    /// <summary>
    /// Date the product cost was estimated.
    /// </summary>
    [Column("COST_ESTIMATE_DATE", TypeName = "datetime")]
    public DateTime? CostEstimateDate { get; set; }

    /// <summary>
    /// Product objective(s).
    /// </summary>
    [Column("OBJECTIVE")]
    [StringLength(2000)]
    public string Objective { get; set; }

    /// <summary>
    /// Product scope.
    /// </summary>
    [Column("SCOPE")]
    [StringLength(2000)]
    public string Scope { get; set; }

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

    [InverseProperty("Product")]
    public virtual ICollection<PimsAcquisitionFile> PimsAcquisitionFiles { get; set; } = new List<PimsAcquisitionFile>();

    [InverseProperty("Product")]
    public virtual ICollection<PimsDispositionFile> PimsDispositionFiles { get; set; } = new List<PimsDispositionFile>();

    [InverseProperty("Product")]
    public virtual ICollection<PimsLease> PimsLeases { get; set; } = new List<PimsLease>();

    [InverseProperty("Product")]
    public virtual ICollection<PimsProjectProduct> PimsProjectProducts { get; set; } = new List<PimsProjectProduct>();
}
