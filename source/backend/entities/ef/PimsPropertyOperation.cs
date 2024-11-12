using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Defines the operations that are associated with properties.  These operations conccern property consolidations and suvdivisions.
/// </summary>
[Table("PIMS_PROPERTY_OPERATION")]
[Index("DestinationPropertyId", Name = "PROPOP_DESTINATION_PROPERTY_ID_IDX")]
[Index("PropertyOperationNo", Name = "PROPOP_PROPERTY_OPERATION_NO_IDX")]
[Index("PropertyOperationTypeCode", Name = "PROPOP_PROPERTY_OPERATION_TYPE_CODE_IDX")]
[Index("SourcePropertyId", Name = "PROPOP_SOURCE_PROPERTY_ID_IDX")]
public partial class PimsPropertyOperation
{
    /// <summary>
    /// Surrogate sequence-based generated primary key for the table.  This is used internally to enforce data uniqueness.
    /// </summary>
    [Key]
    [Column("PROPERTY_OPERATION_ID")]
    public long PropertyOperationId { get; set; }

    /// <summary>
    /// Foreign key to the source property of the property operation.
    /// </summary>
    [Column("SOURCE_PROPERTY_ID")]
    public long SourcePropertyId { get; set; }

    /// <summary>
    /// Foreign key to the destination property of the property operation.
    /// </summary>
    [Column("DESTINATION_PROPERTY_ID")]
    public long DestinationPropertyId { get; set; }

    /// <summary>
    /// Foriegn key to the descriptive operation  type code.
    /// </summary>
    [Column("PROPERTY_OPERATION_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyOperationTypeCode { get; set; }

    /// <summary>
    /// Sequence-based operation identifying business key.  This is used to help identify when multiple properties were involved in a discrete operation.  The sequence number referenced is PIMS_PROPERTY_OPERATION_NO_SEQ.
    /// </summary>
    [Column("PROPERTY_OPERATION_NO")]
    public long PropertyOperationNo { get; set; }

    /// <summary>
    /// Business date of the property operation.
    /// </summary>
    [Column("OPERATION_DT", TypeName = "datetime")]
    public DateTime? OperationDt { get; set; }

    /// <summary>
    /// Indicates if the record is disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
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

    [ForeignKey("DestinationPropertyId")]
    [InverseProperty("PimsPropertyOperationDestinationProperties")]
    public virtual PimsProperty DestinationProperty { get; set; }

    [ForeignKey("PropertyOperationTypeCode")]
    [InverseProperty("PimsPropertyOperations")]
    public virtual PimsPropertyOperationType PropertyOperationTypeCodeNavigation { get; set; }

    [ForeignKey("SourcePropertyId")]
    [InverseProperty("PimsPropertyOperationSourceProperties")]
    public virtual PimsProperty SourceProperty { get; set; }
}
