using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Description of property improvements associated with the lease.
/// </summary>
[Table("PIMS_PROPERTY_IMPROVEMENT")]
[Index("PropertyId", Name = "PIMPRV_PROPERTY_ID_IDX")]
[Index("PropertyImprovementTypeCode", Name = "PIMPRV_PROPERTY_IMPROVEMENT_TYPE_CODE_IDX")]
[Index("PropImprvmntStatusTypeCode", Name = "PIMPRV_PROP_IMPRVMNT_STATUS_TYPE_CODE_IDX")]
[Index("PropertyId", "PropertyImprovementId", Name = "PIMPRV_PROP_IMPRV_TUC", IsUnique = true)]
public partial class PimsPropertyImprovement
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("PROPERTY_IMPROVEMENT_ID")]
    public long PropertyImprovementId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY table.
    /// </summary>
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY_IMPROVEMENT_TYPE table.
    /// </summary>
    [Required]
    [Column("PROPERTY_IMPROVEMENT_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyImprovementTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROP_IMPRVMNT_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("PROP_IMPRVMNT_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string PropImprvmntStatusTypeCode { get; set; }

    /// <summary>
    /// Name assigned to the property improvement.
    /// </summary>
    [Required]
    [Column("IMPROVEMENT_NAME")]
    [StringLength(500)]
    public string ImprovementName { get; set; }

    /// <summary>
    /// Description of the improvement.
    /// </summary>
    [Column("IMPROVEMENT_DESCRIPTION")]
    [StringLength(2000)]
    public string ImprovementDescription { get; set; }

    /// <summary>
    /// Date of the property improvement.
    /// </summary>
    [Column("IMPROVEMENT_DATE", TypeName = "datetime")]
    public DateTime? ImprovementDate { get; set; }

    /// <summary>
    /// Size of the structure (house, building, bridge, etc,)
    /// </summary>
    [Column("STRUCTURE_SIZE")]
    [StringLength(2000)]
    public string StructureSize { get; set; }

    /// <summary>
    /// Addresses affected
    /// </summary>
    [Column("ADDRESS")]
    [StringLength(2000)]
    public string Address { get; set; }

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

    [ForeignKey("PropImprvmntStatusTypeCode")]
    [InverseProperty("PimsPropertyImprovements")]
    public virtual PimsPropImprvmntStatusType PropImprvmntStatusTypeCodeNavigation { get; set; }

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropertyImprovements")]
    public virtual PimsProperty Property { get; set; }

    [ForeignKey("PropertyImprovementTypeCode")]
    [InverseProperty("PimsPropertyImprovements")]
    public virtual PimsPropertyImprovementType PropertyImprovementTypeCodeNavigation { get; set; }
}
