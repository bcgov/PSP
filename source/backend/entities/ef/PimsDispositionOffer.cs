using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DISPOSITION_OFFER")]
    [Index(nameof(DispositionFileId), Name = "DSPOFR_DISPOSITION_FILE_ID_IDX")]
    [Index(nameof(DispositionOfferStatusTypeCode), Name = "DSPOFR_DISPOSITION_OFFER_STATUS_TYPE_CODE_IDX")]
    public partial class PimsDispositionOffer
    {
        [Key]
        [Column("DISPOSITION_OFFER_ID")]
        public long DispositionOfferId { get; set; }
        [Column("DISPOSITION_FILE_ID")]
        public long DispositionFileId { get; set; }
        [Column("DISPOSITION_OFFER_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string DispositionOfferStatusTypeCode { get; set; }
        [Required]
        [Column("OFFER_NAME")]
        [StringLength(1000)]
        public string OfferName { get; set; }
        [Column("OFFER_DT", TypeName = "date")]
        public DateTime OfferDt { get; set; }
        [Column("OFFER_EXPIRY_DT", TypeName = "date")]
        public DateTime? OfferExpiryDt { get; set; }
        [Column("OFFER_AMT", TypeName = "money")]
        public decimal OfferAmt { get; set; }
        [Column("OFFER_NOTE")]
        [StringLength(2000)]
        public string OfferNote { get; set; }
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

        [ForeignKey(nameof(DispositionFileId))]
        [InverseProperty(nameof(PimsDispositionFile.PimsDispositionOffers))]
        public virtual PimsDispositionFile DispositionFile { get; set; }
        [ForeignKey(nameof(DispositionOfferStatusTypeCode))]
        [InverseProperty(nameof(PimsDispositionOfferStatusType.PimsDispositionOffers))]
        public virtual PimsDispositionOfferStatusType DispositionOfferStatusTypeCodeNavigation { get; set; }
    }
}
