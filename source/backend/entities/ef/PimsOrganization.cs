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

    /// <summary>
    /// Incorporation number of the orgnization
    /// </summary>
    [Column("INCORPORATION_NUMBER")]
    [StringLength(50)]
    public string IncorporationNumber { get; set; }

    [Column("WEBSITE")]
    [StringLength(200)]
    public string Website { get; set; }

    [Column("COMMENT")]
    [StringLength(2000)]
    public string Comment { get; set; }

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
    public virtual ICollection<PimsLeaseStakeholder> PimsLeaseStakeholders { get; set; } = new List<PimsLeaseStakeholder>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsOrganizationAddress> PimsOrganizationAddresses { get; set; } = new List<PimsOrganizationAddress>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsPersonOrganization> PimsPersonOrganizations { get; set; } = new List<PimsPersonOrganization>();

    [InverseProperty("Organization")]
    public virtual ICollection<PimsPropActInvolvedParty> PimsPropActInvolvedParties { get; set; } = new List<PimsPropActInvolvedParty>();

    [InverseProperty("ServiceProviderOrg")]
    public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; } = new List<PimsPropertyActivity>();

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
