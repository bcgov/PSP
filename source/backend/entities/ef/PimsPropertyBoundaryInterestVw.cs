using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PimsPropertyBoundaryInterestVw
{
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("BOUNDARY", TypeName = "geometry")]
    public Geometry Boundary { get; set; }

    [Column("IS_NEW_LICENSE_TO_CONSTRUCT")]
    public bool IsNewLicenseToConstruct { get; set; }

    [Column("IS_NEW_LAND_ACT")]
    public bool IsNewLandAct { get; set; }

    [Column("IS_NEW_INTEREST_IN_SRW")]
    public bool IsNewInterestInSrw { get; set; }

    [Column("IS_THERE_SURPLUS")]
    public bool IsThereSurplus { get; set; }

    [Column("IS_ACQUIRED_FOR_INVENTORY")]
    public bool IsAcquiredForInventory { get; set; }
}
