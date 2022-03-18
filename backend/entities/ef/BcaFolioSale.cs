using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_FOLIO_SALES")]
    [Index(nameof(RollNumber), Name = "BCAFSA_ROLL_NUMBER_IDX")]
    public partial class BcaFolioSale
    {
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("DOCUMENT_NUMBER")]
        [StringLength(255)]
        public string DocumentNumber { get; set; }
        [Column("SALE_DATE", TypeName = "date")]
        public DateTime? SaleDate { get; set; }
        [Column("SALE_PRICE", TypeName = "money")]
        public decimal? SalePrice { get; set; }
        [Column("SALE_STATUS_CODE")]
        [StringLength(16)]
        public string SaleStatusCode { get; set; }
        [Column("SALE_STATUS_DESCRIPTION")]
        [StringLength(255)]
        public string SaleStatusDescription { get; set; }
        [Column("CONVEYANCE_DATE", TypeName = "date")]
        public DateTime? ConveyanceDate { get; set; }
        [Column("CONVEYANCE_PRICE", TypeName = "money")]
        public decimal? ConveyancePrice { get; set; }
        [Column("CONVEYANCE_TYPE")]
        [StringLength(16)]
        public string ConveyanceType { get; set; }
        [Column("CONVEYANCE_TYPE_DESCRIPTION")]
        [StringLength(255)]
        public string ConveyanceTypeDescription { get; set; }
        [Column("REJECT_REASON_CODE")]
        [StringLength(16)]
        public string RejectReasonCode { get; set; }
        [Column("REJECT_REASON_DESCRIPTION")]
        [StringLength(255)]
        public string RejectReasonDescription { get; set; }
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

        [ForeignKey(nameof(RollNumber))]
        public virtual BcaFolioRecord RollNumberNavigation { get; set; }
    }
}
