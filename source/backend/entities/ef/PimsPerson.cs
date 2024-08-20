using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_PERSON")]
public partial class PimsPerson
{
    [Key]
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

    [Column("BIRTH_DATE")]
    public DateOnly? BirthDate { get; set; }

    [Column("COMMENT")]
    [StringLength(2000)]
    public string Comment { get; set; }

    [Column("ADDRESS_COMMENT")]
    [StringLength(2000)]
    public string AddressComment { get; set; }

    [Column("USE_ORGANIZATION_ADDRESS")]
    public bool? UseOrganizationAddress { get; set; }

    [Column("PIMS_PROPERTY_ACTIVITY_ID")]
    public long? PimsPropertyActivityId { get; set; }

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

    [InverseProperty("Person")]
    public virtual ICollection<PimsAcquisitionFileTeam> PimsAcquisitionFileTeamPeople { get; set; } = new List<PimsAcquisitionFileTeam>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsAcquisitionFileTeam> PimsAcquisitionFileTeamPrimaryContacts { get; set; } = new List<PimsAcquisitionFileTeam>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsContactMethod> PimsContactMethods { get; set; } = new List<PimsContactMethod>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsDispositionFileTeam> PimsDispositionFileTeamPeople { get; set; } = new List<PimsDispositionFileTeam>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsDispositionFileTeam> PimsDispositionFileTeamPrimaryContacts { get; set; } = new List<PimsDispositionFileTeam>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsDispositionPurchaser> PimsDispositionPurchaserPeople { get; set; } = new List<PimsDispositionPurchaser>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsDispositionPurchaser> PimsDispositionPurchaserPrimaryContacts { get; set; } = new List<PimsDispositionPurchaser>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsDspPurchAgent> PimsDspPurchAgentPeople { get; set; } = new List<PimsDspPurchAgent>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsDspPurchAgent> PimsDspPurchAgentPrimaryContacts { get; set; } = new List<PimsDspPurchAgent>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsDspPurchSolicitor> PimsDspPurchSolicitorPeople { get; set; } = new List<PimsDspPurchSolicitor>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsDspPurchSolicitor> PimsDspPurchSolicitorPrimaryContacts { get; set; } = new List<PimsDspPurchSolicitor>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsInterestHolder> PimsInterestHolderPeople { get; set; } = new List<PimsInterestHolder>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsInterestHolder> PimsInterestHolderPrimaryContacts { get; set; } = new List<PimsInterestHolder>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsLeaseConsultation> PimsLeaseConsultationPeople { get; set; } = new List<PimsLeaseConsultation>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsLeaseConsultation> PimsLeaseConsultationPrimaryContacts { get; set; } = new List<PimsLeaseConsultation>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsLeaseStakeholder> PimsLeaseStakeholderPeople { get; set; } = new List<PimsLeaseStakeholder>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsLeaseStakeholder> PimsLeaseStakeholderPrimaryContacts { get; set; } = new List<PimsLeaseStakeholder>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsPersonAddress> PimsPersonAddresses { get; set; } = new List<PimsPersonAddress>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsPersonOrganization> PimsPersonOrganizations { get; set; } = new List<PimsPersonOrganization>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsProjectPerson> PimsProjectPeople { get; set; } = new List<PimsProjectPerson>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsPropActInvolvedParty> PimsPropActInvolvedParties { get; set; } = new List<PimsPropActInvolvedParty>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsPropActMinContact> PimsPropActMinContacts { get; set; } = new List<PimsPropActMinContact>();

    [InverseProperty("ServiceProviderPerson")]
    public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; } = new List<PimsPropertyActivity>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsPropertyContact> PimsPropertyContactPeople { get; set; } = new List<PimsPropertyContact>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsPropertyContact> PimsPropertyContactPrimaryContacts { get; set; } = new List<PimsPropertyContact>();

    [InverseProperty("RequestorNameNavigation")]
    public virtual ICollection<PimsResearchFile> PimsResearchFiles { get; set; } = new List<PimsResearchFile>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsSecurityDepositHolder> PimsSecurityDepositHolders { get; set; } = new List<PimsSecurityDepositHolder>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsSecurityDepositReturnHolder> PimsSecurityDepositReturnHolders { get; set; } = new List<PimsSecurityDepositReturnHolder>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsUser> PimsUsers { get; set; } = new List<PimsUser>();
}
