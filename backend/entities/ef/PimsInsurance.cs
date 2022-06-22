using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_INSURANCE")]
    [Index(nameof(InsuranceTypeCode), Name = "INSRNC_INSURANCE_TYPE_CODE_IDX")]
    [Index(nameof(LeaseId), Name = "INSRNC_LEASE_ID_IDX")]
    public partial class PimsInsurance
    {
        [Key]
        [Column("INSURANCE_ID")]
        public long InsuranceId { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
        [Required]
        [Column("INSURANCE_TYPE_CODE")]
        [StringLength(20)]
        public string InsuranceTypeCode { get; set; }
        [Column("OTHER_INSURANCE_TYPE")]
        [StringLength(200)]
        public string OtherInsuranceType { get; set; }
        [Column("COVERAGE_DESCRIPTION")]
        [StringLength(2000)]
        public string CoverageDescription { get; set; }
        [Column("COVERAGE_LIMIT", TypeName = "money")]
        public decimal? CoverageLimit { get; set; }
        [Column("IS_INSURANCE_IN_PLACE")]
        public bool? IsInsuranceInPlace { get; set; }
        [Column("EXPIRY_DATE", TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }
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

        [ForeignKey(nameof(InsuranceTypeCode))]
        [InverseProperty(nameof(PimsInsuranceType.PimsInsurances))]
        public virtual PimsInsuranceType InsuranceTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(LeaseId))]
        [InverseProperty(nameof(PimsLease.PimsInsurances))]
        public virtual PimsLease Lease { get; set; }
    }
}
