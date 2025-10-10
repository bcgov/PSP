using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_ANOMALY_TYPE
/// </summary>
[Table("PIMS_PROP_PROP_ANOMALY_TYP")]
[Index("PropertyAnomalyTypeCode", Name = "PRPRAT_PROPERTY_ANOMALY_TYPE_CODE_IDX")]
[Index("PropertyId", Name = "PRPRAT_PROPERTY_ID_IDX")]
[Index("PropertyAnomalyTypeCode", "PropertyId", Name = "PRPRAT_PROP_ANOMALY_TYPE_TUC", IsUnique = true)]
public partial class PimsPropPropAnomalyTyp
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("PROP_PROP_ANOMALY_TYPE_ID")]
    public long PropPropAnomalyTypeId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY table.
    /// </summary>
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY_ANOMALY_TYPE table.
    /// </summary>
    [Required]
    [Column("PROPERTY_ANOMALY_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyAnomalyTypeCode { get; set; }

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
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

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

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropPropAnomalyTyps")]
    public virtual PimsProperty Property { get; set; }

    [ForeignKey("PropertyAnomalyTypeCode")]
    [InverseProperty("PimsPropPropAnomalyTyps")]
    public virtual PimsPropertyAnomalyType PropertyAnomalyTypeCodeNavigation { get; set; }
}
