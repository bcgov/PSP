﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROJECT_STATUS_TYPE")]
    public partial class PimsProjectStatusType
    {
        public PimsProjectStatusType()
        {
            PimsProjects = new HashSet<PimsProject>();
        }

        [Key]
        [Column("PROJECT_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string ProjectStatusTypeCode { get; set; }
        [Required]
        [Column("CODE_GROUP")]
        [StringLength(20)]
        public string CodeGroup { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [Column("TEXT")]
        [StringLength(1000)]
        public string Text { get; set; }
        [Required]
        [Column("IS_MILESTONE")]
        public bool? IsMilestone { get; set; }
        [Required]
        [Column("IS_TERMINAL")]
        public bool? IsTerminal { get; set; }
        [Required]
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long ConcurrencyControlNumber { get; set; }
        [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime DbCreateTimestamp { get; set; }
        [Required]
        [Column("DB_CREATE_USERID")]
        [StringLength(30)]
        public string DbCreateUserid { get; set; }
        [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime DbLastUpdateTimestamp { get; set; }
        [Required]
        [Column("DB_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string DbLastUpdateUserid { get; set; }

        [InverseProperty(nameof(PimsProject.ProjectStatusTypeCodeNavigation))]
        public virtual ICollection<PimsProject> PimsProjects { get; set; }
    }
}
