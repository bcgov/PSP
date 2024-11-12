using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_INSURANCE")]
[Index("InsuranceTypeCode", Name = "INSRNC_INSURANCE_TYPE_CODE_IDX")]
[Index("LeaseId", Name = "INSRNC_LEASE_ID_IDX")]
public partial class PimsInsurance
{
    /// <summary>
    /// Generated surrogate primary key
    /// </summary>
    [Key]
    [Column("INSURANCE_ID")]
    public long InsuranceId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    /// <summary>
    /// Foreign key indicating the type of insurance on the lease.
    /// </summary>
    [Required]
    [Column("INSURANCE_TYPE_CODE")]
    [StringLength(20)]
    public string InsuranceTypeCode { get; set; }

    /// <summary>
    /// Description of the non-standard insurance coverage type
    /// </summary>
    [Column("OTHER_INSURANCE_TYPE")]
    [StringLength(200)]
    public string OtherInsuranceType { get; set; }

    /// <summary>
    /// Description of the insurance coverage
    /// </summary>
    [Column("COVERAGE_DESCRIPTION")]
    [StringLength(2000)]
    public string CoverageDescription { get; set; }

    /// <summary>
    /// Monetary limit of the insurance coverage
    /// </summary>
    [Column("COVERAGE_LIMIT", TypeName = "money")]
    public decimal? CoverageLimit { get; set; }

    /// <summary>
    /// Indicator that insurance is in place.  TRUE if insurance is in place, FALSE if insurance is not in place, and NULL if it is unknown if insurance is in place.
    /// </summary>
    [Column("IS_INSURANCE_IN_PLACE")]
    public bool? IsInsuranceInPlace { get; set; }

    /// <summary>
    /// Date the insurance expires
    /// </summary>
    [Column("EXPIRY_DATE")]
    public DateOnly? ExpiryDate { get; set; }

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
    /// The date and time the record was updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("InsuranceTypeCode")]
    [InverseProperty("PimsInsurances")]
    public virtual PimsInsuranceType InsuranceTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsInsurances")]
    public virtual PimsLease Lease { get; set; }
}
