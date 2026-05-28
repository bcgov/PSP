using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PimsIsPropertyDisposedVw
{
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("IS_DISPOSED")]
    public bool? IsDisposed { get; set; }
}
