﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table containing the countries defined to the system.
/// </summary>
[Table("PIMS_COUNTRY")]
public partial class PimsCountry
{
    [Key]
    [Column("COUNTRY_ID")]
    public short CountryId { get; set; }

    /// <summary>
    /// Abbreviated country code.
    /// </summary>
    [Required]
    [Column("COUNTRY_CODE")]
    [StringLength(20)]
    public string CountryCode { get; set; }

    /// <summary>
    /// Country name/description.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Defines the display order of the codes.
    /// </summary>
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

    [InverseProperty("Country")]
    public virtual ICollection<PimsAddress> PimsAddresses { get; set; } = new List<PimsAddress>();

    [InverseProperty("Country")]
    public virtual ICollection<PimsProvinceState> PimsProvinceStates { get; set; } = new List<PimsProvinceState>();
}
