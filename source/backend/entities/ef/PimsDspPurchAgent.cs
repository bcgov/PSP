using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DSP_PURCH_AGENT")]
    [Index(nameof(DispositionSaleId), Name = "DSPPAG_DISPOSITION_SALE_ID_IDX")]
    [Index(nameof(OrganizationId), Name = "DSPPAG_ORGANIZATION_ID_IDX")]
    [Index(nameof(PersonId), Name = "DSPPAG_PERSON_ID_IDX")]
    [Index(nameof(PrimaryContactId), Name = "DSPPAG_PRIMARY_CONTACT_ID_IDX")]
    public partial class PimsDspPurchAgent
    {
        [Key]
        [Column("DSP_PURCH_AGENT_ID")]
        public long DspPurchAgentId { get; set; }
        [Column("DISPOSITION_SALE_ID")]
        public long DispositionSaleId { get; set; }
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }
        [Column("PRIMARY_CONTACT_ID")]
        public long? PrimaryContactId { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [ForeignKey(nameof(DispositionSaleId))]
        [InverseProperty(nameof(PimsDispositionSale.PimsDspPurchAgents))]
        public virtual PimsDispositionSale DispositionSale { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsDspPurchAgents))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsDspPurchAgentPeople))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(PrimaryContactId))]
        [InverseProperty(nameof(PimsPerson.PimsDspPurchAgentPrimaryContacts))]
        public virtual PimsPerson PrimaryContact { get; set; }
    }
}
