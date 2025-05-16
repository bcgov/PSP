using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PimsPropertyBoundaryLeaseVw
{
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("BOUNDARY", TypeName = "geometry")]
    public Geometry Boundary { get; set; }

    [Column("HAS_PAYABLE_LEASE")]
    public int HasPayableLease { get; set; }

    [Column("HAS_RECEIVABLE_LEASE")]
    public int HasReceivableLease { get; set; }
}
