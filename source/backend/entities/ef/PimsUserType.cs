using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_USER_TYPE")]
    public partial class PimsUserType
    {
        public PimsUserType()
        {
            PimsAccessRequests = new HashSet<PimsAccessRequest>();
            PimsUsers = new HashSet<PimsUser>();
        }

        [Key]
        [Column("USER_TYPE_CODE")]
        [StringLength(20)]
        public string UserTypeCode { get; set; }
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

        [InverseProperty(nameof(PimsAccessRequest.UserTypeCodeNavigation))]
        public virtual ICollection<PimsAccessRequest> PimsAccessRequests { get; set; }
        [InverseProperty(nameof(PimsUser.UserTypeCodeNavigation))]
        public virtual ICollection<PimsUser> PimsUsers { get; set; }
    }
}
