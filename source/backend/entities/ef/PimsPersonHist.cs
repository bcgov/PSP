using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PERSON_HIST")]
    [Index(nameof(PersonHistId), nameof(EndDateHist), Name = "PIMS_PERSON_H_UK", IsUnique = true)]
    public partial class PimsPersonHist
    {
        [Key]
        [Column("_PERSON_HIST_ID")]
        public long PersonHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("PERSON_ID")]
        public long PersonId { get; set; }
        [Required]
        [Column("SURNAME")]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [Column("FIRST_NAME")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Column("MIDDLE_NAMES")]
        [StringLength(200)]
        public string MiddleNames { get; set; }
        [Column("NAME_SUFFIX")]
        [StringLength(50)]
        public string NameSuffix { get; set; }
        [Column("PREFERRED_NAME")]
        [StringLength(200)]
        public string PreferredName { get; set; }
        [Column("BIRTH_DATE", TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        [Column("COMMENT")]
        [StringLength(2000)]
        public string Comment { get; set; }
        [Column("ADDRESS_COMMENT")]
        [StringLength(2000)]
        public string AddressComment { get; set; }
        [Column("USE_ORGANIZATION_ADDRESS")]
        public bool? UseOrganizationAddress { get; set; }
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }
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
    }
}
