using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_INSURANCE_HIST")]
    [Index(nameof(InsuranceHistId), nameof(EndDateHist), Name = "PIMS_INSRNC_H_UK", IsUnique = true)]
    public partial class PimsInsuranceHist
    {
        [Key]
        [Column("_INSURANCE_HIST_ID")]
        public long InsuranceHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
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
    }
}
