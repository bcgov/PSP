﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQ_FL_TEAM_PROFILE_TYPE")]
    public partial class PimsAcqFlTeamProfileType
    {
        public PimsAcqFlTeamProfileType()
        {
            PimsAcquisitionFileTeams = new HashSet<PimsAcquisitionFileTeam>();
        }

        [Key]
        [Column("ACQ_FL_TEAM_PROFILE_TYPE_CODE")]
        [StringLength(20)]
        public string AcqFlTeamProfileTypeCode { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
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

        [InverseProperty(nameof(PimsAcquisitionFileTeam.AcqFlTeamProfileTypeCodeNavigation))]
        public virtual ICollection<PimsAcquisitionFileTeam> PimsAcquisitionFileTeams { get; set; }
    }
}