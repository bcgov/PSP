using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_FILE_HIST")]
    [Index(nameof(AcquisitionFileHistId), nameof(EndDateHist), Name = "PIMS_ACQNFL_H_UK", IsUnique = true)]
    public partial class PimsAcquisitionFileHist
    {
        [Key]
        [Column("_ACQUISITION_FILE_HIST_ID")]
        public long AcquisitionFileHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("PROJECT_ID")]
        public long? ProjectId { get; set; }
        [Column("PRODUCT_ID")]
        public long? ProductId { get; set; }
        [Required]
        [Column("ACQUISITION_FILE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string AcquisitionFileStatusTypeCode { get; set; }
        [Required]
        [Column("ACQUISITION_TYPE_CODE")]
        [StringLength(20)]
        public string AcquisitionTypeCode { get; set; }
        [Column("ACQUISITION_FUNDING_TYPE_CODE")]
        [StringLength(20)]
        public string AcquisitionFundingTypeCode { get; set; }
        [Column("ACQ_PHYS_FILE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string AcqPhysFileStatusTypeCode { get; set; }
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Required]
        [Column("FILE_NAME")]
        [StringLength(500)]
        public string FileName { get; set; }
        [Column("FILE_NO")]
        public int FileNo { get; set; }
        [Required]
        [Column("FILE_NUMBER")]
        [StringLength(18)]
        public string FileNumber { get; set; }
        [Column("LEGACY_FILE_NUMBER")]
        [StringLength(18)]
        public string LegacyFileNumber { get; set; }
        [Column("LEGACY_STAKEHOLDER")]
        [StringLength(4000)]
        public string LegacyStakeholder { get; set; }
        [Column("FUNDING_OTHER")]
        [StringLength(200)]
        public string FundingOther { get; set; }
        [Column("ASSIGNED_DATE", TypeName = "datetime")]
        public DateTime? AssignedDate { get; set; }
        [Column("DELIVERY_DATE", TypeName = "datetime")]
        public DateTime? DeliveryDate { get; set; }
        [Column("COMPLETION_DATE", TypeName = "datetime")]
        public DateTime? CompletionDate { get; set; }
        [Column("PAIMS_ACQUISITION_FILE_ID")]
        public int? PaimsAcquisitionFileId { get; set; }
        [Column("TOTAL_ALLOWABLE_COMPENSATION", TypeName = "money")]
        public decimal? TotalAllowableCompensation { get; set; }
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
