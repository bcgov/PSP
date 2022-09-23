using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_JURISDICTION_DELETE")]
    [Index(nameof(JurisdictionCode), Name = "BCJDEL_JURISDICTION_CODE_IDX")]
    public partial class BcaJurisdictionDelete
    {
        [Column("JURISDICTION_CODE")]
        [StringLength(16)]
        public string JurisdictionCode { get; set; }
        [Required]
        [Column("DELETE_REASON_CODE")]
        [StringLength(16)]
        public string DeleteReasonCode { get; set; }
        [Column("DELETE_REASON_DESCRIPTION")]
        [StringLength(255)]
        public string DeleteReasonDescription { get; set; }
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

        [ForeignKey(nameof(JurisdictionCode))]
        public virtual BcaJurisdiction JurisdictionCodeNavigation { get; set; }
    }
}
