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
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("PROVINCE_STATE_ID")]
    public short ProvinceStateId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_COUNTRY table.
    /// </summary>
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
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Designates a preferred presentation order of the code descriptions.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
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
