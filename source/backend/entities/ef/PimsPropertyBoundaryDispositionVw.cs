using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PimsPropertyBoundaryDispositionVw
{
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    [Required]
    [Column("DISPOSITION_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionFileStatusTypeCode { get; set; }

    [Column("BOUNDARY", TypeName = "geometry")]
    public Geometry Boundary { get; set; }
}
