using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DISPOSITION_PURCHASER")]
    [Index(nameof(DispositionSaleId), Name = "DSPPUR_DISPOSITION_SALE_ID_IDX")]
    [Index(nameof(OrganizationId), Name = "DSPPUR_ORGANIZATION_ID_IDX")]
    [Index(nameof(PersonId), Name = "DSPPUR_PERSON_ID_IDX")]
    [Index(nameof(PrimaryContactId), Name = "DSPPUR_PRIMARY_CONTACT_ID_IDX")]
    public partial class PimsDispositionPurchaser
    {
        [Key]
        [Column("DISPOSITION_PURCHASER_ID")]
        public long DispositionPurchaserId { get; set; }
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
        [InverseProperty(nameof(PimsDispositionSale.PimsDispositionPurchasers))]
        public virtual PimsDispositionSale DispositionSale { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsDispositionPurchasers))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsDispositionPurchaserPeople))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(PrimaryContactId))]
        [InverseProperty(nameof(PimsPerson.PimsDispositionPurchaserPrimaryContacts))]
        public virtual PimsPerson PrimaryContact { get; set; }
    }
}
