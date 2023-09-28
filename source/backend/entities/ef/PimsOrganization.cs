using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ORGANIZATION")]
    [Index(nameof(DistrictCode), Name = "ORG_DISTRICT_CODE_IDX")]
    [Index(nameof(OrganizationTypeCode), Name = "ORG_ORGANIZATION_TYPE_CODE_IDX")]
    [Index(nameof(OrgIdentifierTypeCode), Name = "ORG_ORG_IDENTIFIER_TYPE_CODE_IDX")]
    [Index(nameof(PrntOrganizationId), Name = "ORG_PRNT_ORGANIZATION_ID_IDX")]
    [Index(nameof(RegionCode), Name = "ORG_REGION_CODE_IDX")]
    public partial class PimsOrganization
    {
        public PimsOrganization()
        {
            InversePrntOrganization = new HashSet<PimsOrganization>();
            PimsAccessRequestOrganizations = new HashSet<PimsAccessRequestOrganization>();
            PimsContactMethods = new HashSet<PimsContactMethod>();
            PimsExpropriationPayments = new HashSet<PimsExpropriationPayment>();
            PimsInterestHolders = new HashSet<PimsInterestHolder>();
            PimsLeaseTenants = new HashSet<PimsLeaseTenant>();
            PimsOrganizationAddresses = new HashSet<PimsOrganizationAddress>();
            PimsPersonOrganizations = new HashSet<PimsPersonOrganization>();
            PimsProperties = new HashSet<PimsProperty>();
            PimsPropertyContacts = new HashSet<PimsPropertyContact>();
            PimsPropertyOrganizations = new HashSet<PimsPropertyOrganization>();
            PimsResearchFiles = new HashSet<PimsResearchFile>();
            PimsSecurityDepositHolders = new HashSet<PimsSecurityDepositHolder>();
            PimsSecurityDepositReturnHolders = new HashSet<PimsSecurityDepositReturnHolder>();
            PimsUserOrganizations = new HashSet<PimsUserOrganization>();
        }

        [Key]
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { get; set; }
        [Column("PRNT_ORGANIZATION_ID")]
        public long? PrntOrganizationId { get; set; }
        [Column("REGION_CODE")]
        public short? RegionCode { get; set; }
        [Column("DISTRICT_CODE")]
        public short? DistrictCode { get; set; }
        [Column("ORGANIZATION_TYPE_CODE")]
        [StringLength(20)]
        public string OrganizationTypeCode { get; set; }
        [Column("ORG_IDENTIFIER_TYPE_CODE")]
        [StringLength(20)]
        public string OrgIdentifierTypeCode { get; set; }
        [Column("ORGANIZATION_IDENTIFIER")]
        [StringLength(100)]
        public string OrganizationIdentifier { get; set; }
        [Required]
        [Column("ORGANIZATION_NAME")]
        [StringLength(200)]
        public string OrganizationName { get; set; }
        [Column("ORGANIZATION_ALIAS")]
        [StringLength(200)]
        public string OrganizationAlias { get; set; }
        [Column("INCORPORATION_NUMBER")]
        [StringLength(50)]
        public string IncorporationNumber { get; set; }
        [Column("WEBSITE")]
        [StringLength(200)]
        public string Website { get; set; }
        [Column("COMMENT")]
        [StringLength(2000)]
        public string Comment { get; set; }
        [Required]
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

        [ForeignKey(nameof(DistrictCode))]
        [InverseProperty(nameof(PimsDistrict.PimsOrganizations))]
        public virtual PimsDistrict DistrictCodeNavigation { get; set; }
        [ForeignKey(nameof(OrgIdentifierTypeCode))]
        [InverseProperty(nameof(PimsOrgIdentifierType.PimsOrganizations))]
        public virtual PimsOrgIdentifierType OrgIdentifierTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(OrganizationTypeCode))]
        [InverseProperty(nameof(PimsOrganizationType.PimsOrganizations))]
        public virtual PimsOrganizationType OrganizationTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PrntOrganizationId))]
        [InverseProperty(nameof(PimsOrganization.InversePrntOrganization))]
        public virtual PimsOrganization PrntOrganization { get; set; }
        [ForeignKey(nameof(RegionCode))]
        [InverseProperty(nameof(PimsRegion.PimsOrganizations))]
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsOrganization.PrntOrganization))]
        public virtual ICollection<PimsOrganization> InversePrntOrganization { get; set; }
        [InverseProperty(nameof(PimsAccessRequestOrganization.Organization))]
        public virtual ICollection<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; }
        [InverseProperty(nameof(PimsContactMethod.Organization))]
        public virtual ICollection<PimsContactMethod> PimsContactMethods { get; set; }
        [InverseProperty(nameof(PimsExpropriationPayment.ExpropriatingAuthorityNavigation))]
        public virtual ICollection<PimsExpropriationPayment> PimsExpropriationPayments { get; set; }
        [InverseProperty(nameof(PimsInterestHolder.Organization))]
        public virtual ICollection<PimsInterestHolder> PimsInterestHolders { get; set; }
        [InverseProperty(nameof(PimsLeaseTenant.Organization))]
        public virtual ICollection<PimsLeaseTenant> PimsLeaseTenants { get; set; }
        [InverseProperty(nameof(PimsOrganizationAddress.Organization))]
        public virtual ICollection<PimsOrganizationAddress> PimsOrganizationAddresses { get; set; }
        [InverseProperty(nameof(PimsPersonOrganization.Organization))]
        public virtual ICollection<PimsPersonOrganization> PimsPersonOrganizations { get; set; }
        [InverseProperty(nameof(PimsProperty.PropMgmtOrg))]
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
        [InverseProperty(nameof(PimsPropertyContact.Organization))]
        public virtual ICollection<PimsPropertyContact> PimsPropertyContacts { get; set; }
        [InverseProperty(nameof(PimsPropertyOrganization.Organization))]
        public virtual ICollection<PimsPropertyOrganization> PimsPropertyOrganizations { get; set; }
        [InverseProperty(nameof(PimsResearchFile.RequestorOrganizationNavigation))]
        public virtual ICollection<PimsResearchFile> PimsResearchFiles { get; set; }
        [InverseProperty(nameof(PimsSecurityDepositHolder.Organization))]
        public virtual ICollection<PimsSecurityDepositHolder> PimsSecurityDepositHolders { get; set; }
        [InverseProperty(nameof(PimsSecurityDepositReturnHolder.Organization))]
        public virtual ICollection<PimsSecurityDepositReturnHolder> PimsSecurityDepositReturnHolders { get; set; }
        [InverseProperty(nameof(PimsUserOrganization.Organization))]
        public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; }
    }
}
