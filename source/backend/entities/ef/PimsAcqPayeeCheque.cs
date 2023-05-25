using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQ_PAYEE_CHEQUE")]
    [Index(nameof(AcquisitionPayeeId), Name = "AQPCHQ_ACQUISITION_PAYEE_ID_IDX")]
    public partial class PimsAcqPayeeCheque
    {
        [Key]
        [Column("ACQ_PAYEE_CHEQUE_ID")]
        public long AcqPayeeChequeId { get; set; }
        [Column("ACQUISITION_PAYEE_ID")]
        public long AcquisitionPayeeId { get; set; }
        [Column("PRETAX_AMT", TypeName = "money")]
        public decimal? PretaxAmt { get; set; }
        [Column("TAX_AMT", TypeName = "money")]
        public decimal? TaxAmt { get; set; }
        [Column("TOTAL_AMT", TypeName = "money")]
        public decimal? TotalAmt { get; set; }
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
        [Column("IS_GST_REQUIRED")]
        public bool? IsGstRequired { get; set; }

        [ForeignKey(nameof(AcquisitionPayeeId))]
        [InverseProperty(nameof(PimsAcquisitionPayee.PimsAcqPayeeCheques))]
        public virtual PimsAcquisitionPayee AcquisitionPayee { get; set; }
    }
}
