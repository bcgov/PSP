using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_INTEREST_HOLDER")]
    [Index(nameof(AcquisitionFileId), Name = "INTHLD_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(InterestHolderTypeCode), Name = "INTHLD_INTEREST_HOLDER_TYPE_CODE_IDX")]
    [Index(nameof(OrganizationId), Name = "INTHLD_ORGANIZATION_ID_IDX")]
    [Index(nameof(PersonId), Name = "INTHLD_PERSON_ID_IDX")]
    [Index(nameof(PrimaryContactId), Name = "INTHLD_PRIMARY_CONTACT_ID_IDX")]
    public partial class PimsInterestHolder
    {
        public PimsInterestHolder()
        {
            PimsCompensationRequisitions = new HashSet<PimsCompensationRequisition>();
            PimsExpropriationPayments = new HashSet<PimsExpropriationPayment>();
            PimsInthldrPropInterests = new HashSet<PimsInthldrPropInterest>();
        }

        [Key]
        [Column("INTEREST_HOLDER_ID")]
        public long InterestHolderId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }
        [Column("PRIMARY_CONTACT_ID")]
        public long? PrimaryContactId { get; set; }
        [Required]
        [Column("INTEREST_HOLDER_TYPE_CODE")]
        [StringLength(20)]
        public string InterestHolderTypeCode { get; set; }
        [Column("COMMENT")]
        [StringLength(2000)]
        public string Comment { get; set; }
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

        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsInterestHolders))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [ForeignKey(nameof(InterestHolderTypeCode))]
        [InverseProperty(nameof(PimsInterestHolderType.PimsInterestHolders))]
        public virtual PimsInterestHolderType InterestHolderTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsInterestHolders))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsInterestHolderPeople))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(PrimaryContactId))]
        [InverseProperty(nameof(PimsPerson.PimsInterestHolderPrimaryContacts))]
        public virtual PimsPerson PrimaryContact { get; set; }
        [InverseProperty(nameof(PimsCompensationRequisition.InterestHolder))]
        public virtual ICollection<PimsCompensationRequisition> PimsCompensationRequisitions { get; set; }
        [InverseProperty(nameof(PimsExpropriationPayment.InterestHolder))]
        public virtual ICollection<PimsExpropriationPayment> PimsExpropriationPayments { get; set; }
        [InverseProperty(nameof(PimsInthldrPropInterest.InterestHolder))]
        public virtual ICollection<PimsInthldrPropInterest> PimsInthldrPropInterests { get; set; }
    }
}
