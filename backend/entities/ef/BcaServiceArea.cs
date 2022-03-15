using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_SERVICE_AREA")]
    [Index(nameof(MinorTaxingId), Name = "BCSVCA_MINOR_TAXING_ID_IDX")]
    public partial class BcaServiceArea
    {
        [Column("MINOR_TAXING_ID")]
        public long? MinorTaxingId { get; set; }
        [Column("MINOR_TAXING_CODE")]
        [StringLength(16)]
        public string MinorTaxingCode { get; set; }
        [Column("MINOR_TAXING_CODE_SHORT")]
        [StringLength(1)]
        public string MinorTaxingCodeShort { get; set; }
        [Column("MINOR_TAXING_DESCRIPTION")]
        [StringLength(255)]
        public string MinorTaxingDescription { get; set; }
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

        [ForeignKey(nameof(MinorTaxingId))]
        public virtual BcaMinorTaxing MinorTaxing { get; set; }
    }
}
