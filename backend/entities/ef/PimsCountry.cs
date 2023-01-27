﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_COUNTRY")]
    public partial class PimsCountry
    {
        public PimsCountry()
        {
            PimsAddresses = new HashSet<PimsAddress>();
            PimsProvinceStates = new HashSet<PimsProvinceState>();
        }

        [Key]
        [Column("COUNTRY_ID")]
        public short CountryId { get; set; }
        [Required]
        [Column("COUNTRY_CODE")]
        [StringLength(20)]
        public string CountryCode { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
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

        [InverseProperty(nameof(PimsAddress.Country))]
        public virtual ICollection<PimsAddress> PimsAddresses { get; set; }
        [InverseProperty(nameof(PimsProvinceState.Country))]
        public virtual ICollection<PimsProvinceState> PimsProvinceStates { get; set; }
    }
}
