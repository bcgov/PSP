using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table containing the compensation requisition data for the acquisition file.
/// </summary>
[Table("PIMS_H120_CATEGORY")]
[Index("CostTypeId", Name = "H120CT_COST_TYPE_ID_IDX")]
[Index("FinancialActivityId", Name = "H120CT_FINANCIAL_ACTIVITY_ID_IDX")]
[Index("WorkActivityId", Name = "H120CT_WORK_ACTIVITY_ID_IDX")]
public partial class PimsH120Category
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("H120_CATEGORY_ID")]
    public long H120CategoryId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_FINANCIAL_ACTIVITY table.
    /// </summary>
    [Column("FINANCIAL_ACTIVITY_ID")]
    public long FinancialActivityId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_WORK_ACTIVITY table.
    /// </summary>
    [Column("WORK_ACTIVITY_ID")]
    public long? WorkActivityId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_COST_TYPE table.
    /// </summary>
    [Column("COST_TYPE_ID")]
    public long? CostTypeId { get; set; }

    /// <summary>
    /// Number assigned to the H120 category.
    /// </summary>
    [Column("H120_CATEGORY_NO")]
    public int? H120CategoryNo { get; set; }

    /// <summary>
    /// Description of the H120 category.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Expiry date of the H120 category.
    /// </summary>
    [Column("EXPIRY_DATE", TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

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

    [ForeignKey("CostTypeId")]
    [InverseProperty("PimsH120Categories")]
    public virtual PimsCostTypeCode CostType { get; set; }

    [ForeignKey("FinancialActivityId")]
    [InverseProperty("PimsH120Categories")]
    public virtual PimsFinancialActivityCode FinancialActivity { get; set; }

    [ForeignKey("WorkActivityId")]
    [InverseProperty("PimsH120Categories")]
    public virtual PimsWorkActivityCode WorkActivity { get; set; }
}
