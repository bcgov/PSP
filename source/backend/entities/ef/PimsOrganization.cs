using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Information related to an organization identified in the PSP system.
/// </summary>
[Table("PIMS_ORGANIZATION")]
[Index("DistrictCode", Name = "ORG_DISTRICT_CODE_IDX")]
[Index("OrganizationTypeCode", Name = "ORG_ORGANIZATION_TYPE_CODE_IDX")]
[Index("OrgIdentifierTypeCode", Name = "ORG_ORG_IDENTIFIER_TYPE_CODE_IDX")]
[Index("PrntOrganizationId", Name = "ORG_PRNT_ORGANIZATION_ID_IDX")]
[Index("RegionCode", Name = "ORG_REGION_CODE_IDX")]
public partial class PimsOrganization
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("ORGANIZATION_ID")]
    public long OrganizationId { get; set; }

    /// <summary>
    /// Recursive foreign key to the parent organization.
    /// </summary>
    [Column("PRNT_ORGANIZATION_ID")]
    public long? PrntOrganizationId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_REGION table.
    /// </summary>
    [Column("REGION_CODE")]
    public short? RegionCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_DISTRICT table.
    /// </summary>
    [Column("DISTRICT_CODE")]
    public short? DistrictCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ORGANIZATION_TYPE table.
    /// </summary>
    [Column("ORGANIZATION_TYPE_CODE")]
    [StringLength(20)]
    public string OrganizationTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ORG_IDENTIFIER_TYPE table.
    /// </summary>
    [Column("ORG_IDENTIFIER_TYPE_CODE")]
    [StringLength(20)]
    public string OrgIdentifierTypeCode { get; set; }

    /// <summary>
    /// Identifier assigned to the organization.
    /// </summary>
    [Column("ORGANIZATION_IDENTIFIER")]
    [StringLength(100)]
    public string OrganizationIdentifier { get; set; }

    /// <summary>
    /// Name assigned to the organization.
    /// </summary>
    [Required]
    [Column("ORGANIZATION_NAME")]
    [StringLength(200)]
    public string OrganizationName { get; set; }

    /// <summary>
    /// Alias assigned to the organization.
    /// </summary>
    [Column("ORGANIZATION_ALIAS")]
    [StringLength(200)]
    public string OrganizationAlias { get; set; }

    /// <summary>
    /// Incorporation number of the orgnization
    /// </summary>
    [Column("INCORPORATION_NUMBER")]
    [StringLength(50)]
    public string IncorporationNumber { get; set; }

    /// <summary>
    /// URL to the organization&apos;s web site.
    /// </summary>
    [Column("WEBSITE")]
    [StringLength(200)]
    public string Website { get; set; }

    /// <summary>
    /// Comment accompanying the organization.
    /// </summary>
    [Column("COMMENT")]
    [StringLength(2000)]
    public string Comment { get; set; }

    /// <summary>
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("DistrictCode")]
    [InverseProperty("PimsOrganizations")]
    public virtual PimsDistrict DistrictCodeNavigation { get; set; }

    [InverseProperty("PrntOrganization")]
    public virtual ICollection<PimsOrganization> InversePrntOrganization { get; set; } = new List<PimsOrganization>();

    [ForeignKey("OrgIdentifierTypeCode")]
    [InverseProperty("PimsOrganizations")]
    public virtual PimsOrgIdentifierType OrgIdentifierTypeCodeNavigation { get; set; }

    [ForeignKey("OrganizationTypeCode")]
    [InverseProperty("PimsOrganizations")]
    public virtual PimsOrganizationType OrganizationTypeCodeNavigation { get; set; }

    [InverseProperty("Organization")]
    public virtual ICollection<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; } = new List<PimsAccessRequestOrganization>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsAcquisitionFileTeam> PimsAcquisitionFileTeams { get; set; } = new List<PimsAcquisitionFileTeam>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsContactMethod> PimsContactMethods { get; set; } = new List<PimsContactMethod>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsDispositionFileTeam> PimsDispositionFileTeams { get; set; } = new List<PimsDispositionFileTeam>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsDispositionPurchaser> PimsDispositionPurchasers { get; set; } = new List<PimsDispositionPurchaser>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsDspPurchAgent> PimsDspPurchAgents { get; set; } = new List<PimsDspPurchAgent>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsDspPurchSolicitor> PimsDspPurchSolicitors { get; set; } = new List<PimsDspPurchSolicitor>();

    [InverseProperty("ExpropriatingAuthorityNavigation")]
    public virtual ICollection<PimsExpropriationPayment> PimsExpropriationPayments { get; set; } = new List<PimsExpropriationPayment>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsInterestHolder> PimsInterestHolders { get; set; } = new List<PimsInterestHolder>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsLeaseConsultation> PimsLeaseConsultations { get; set; } = new List<PimsLeaseConsultation>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsLeaseLicenseTeam> PimsLeaseLicenseTeams { get; set; } = new List<PimsLeaseLicenseTeam>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsLeaseStakeholder> PimsLeaseStakeholders { get; set; } = new List<PimsLeaseStakeholder>();

    [InverseProperty("RequestorOrganization")]
    public virtual ICollection<PimsManagementActivity> PimsManagementActivityRequestorOrganizations { get; set; } = new List<PimsManagementActivity>();

    [InverseProperty("ServiceProviderOrg")]
    public virtual ICollection<PimsManagementActivity> PimsManagementActivityServiceProviderOrgs { get; set; } = new List<PimsManagementActivity>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsManagementFileContact> PimsManagementFileContacts { get; set; } = new List<PimsManagementFileContact>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsManagementFileTeam> PimsManagementFileTeams { get; set; } = new List<PimsManagementFileTeam>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsMgmtActInvolvedParty> PimsMgmtActInvolvedParties { get; set; } = new List<PimsMgmtActInvolvedParty>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsOrganizationAddress> PimsOrganizationAddresses { get; set; } = new List<PimsOrganizationAddress>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsPersonOrganization> PimsPersonOrganizations { get; set; } = new List<PimsPersonOrganization>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsPropertyContact> PimsPropertyContacts { get; set; } = new List<PimsPropertyContact>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsPropertyOrganization> PimsPropertyOrganizations { get; set; } = new List<PimsPropertyOrganization>();

    [InverseProperty("RequestorOrganizationNavigation")]
    public virtual ICollection<PimsResearchFile> PimsResearchFiles { get; set; } = new List<PimsResearchFile>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsSecurityDepositHolder> PimsSecurityDepositHolders { get; set; } = new List<PimsSecurityDepositHolder>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsSecurityDepositReturnHolder> PimsSecurityDepositReturnHolders { get; set; } = new List<PimsSecurityDepositReturnHolder>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; } = new List<PimsUserOrganization>();

    [ForeignKey("PrntOrganizationId")]
    [InverseProperty("InversePrntOrganization")]
    public virtual PimsOrganization PrntOrganization { get; set; }

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsOrganizations")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }
}
