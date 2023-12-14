using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DISPOSITION_SALE")]
    [Index(nameof(DispositionFileId), Name = "DSPSAL_DISPOSITION_FILE_ID_IDX")]
    public partial class PimsDispositionSale
    {
        public PimsDispositionSale()
        {
            PimsDispositionPurchasers = new HashSet<PimsDispositionPurchaser>();
            PimsDspPurchAgents = new HashSet<PimsDspPurchAgent>();
            PimsDspPurchSolicitors = new HashSet<PimsDspPurchSolicitor>();
        }

        [Key]
        [Column("DISPOSITION_SALE_ID")]
        public long DispositionSaleId { get; set; }
        [Column("DISPOSITION_FILE_ID")]
        public long DispositionFileId { get; set; }
        [Column("FINAL_CONDITION_REMOVAL_DT", TypeName = "date")]
        public DateTime? FinalConditionRemovalDt { get; set; }
        [Column("SALE_COMPLETION_DT", TypeName = "date")]
        public DateTime? SaleCompletionDt { get; set; }
        [Column("SALE_FISCAL_YEAR")]
        [StringLength(4)]
        public string SaleFiscalYear { get; set; }
        [Column("FINAL_SALE_AMT", TypeName = "money")]
        public decimal? FinalSaleAmt { get; set; }
        [Column("REALTOR_COMMISISSION_AMT", TypeName = "money")]
        public decimal? RealtorCommisissionAmt { get; set; }
        [Required]
        [Column("IS_GST_REQUIRED")]
        public bool? IsGstRequired { get; set; }
        [Column("GST_COLLECTED_AMT", TypeName = "money")]
        public decimal? GstCollectedAmt { get; set; }
        [Column("NET_BOOK_AMT", TypeName = "money")]
        public decimal? NetBookAmt { get; set; }
        [Column("TOTAL_COST_AMT", TypeName = "money")]
        public decimal? TotalCostAmt { get; set; }
        [Column("NET_PROCEEDS_BEFORE_SPP_AMT", TypeName = "money")]
        public decimal? NetProceedsBeforeSppAmt { get; set; }
        [Column("NET_PROCEEDS_AFTER_SPP_AMT", TypeName = "money")]
        public decimal? NetProceedsAfterSppAmt { get; set; }
        [Column("SPP_AMT", TypeName = "money")]
        public decimal? SppAmt { get; set; }
        [Column("REMEDIATION_AMT", TypeName = "money")]
        public decimal? RemediationAmt { get; set; }
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
        [InverseProperty(nameof(PimsDispositionFile.PimsDispositionSales))]
        public virtual PimsDispositionFile DispositionFile { get; set; }
        [InverseProperty(nameof(PimsDispositionPurchaser.DispositionSale))]
        public virtual ICollection<PimsDispositionPurchaser> PimsDispositionPurchasers { get; set; }
        [InverseProperty(nameof(PimsDspPurchAgent.DispositionSale))]
        public virtual ICollection<PimsDspPurchAgent> PimsDspPurchAgents { get; set; }
        [InverseProperty(nameof(PimsDspPurchSolicitor.DispositionSale))]
        public virtual ICollection<PimsDspPurchSolicitor> PimsDspPurchSolicitors { get; set; }
    }
}
