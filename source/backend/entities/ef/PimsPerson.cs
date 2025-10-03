using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_PERSON")]
public partial class PimsPerson
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("PERSON_ID")]
    public long PersonId { get; set; }

    /// <summary>
    /// Person&apos;s surname.
    /// </summary>
    [Required]
    [Column("SURNAME")]
    [StringLength(50)]
    public string Surname { get; set; }

    /// <summary>
    /// Person&apos;s first name.
    /// </summary>
    [Required]
    [Column("FIRST_NAME")]
    [StringLength(50)]
    public string FirstName { get; set; }

    /// <summary>
    /// Person&apos;s middle name(s).
    /// </summary>
    [Column("MIDDLE_NAMES")]
    [StringLength(200)]
    public string MiddleNames { get; set; }

    /// <summary>
    /// Person&apos;s name suffix.
    /// </summary>
    [Column("NAME_SUFFIX")]
    [StringLength(50)]
    public string NameSuffix { get; set; }

    /// <summary>
    /// Person&apos;s preferred name.
    /// </summary>
    [Column("PREFERRED_NAME")]
    [StringLength(200)]
    public string PreferredName { get; set; }

    /// <summary>
    /// Person&apos;s date of birth.
    /// </summary>
    [Column("BIRTH_DATE")]
    public DateOnly? BirthDate { get; set; }

    /// <summary>
    /// Comment associated with the person.
    /// </summary>
    [Column("COMMENT")]
    [StringLength(2000)]
    public string Comment { get; set; }

    /// <summary>
    /// Comment associated with the person&apos;s address.
    /// </summary>
    [Column("ADDRESS_COMMENT")]
    [StringLength(2000)]
    public string AddressComment { get; set; }

    /// <summary>
    /// Indicates if the organization&apos;s address si preferred.
    /// </summary>
    [Column("USE_ORGANIZATION_ADDRESS")]
    public bool? UseOrganizationAddress { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY_ACTIVITY table.
    /// </summary>
    [Column("PIMS_PROPERTY_ACTIVITY_ID")]
    public long? PimsPropertyActivityId { get; set; }

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
    public virtual ICollection<PimsLeaseLicenseTeam> PimsLeaseLicenseTeamPeople { get; set; } = new List<PimsLeaseLicenseTeam>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsLeaseLicenseTeam> PimsLeaseLicenseTeamPrimaryContacts { get; set; } = new List<PimsLeaseLicenseTeam>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsLeaseStakeholder> PimsLeaseStakeholderPeople { get; set; } = new List<PimsLeaseStakeholder>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsLeaseStakeholder> PimsLeaseStakeholderPrimaryContacts { get; set; } = new List<PimsLeaseStakeholder>();

    [InverseProperty("RequestorPerson")]
    public virtual ICollection<PimsManagementActivity> PimsManagementActivityRequestorPeople { get; set; } = new List<PimsManagementActivity>();

    [InverseProperty("RequestorPrimaryContact")]
    public virtual ICollection<PimsManagementActivity> PimsManagementActivityRequestorPrimaryContacts { get; set; } = new List<PimsManagementActivity>();

    [InverseProperty("ServiceProviderPerson")]
    public virtual ICollection<PimsManagementActivity> PimsManagementActivityServiceProviderPeople { get; set; } = new List<PimsManagementActivity>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsManagementFileContact> PimsManagementFileContactPeople { get; set; } = new List<PimsManagementFileContact>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsManagementFileContact> PimsManagementFileContactPrimaryContacts { get; set; } = new List<PimsManagementFileContact>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsManagementFileTeam> PimsManagementFileTeamPeople { get; set; } = new List<PimsManagementFileTeam>();

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<PimsManagementFileTeam> PimsManagementFileTeamPrimaryContacts { get; set; } = new List<PimsManagementFileTeam>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsMgmtActInvolvedParty> PimsMgmtActInvolvedParties { get; set; } = new List<PimsMgmtActInvolvedParty>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsMgmtActMinContact> PimsMgmtActMinContacts { get; set; } = new List<PimsMgmtActMinContact>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsPersonAddress> PimsPersonAddresses { get; set; } = new List<PimsPersonAddress>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsPersonOrganization> PimsPersonOrganizations { get; set; } = new List<PimsPersonOrganization>();

    [InverseProperty("Person")]
    public virtual ICollection<PimsProjectPerson> PimsProjectPeople { get; set; } = new List<PimsProjectPerson>();

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
