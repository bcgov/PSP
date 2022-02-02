using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    public partial class PimsContactMgrVw
    {
        [Required]
        [Column("ID")]
        [StringLength(25)]
        public string Id { get; set; }
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }
        [Column("SUMMARY")]
        [StringLength(302)]
        public string Summary { get; set; }
        [Column("SURNAME")]
        [StringLength(50)]
        public string Surname { get; set; }
        [Column("FIRST_NAME")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Column("MIDDLE_NAMES")]
        [StringLength(200)]
        public string MiddleNames { get; set; }
        [Column("ORGANIZATION_NAME")]
        [StringLength(200)]
        public string OrganizationName { get; set; }
        [Column("ADDRESS_ID")]
        public long? AddressId { get; set; }
        [Column("MAILING_ADDRESS")]
        [StringLength(200)]
        public string MailingAddress { get; set; }
        [Column("MUNICIPALITY_NAME")]
        [StringLength(200)]
        public string MunicipalityName { get; set; }
        [Column("PROVINCE_STATE")]
        [StringLength(20)]
        public string ProvinceState { get; set; }
        [Column("EMAIL_ADDRESS")]
        [StringLength(200)]
        public string EmailAddress { get; set; }
    }
}
