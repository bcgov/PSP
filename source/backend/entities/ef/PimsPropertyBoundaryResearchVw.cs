using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PimsPropertyBoundaryResearchVw
{
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("BOUNDARY", TypeName = "geometry")]
    public Geometry Boundary { get; set; }
}
