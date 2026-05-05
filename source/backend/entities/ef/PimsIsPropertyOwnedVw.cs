using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PimsIsPropertyOwnedVw
{
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("IS_OWNED")]
    public bool? IsOwned { get; set; }
}
