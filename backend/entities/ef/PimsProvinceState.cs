using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROVINCE_STATE")]
    [Index(nameof(CountryId), Name = "PROVNC_COUNTRY_ID_IDX")]
    public partial class PimsProvinceState
    {
        public PimsProvinceState()
        {
            PimsAddresses = new HashSet<PimsAddress>();
        }

        [Key]
        [Column("PROVINCE_STATE_ID")]
        public short ProvinceStateId { get; set; }
        [Column("COUNTRY_ID")]
        public short CountryId { get; set; }
        [Required]
        [Column("PROVINCE_STATE_CODE")]
        [StringLength(20)]
        public string ProvinceStateCode { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long ConcurrencyControlNumber { get; set; }
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

        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(PimsCountry.PimsProvinceStates))]
        public virtual PimsCountry Country { get; set; }
        [InverseProperty(nameof(PimsAddress.ProvinceState))]
        public virtual ICollection<PimsAddress> PimsAddresses { get; set; }
    }
}
