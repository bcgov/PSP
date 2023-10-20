﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PERSON")]
    public partial class PimsPerson
    {
        public PimsPerson()
        {
            PimsAcquisitionFileTeamPeople = new HashSet<PimsAcquisitionFileTeam>();
            PimsAcquisitionFileTeamPrimaryContacts = new HashSet<PimsAcquisitionFileTeam>();
            PimsContactMethods = new HashSet<PimsContactMethod>();
            PimsInterestHolderPeople = new HashSet<PimsInterestHolder>();
            PimsInterestHolderPrimaryContacts = new HashSet<PimsInterestHolder>();
            PimsLeaseTenantPeople = new HashSet<PimsLeaseTenant>();
            PimsLeaseTenantPrimaryContacts = new HashSet<PimsLeaseTenant>();
            PimsPersonAddresses = new HashSet<PimsPersonAddress>();
            PimsPersonOrganizations = new HashSet<PimsPersonOrganization>();
            PimsProjectPeople = new HashSet<PimsProjectPerson>();
            PimsPropActInvolvedParties = new HashSet<PimsPropActInvolvedParty>();
            PimsPropActMinContacts = new HashSet<PimsPropActMinContact>();
            PimsProperties = new HashSet<PimsProperty>();
            PimsPropertyActivities = new HashSet<PimsPropertyActivity>();
            PimsPropertyContactPeople = new HashSet<PimsPropertyContact>();
            PimsPropertyContactPrimaryContacts = new HashSet<PimsPropertyContact>();
            PimsResearchFiles = new HashSet<PimsResearchFile>();
            PimsSecurityDepositHolders = new HashSet<PimsSecurityDepositHolder>();
            PimsSecurityDepositReturnHolders = new HashSet<PimsSecurityDepositReturnHolder>();
            PimsUsers = new HashSet<PimsUser>();
        }

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
        [Column("BIRTH_DATE", TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        [Column("COMMENT")]
        [StringLength(2000)]
        public string Comment { get; set; }
        [Column("ADDRESS_COMMENT")]
        [StringLength(2000)]
        public string AddressComment { get; set; }
        [Column("USE_ORGANIZATION_ADDRESS")]
        public bool? UseOrganizationAddress { get; set; }
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
        [Column("PIMS_PROPERTY_ACTIVITY_ID")]
        public long? PimsPropertyActivityId { get; set; }

        [InverseProperty(nameof(PimsAcquisitionFileTeam.Person))]
        public virtual ICollection<PimsAcquisitionFileTeam> PimsAcquisitionFileTeamPeople { get; set; }
        [InverseProperty(nameof(PimsAcquisitionFileTeam.PrimaryContact))]
        public virtual ICollection<PimsAcquisitionFileTeam> PimsAcquisitionFileTeamPrimaryContacts { get; set; }
        [InverseProperty(nameof(PimsContactMethod.Person))]
        public virtual ICollection<PimsContactMethod> PimsContactMethods { get; set; }
        [InverseProperty(nameof(PimsInterestHolder.Person))]
        public virtual ICollection<PimsInterestHolder> PimsInterestHolderPeople { get; set; }
        [InverseProperty(nameof(PimsInterestHolder.PrimaryContact))]
        public virtual ICollection<PimsInterestHolder> PimsInterestHolderPrimaryContacts { get; set; }
        [InverseProperty(nameof(PimsLeaseTenant.Person))]
        public virtual ICollection<PimsLeaseTenant> PimsLeaseTenantPeople { get; set; }
        [InverseProperty(nameof(PimsLeaseTenant.PrimaryContact))]
        public virtual ICollection<PimsLeaseTenant> PimsLeaseTenantPrimaryContacts { get; set; }
        [InverseProperty(nameof(PimsPersonAddress.Person))]
        public virtual ICollection<PimsPersonAddress> PimsPersonAddresses { get; set; }
        [InverseProperty(nameof(PimsPersonOrganization.Person))]
        public virtual ICollection<PimsPersonOrganization> PimsPersonOrganizations { get; set; }
        [InverseProperty(nameof(PimsProjectPerson.Person))]
        public virtual ICollection<PimsProjectPerson> PimsProjectPeople { get; set; }
        [InverseProperty(nameof(PimsPropActInvolvedParty.Person))]
        public virtual ICollection<PimsPropActInvolvedParty> PimsPropActInvolvedParties { get; set; }
        [InverseProperty(nameof(PimsPropActMinContact.Person))]
        public virtual ICollection<PimsPropActMinContact> PimsPropActMinContacts { get; set; }
        [InverseProperty(nameof(PimsProperty.PropertyManager))]
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
        [InverseProperty(nameof(PimsPropertyActivity.ServiceProviderPerson))]
        public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; }
        [InverseProperty(nameof(PimsPropertyContact.Person))]
        public virtual ICollection<PimsPropertyContact> PimsPropertyContactPeople { get; set; }
        [InverseProperty(nameof(PimsPropertyContact.PrimaryContact))]
        public virtual ICollection<PimsPropertyContact> PimsPropertyContactPrimaryContacts { get; set; }
        [InverseProperty(nameof(PimsResearchFile.RequestorNameNavigation))]
        public virtual ICollection<PimsResearchFile> PimsResearchFiles { get; set; }
        [InverseProperty(nameof(PimsSecurityDepositHolder.Person))]
        public virtual ICollection<PimsSecurityDepositHolder> PimsSecurityDepositHolders { get; set; }
        [InverseProperty(nameof(PimsSecurityDepositReturnHolder.Person))]
        public virtual ICollection<PimsSecurityDepositReturnHolder> PimsSecurityDepositReturnHolders { get; set; }
        [InverseProperty(nameof(PimsUser.Person))]
        public virtual ICollection<PimsUser> PimsUsers { get; set; }
    }
}
