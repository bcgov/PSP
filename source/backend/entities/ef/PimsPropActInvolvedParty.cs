﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROP_ACT_INVOLVED_PARTY")]
    [Index(nameof(OrganizationId), Name = "PAINVP_ORGANIZATION_ID_IDX")]
    [Index(nameof(PersonId), Name = "PAINVP_PERSON_ID_IDX")]
    [Index(nameof(PimsPropertyActivityId), Name = "PAINVP_PIMS_PROPERTY_ACTIVITY_ID_IDX")]
    public partial class PimsPropActInvolvedParty
    {
        [Key]
        [Column("PROP_ACT_INVOLVED_PARTY_ID")]
        public long PropActInvolvedPartyId { get; set; }
        [Column("PIMS_PROPERTY_ACTIVITY_ID")]
        public long PimsPropertyActivityId { get; set; }
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long ConcurrencyControlNumber { get; set; }
        [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppCreateTimestamp { get; set; }
        [Required]
        [Column("APP_CREATE_USERID")]
        [StringLength(30)]
        public string AppCreateUserid { get; set; }
        [Column("APP_CREATE_USER_GUID")]
        public Guid? AppCreateUserGuid { get; set; }
        [Required]
        [Column("APP_CREATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppCreateUserDirectory { get; set; }
        [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppLastUpdateTimestamp { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string AppLastUpdateUserid { get; set; }
        [Column("APP_LAST_UPDATE_USER_GUID")]
        public Guid? AppLastUpdateUserGuid { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppLastUpdateUserDirectory { get; set; }
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

        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsPropActInvolvedParties))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsPropActInvolvedParties))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(PimsPropertyActivityId))]
        [InverseProperty("PimsPropActInvolvedParties")]
        public virtual PimsPropertyActivity PimsPropertyActivity { get; set; }
    }
}
