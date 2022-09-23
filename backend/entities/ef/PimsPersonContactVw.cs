using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    public partial class PimsPersonContactVw
    {
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("FAX")]
        [StringLength(200)]
        public string Fax { get; set; }
        [Column("PERSONAL_MOBILE")]
        [StringLength(200)]
        public string PersonalMobile { get; set; }
        [Column("PERSONAL_PHONE")]
        [StringLength(200)]
        public string PersonalPhone { get; set; }
        [Column("WORK_MOBILE")]
        [StringLength(200)]
        public string WorkMobile { get; set; }
        [Column("WORK_PHONE")]
        [StringLength(200)]
        public string WorkPhone { get; set; }
        [Column("WORK_EMAIL")]
        [StringLength(200)]
        public string WorkEmail { get; set; }
        [Column("PERSONAL_EMAIL")]
        [StringLength(200)]
        public string PersonalEmail { get; set; }
    }
}
