﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROJECT_PROPERTY")]
    [Index(nameof(ProjectId), Name = "PRJPRP_PROJECT_ID_IDX")]
    [Index(nameof(PropertyId), nameof(ProjectId), Name = "PRJPRP_PROJECT_PROPERTY_TUC", IsUnique = true)]
    [Index(nameof(PropertyId), Name = "PRJPRP_PROPERTY_ID_IDX")]
    public partial class PimsProjectProperty
    {
        [Key]
        [Column("PROJECT_PROPERTY_ID")]
        public long ProjectPropertyId { get; set; }
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppCreateTimestamp { get; set; }
        [Required]
        [Column("APP_CREATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppCreateUserDirectory { get; set; }
        [Column("APP_CREATE_USER_GUID")]
        public Guid? AppCreateUserGuid { get; set; }
        [Required]
        [Column("APP_CREATE_USERID")]
        [StringLength(30)]
        public string AppCreateUserid { get; set; }
        [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppLastUpdateTimestamp { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppLastUpdateUserDirectory { get; set; }
        [Column("APP_LAST_UPDATE_USER_GUID")]
        public Guid? AppLastUpdateUserGuid { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string AppLastUpdateUserid { get; set; }
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

        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(PimsProject.PimsProjectProperties))]
        public virtual PimsProject Project { get; set; }
        [ForeignKey(nameof(PropertyId))]
        [InverseProperty(nameof(PimsProperty.PimsProjectProperties))]
        public virtual PimsProperty Property { get; set; }
    }
}
