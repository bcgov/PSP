using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table containing the provinces and states that are defined for the system.
/// </summary>
[Table("PIMS_PROVINCE_STATE")]
[Index("CountryId", Name = "PROVNC_COUNTRY_ID_IDX")]
public partial class PimsProvinceState
{
    [Key]
    [Column("PROVINCE_STATE_ID")]
    public short ProvinceStateId { get; set; }

    [Column("COUNTRY_ID")]
    public short CountryId { get; set; }

    /// <summary>
    /// Abbreviated province.state code.
    /// </summary>
    [Required]
    [Column("PROVINCE_STATE_CODE")]
    [StringLength(20)]
    public string ProvinceStateCode { get; set; }

    /// <summary>
    /// Full name/description of the provbince/state.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if this code is disabled or enabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

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

    [ForeignKey("CountryId")]
    [InverseProperty("PimsProvinceStates")]
    public virtual PimsCountry Country { get; set; }

    [InverseProperty("ProvinceState")]
    public virtual ICollection<PimsAddress> PimsAddresses { get; set; } = new List<PimsAddress>();
}
