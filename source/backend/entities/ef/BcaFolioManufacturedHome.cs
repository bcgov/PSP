using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_FOLIO_MANUFACTURED_HOME")]
    [Index(nameof(RollNumber), Name = "BCMANH_ROLL_NUMBER_IDX")]
    public partial class BcaFolioManufacturedHome
    {
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("MH_REGISTRY_NUMBER")]
        [StringLength(255)]
        public string MhRegistryNumber { get; set; }
        [Column("MH_BAY_NUMBER")]
        [StringLength(255)]
        public string MhBayNumber { get; set; }
        [Column("MH_PARK")]
        [StringLength(255)]
        public string MhPark { get; set; }
        [Column("MH_PARK_ROLL_NUMBER")]
        [StringLength(255)]
        public string MhParkRollNumber { get; set; }
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
