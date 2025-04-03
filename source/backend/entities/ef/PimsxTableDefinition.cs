using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[PrimaryKey("TableSchema", "TableName")]
[Table("PIMSX_TableDefinitions")]
public partial class PimsxTableDefinition
{
    /// <summary>
    /// Table schema.
    /// </summary>
    [Key]
    [Column("TABLE_SCHEMA")]
    [StringLength(128)]
    public string TableSchema { get; set; }

    /// <summary>
    /// Table name.
    /// </summary>
    [Key]
    [Column("TABLE_NAME")]
    [StringLength(255)]
    public string TableName { get; set; }

    /// <summary>
    /// Table alias.
    /// </summary>
    [Column("TABLE_ALIAS")]
    [StringLength(255)]
    public string TableAlias { get; set; }

    /// <summary>
    /// Is history required for this table?
    /// </summary>
    [Column("HIST_REQUIRED")]
    [StringLength(1)]
    public string HistRequired { get; set; }

    /// <summary>
    /// Description of the table.
    /// </summary>
    [Column("DESCRIPTION")]
    [StringLength(500)]
    public string Description { get; set; }
}
