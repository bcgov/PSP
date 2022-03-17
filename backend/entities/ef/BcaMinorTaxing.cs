using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("BCA_MINOR_TAXING")]
    [Index(nameof(RollNumber), Name = "BCMNTX_ROLL_NUMBER_IDX")]
    public partial class BcaMinorTaxing
    {
        [Key]
        [Column("MINOR_TAXING_ID")]
        public long MinorTaxingId { get; set; }
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
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
        [InverseProperty(nameof(BcaFolioRecord.BcaMinorTaxings))]
        public virtual BcaFolioRecord RollNumberNavigation { get; set; }
    }
}
