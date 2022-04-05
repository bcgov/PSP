using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_FOLIO_AMENDMENT")]
    [Index(nameof(RollNumber), Name = "BCAFAM_ROLL_NUMBER_IDX")]
    public partial class BcaFolioAmendment
    {
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("AMENDMENT_TYPE")]
        [StringLength(16)]
        public string AmendmentType { get; set; }
        [Column("AMENDMENT_TYPE_DESCRIPTION")]
        [StringLength(255)]
        public string AmendmentTypeDescription { get; set; }
        [Column("AMENDMENT_REASON_CODE")]
        [StringLength(16)]
        public string AmendmentReasonCode { get; set; }
        [Column("AMENDMENT_REASON_DESCRIPTION")]
        [StringLength(255)]
        public string AmendmentReasonDescription { get; set; }
        [Column("SUPP_OCCUPANCY_DATE", TypeName = "date")]
        public DateTime? SuppOccupancyDate { get; set; }
        [Column("SUPP_OCCUPANCY_CODE")]
        [StringLength(16)]
        public string SuppOccupancyCode { get; set; }
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
