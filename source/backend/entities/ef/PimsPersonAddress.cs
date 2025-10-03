using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// An associative entity to define multiple addresses for a person.
/// </summary>
[Table("PIMS_PERSON_ADDRESS")]
[Index("AddressId", Name = "PERADD_ADDRESS_ID_IDX")]
[Index("AddressUsageTypeCode", Name = "PERADD_ADDRESS_USAGE_TYPE_CODE_IDX")]
[Index("PersonId", Name = "PERADD_PERSON_ID_IDX")]
[Index("PersonId", "AddressId", "AddressUsageTypeCode", Name = "PERADD_UNQ_ADDR_TYPE_TUC", IsUnique = true)]
public partial class PimsPersonAddress
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("PERSON_ADDRESS_ID")]
    public long PersonAddressId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table.
    /// </summary>
    [Column("PERSON_ID")]
    public long PersonId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ADDRESS table.
    /// </summary>
    [Column("ADDRESS_ID")]
    public long AddressId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ADDRESS_USAGE_TYPE table.
    /// </summary>
    [Required]
    [Column("ADDRESS_USAGE_TYPE_CODE")]
    [StringLength(20)]
    public string AddressUsageTypeCode { get; set; }

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

    [ForeignKey("AddressId")]
    [InverseProperty("PimsPersonAddresses")]
    public virtual PimsAddress Address { get; set; }

    [ForeignKey("AddressUsageTypeCode")]
    [InverseProperty("PimsPersonAddresses")]
    public virtual PimsAddressUsageType AddressUsageTypeCodeNavigation { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsPersonAddresses")]
    public virtual PimsPerson Person { get; set; }
}
