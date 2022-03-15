using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("BCA_FOLIO_RECORD")]
    [Index(nameof(JurisdictionCode), Name = "BCAFOR_JURISDICTION_CODE_IDX")]
    public partial class BcaFolioRecord
    {
        public BcaFolioRecord()
        {
            BcaMinorTaxings = new HashSet<BcaMinorTaxing>();
            BcaOwnershipGroups = new HashSet<BcaOwnershipGroup>();
        }

        [Key]
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("JURISDICTION_CODE")]
        [StringLength(16)]
        public string JurisdictionCode { get; set; }
        [Column("FOLIO_STATUS")]
        [StringLength(16)]
        public string FolioStatus { get; set; }
        [Column("FOLIO_STATUS_DESCRIPTION")]
        [StringLength(255)]
        public string FolioStatusDescription { get; set; }
        [Column("ACTION")]
        [StringLength(16)]
        public string Action { get; set; }
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
        [InverseProperty(nameof(BcaJurisdiction.BcaFolioRecords))]
        public virtual BcaJurisdiction JurisdictionCodeNavigation { get; set; }
        [InverseProperty(nameof(BcaMinorTaxing.RollNumberNavigation))]
        public virtual ICollection<BcaMinorTaxing> BcaMinorTaxings { get; set; }
        [InverseProperty(nameof(BcaOwnershipGroup.RollNumberNavigation))]
        public virtual ICollection<BcaOwnershipGroup> BcaOwnershipGroups { get; set; }
    }
}
