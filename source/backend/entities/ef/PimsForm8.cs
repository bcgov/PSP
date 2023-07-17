using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_FORM_8")]
    [Index(nameof(AcquisitionFileId), Name = "FORM8_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(AcquisitionOwnerId), Name = "FORM8_ACQUISITION_OWNER_ID_IDX")]
    [Index(nameof(ExpropriatingAuthority), Name = "FORM8_EXPROPRIATING_AUTHORITY_IDX")]
    [Index(nameof(InterestHolderId), Name = "FORM8_INTEREST_HOLDER_ID_IDX")]
    [Index(nameof(PaymentItemTypeCode), Name = "FORM8_PAYMENT_ITEM_TYPE_CODE_IDX")]
    public partial class PimsForm8
    {
        [Key]
        [Column("FORM_8_ID")]
        public long Form8Id { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("ACQUISITION_OWNER_ID")]
        public long? AcquisitionOwnerId { get; set; }
        [Column("INTEREST_HOLDER_ID")]
        public long? InterestHolderId { get; set; }
        [Column("EXPROPRIATING_AUTHORITY")]
        public long? ExpropriatingAuthority { get; set; }
        [Column("PAYMENT_ITEM_TYPE_CODE")]
        [StringLength(20)]
        public string PaymentItemTypeCode { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(2000)]
        public string Description { get; set; }
        [Column("IS_GST_REQUIRED")]
        public bool? IsGstRequired { get; set; }
        [Column("PRETAX_AMT", TypeName = "money")]
        public decimal? PretaxAmt { get; set; }
        [Column("TAX_AMT", TypeName = "money")]
        public decimal? TaxAmt { get; set; }
        [Column("TOTAL_AMT", TypeName = "money")]
        public decimal? TotalAmt { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsForm8s))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [ForeignKey(nameof(AcquisitionOwnerId))]
        [InverseProperty(nameof(PimsAcquisitionOwner.PimsForm8s))]
        public virtual PimsAcquisitionOwner AcquisitionOwner { get; set; }
        [ForeignKey(nameof(ExpropriatingAuthority))]
        [InverseProperty(nameof(PimsOrganization.PimsForm8s))]
        public virtual PimsOrganization ExpropriatingAuthorityNavigation { get; set; }
        [ForeignKey(nameof(InterestHolderId))]
        [InverseProperty(nameof(PimsInterestHolder.PimsForm8s))]
        public virtual PimsInterestHolder InterestHolder { get; set; }
        [ForeignKey(nameof(PaymentItemTypeCode))]
        [InverseProperty(nameof(PimsPaymentItemType.PimsForm8s))]
        public virtual PimsPaymentItemType PaymentItemTypeCodeNavigation { get; set; }
    }
}
