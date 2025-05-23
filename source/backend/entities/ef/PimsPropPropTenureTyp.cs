﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_TENURE_TYPE
/// </summary>
[Table("PIMS_PROP_PROP_TENURE_TYP")]
[Index("PropertyId", Name = "PRPRTT_PROPERTY_ID_IDX")]
[Index("PropertyTenureTypeCode", Name = "PRPRTT_PROPERTY_TENURE_TYPE_CODE_IDX")]
[Index("PropertyTenureTypeCode", "PropertyId", Name = "PRPRTT_PROP_PROP_TENURE_TUC", IsUnique = true)]
public partial class PimsPropPropTenureTyp
{
    [Key]
    [Column("PROP_PROP_TENURE_TYPE_ID")]
    public long PropPropTenureTypeId { get; set; }

    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Required]
    [Column("PROPERTY_TENURE_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyTenureTypeCode { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

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

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropPropTenureTyps")]
    public virtual PimsProperty Property { get; set; }

    [ForeignKey("PropertyTenureTypeCode")]
    [InverseProperty("PimsPropPropTenureTyps")]
    public virtual PimsPropertyTenureType PropertyTenureTypeCodeNavigation { get; set; }
}
