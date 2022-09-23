using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_DATA_ADVICE_AMENDMENT")]
    [Index(nameof(DataAdviceId), Name = "BCDAMD_DATA_ADVICE_ID_IDX")]
    public partial class BcaDataAdviceAmendment
    {
        [Column("DATA_ADVICE_ID")]
        public long? DataAdviceId { get; set; }
        [Column("AMENDMENT_REASON_CODE")]
        [StringLength(16)]
        public string AmendmentReasonCode { get; set; }
        [Column("AMENDMENT_REASON_DESCRIPTION")]
        [StringLength(255)]
        public string AmendmentReasonDescription { get; set; }
        [Column("FOLIO_COUNT")]
        public int FolioCount { get; set; }
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

        [ForeignKey(nameof(DataAdviceId))]
        public virtual BcaDataAdvice DataAdvice { get; set; }
    }
}
