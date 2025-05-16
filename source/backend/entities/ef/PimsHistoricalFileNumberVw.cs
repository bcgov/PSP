using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PimsHistoricalFileNumberVw
{
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("HISTORICAL_FILE_NUMBER_STR")]
    [StringLength(4000)]
    public string HistoricalFileNumberStr { get; set; }
}
