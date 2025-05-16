using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PimsPropertyLocationLiteVw
{
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("GEOMETRY", TypeName = "geometry")]
    public Geometry Geometry { get; set; }

    [Column("IS_OWNED")]
    public bool IsOwned { get; set; }

    [Column("IS_RETIRED")]
    public bool? IsRetired { get; set; }

    [Column("IS_OTHER_INTEREST")]
    public bool? IsOtherInterest { get; set; }

    [Column("IS_DISPOSED")]
    public bool? IsDisposed { get; set; }

    [Column("HAS_ACTIVE_ACQUISITION_FILE")]
    public bool? HasActiveAcquisitionFile { get; set; }

    [Column("HAS_ACTIVE_RESEARCH_FILE")]
    public bool? HasActiveResearchFile { get; set; }
}
