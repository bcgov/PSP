using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_CONTACT_METHOD")]
    [Index(nameof(ContactMethodTypeCode), Name = "CNTMTH_CONTACT_METHOD_TYPE_CODE_IDX")]
    [Index(nameof(OrganizationId), Name = "CNTMTH_ORGANIZATION_ID_IDX")]
    [Index(nameof(PersonId), Name = "CNTMTH_PERSON_ID_IDX")]
    public partial class PimsContactMethod
    {
        [Key]
        [Column("CONTACT_METHOD_ID")]
        public long ContactMethodId { get; set; }
        [Required]
        [Column("CONTACT_METHOD_TYPE_CODE")]
        [StringLength(20)]
        public string ContactMethodTypeCode { get; set; }
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }
        [Required]
        [Column("CONTACT_METHOD_VALUE")]
        [StringLength(200)]
        public string ContactMethodValue { get; set; }
        [Column("IS_PREFERRED_METHOD")]
        public bool? IsPreferredMethod { get; set; }
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

        [ForeignKey(nameof(ContactMethodTypeCode))]
        [InverseProperty(nameof(PimsContactMethodType.PimsContactMethods))]
        public virtual PimsContactMethodType ContactMethodTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsContactMethods))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsContactMethods))]
        public virtual PimsPerson Person { get; set; }
    }
}
