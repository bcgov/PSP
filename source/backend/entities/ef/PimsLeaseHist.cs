using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_HIST")]
    [Index(nameof(LeaseHistId), nameof(EndDateHist), Name = "PIMS_LEASE_H_UK", IsUnique = true)]
    public partial class PimsLeaseHist
    {
        [Key]
        [Column("_LEASE_HIST_ID")]
        public long LeaseHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
        [Required]
        [Column("LEASE_PAY_RVBL_TYPE_CODE")]
        [StringLength(20)]
        public string LeasePayRvblTypeCode { get; set; }
        [Required]
        [Column("LEASE_LICENSE_TYPE_CODE")]
        [StringLength(20)]
        public string LeaseLicenseTypeCode { get; set; }
        [Column("LEASE_CATEGORY_TYPE_CODE")]
        [StringLength(20)]
        public string LeaseCategoryTypeCode { get; set; }
        [Required]
        [Column("LEASE_PURPOSE_TYPE_CODE")]
        [StringLength(20)]
        public string LeasePurposeTypeCode { get; set; }
        [Required]
        [Column("LEASE_PROGRAM_TYPE_CODE")]
        [StringLength(20)]
        public string LeaseProgramTypeCode { get; set; }
        [Column("LEASE_INITIATOR_TYPE_CODE")]
        [StringLength(20)]
        public string LeaseInitiatorTypeCode { get; set; }
        [Column("LEASE_RESPONSIBILITY_TYPE_CODE")]
        [StringLength(20)]
        public string LeaseResponsibilityTypeCode { get; set; }
        [Required]
        [Column("LEASE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string LeaseStatusTypeCode { get; set; }
        [Column("REGION_CODE")]
        public short? RegionCode { get; set; }
        [Column("PROJECT_ID")]
        public long? ProjectId { get; set; }
        [Column("L_FILE_NO")]
        [StringLength(50)]
        public string LFileNo { get; set; }
        [Column("TFA_FILE_NO")]
        public int? TfaFileNo { get; set; }
        [Column("TFA_FILE_NUMBER")]
        [StringLength(500)]
        public string TfaFileNumber { get; set; }
        [Column("PS_FILE_NO")]
        [StringLength(50)]
        public string PsFileNo { get; set; }
        [Column("LEASE_CATEGORY_OTHER_DESC")]
        [StringLength(200)]
        public string LeaseCategoryOtherDesc { get; set; }
        [Column("LEASE_PURPOSE_OTHER_DESC")]
        [StringLength(200)]
        public string LeasePurposeOtherDesc { get; set; }
        [Column("MOTI_CONTACT")]
        [StringLength(200)]
        public string MotiContact { get; set; }
        [Column("DOCUMENTATION_REFERENCE")]
        [StringLength(500)]
        public string DocumentationReference { get; set; }
        [Column("OTHER_LEASE_PROGRAM_TYPE")]
        [StringLength(200)]
        public string OtherLeaseProgramType { get; set; }
        [Column("OTHER_LEASE_LICENSE_TYPE")]
        [StringLength(200)]
        public string OtherLeaseLicenseType { get; set; }
        [Column("OTHER_LEASE_PURPOSE_TYPE")]
        [StringLength(200)]
        public string OtherLeasePurposeType { get; set; }
        [Column("ORIG_START_DATE", TypeName = "datetime")]
        public DateTime OrigStartDate { get; set; }
        [Column("ORIG_EXPIRY_DATE", TypeName = "datetime")]
        public DateTime? OrigExpiryDate { get; set; }
        [Column("LEASE_AMOUNT", TypeName = "money")]
        public decimal? LeaseAmount { get; set; }
        [Column("RESPONSIBILITY_EFFECTIVE_DATE", TypeName = "datetime")]
        public DateTime? ResponsibilityEffectiveDate { get; set; }
        [Column("INSPECTION_DATE", TypeName = "datetime")]
        public DateTime? InspectionDate { get; set; }
        [Column("IS_SUBJECT_TO_RTA")]
        public bool? IsSubjectToRta { get; set; }
        [Column("IS_COMM_BLDG")]
        public bool? IsCommBldg { get; set; }
        [Column("IS_OTHER_IMPROVEMENT")]
        public bool? IsOtherImprovement { get; set; }
        [Column("IS_EXPIRED")]
        public bool IsExpired { get; set; }
        [Column("HAS_PHYSICAL_FILE")]
        public bool? HasPhysicalFile { get; set; }
        [Column("HAS_DIGITAL_FILE")]
        public bool? HasDigitalFile { get; set; }
        [Column("HAS_PHYSICIAL_LICENSE")]
        public bool? HasPhysicialLicense { get; set; }
        [Column("HAS_DIGITAL_LICENSE")]
        public bool? HasDigitalLicense { get; set; }
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
