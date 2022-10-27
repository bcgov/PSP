using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("PIMSX_TableDefinitions")]
    public partial class PimsxTableDefinition
    {
        [Column("TABLE_NAME")]
        [StringLength(255)]
        public string TableName { get; set; }
        [Column("TABLE_ALIAS")]
        [StringLength(255)]
        public string TableAlias { get; set; }
        [Column("HIST_REQUIRED")]
        [StringLength(1)]
        public string HistRequired { get; set; }
        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }
}
