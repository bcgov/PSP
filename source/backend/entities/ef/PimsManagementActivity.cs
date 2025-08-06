using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Defines the activities that are associated with this property.
/// </summary>
[Table("PIMS_MANAGEMENT_ACTIVITY")]
[Index("ManagementFileId", Name = "MGMTAC_MANAGEMENT_FILE_ID_IDX")]
[Index("MgmtActivityStatusTypeCode", Name = "MGMTAC_MGMT_ACTIVITY_STATUS_TYPE_CODE_IDX")]
[Index("MgmtActivityTypeCode", Name = "MGMTAC_MGMT_ACTIVITY_TYPE_CODE_IDX")]
[Index("ServiceProviderOrgId", Name = "MGMTAC_SERVICE_PROVIDER_ORG_ID_IDX")]
[Index("ServiceProviderPersonId", Name = "MGMTAC_SERVICE_PROVIDER_PERSON_ID_IDX")]
public partial class PimsManagementActivity
{
    [Key]
    [Column("MANAGEMENT_ACTIVITY_ID")]
    public long ManagementActivityId { get; set; }

    /// <summary>
    /// Status of the property management activity.
    /// </summary>
    [Required]
    [Column("MGMT_ACTIVITY_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string MgmtActivityStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key of the person as a service provider.
    /// </summary>
    [Column("SERVICE_PROVIDER_PERSON_ID")]
    public long? ServiceProviderPersonId { get; set; }

    /// <summary>
    /// Foreign key of the organization as a service provider.
    /// </summary>
    [Column("SERVICE_PROVIDER_ORG_ID")]
    public long? ServiceProviderOrgId { get; set; }

    /// <summary>
    /// Foreign key of the associated management file (PIMS_MANAGEMENT_FILE).
    /// </summary>
    [Column("MANAGEMENT_FILE_ID")]
    public long? ManagementFileId { get; set; }

    /// <summary>
    /// Foreign key of the associated management activity type (PIMS_PROP_MGMT_ACTIVITY_TYPE).
    /// </summary>
    [Column("MGMT_ACTIVITY_TYPE_CODE")]
    [StringLength(20)]
    public string MgmtActivityTypeCode { get; set; }

    /// <summary>
    /// Date the request for a property management activity was added
    /// </summary>
    [Column("REQUEST_ADDED_DT")]
    public DateOnly RequestAddedDt { get; set; }

    /// <summary>
    /// Date the property management activity was completed.
    /// </summary>
    [Column("COMPLETION_DT")]
    public DateOnly? CompletionDt { get; set; }

    /// <summary>
    /// Description of the property management activity.
    /// </summary>
    [Column("DESCRIPTION")]
    public string Description { get; set; }

    /// <summary>
    /// Source of the management activity request.
    /// </summary>
    [Column("REQUEST_SOURCE")]
    [StringLength(2000)]
    public string RequestSource { get; set; }

    /// <summary>
    /// Indicates if the code is disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

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
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
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

    [ForeignKey("ManagementFileId")]
    [InverseProperty("PimsManagementActivities")]
    public virtual PimsManagementFile ManagementFile { get; set; }

    [ForeignKey("MgmtActivityStatusTypeCode")]
    [InverseProperty("PimsManagementActivities")]
    public virtual PimsMgmtActivityStatusType MgmtActivityStatusTypeCodeNavigation { get; set; }

    [ForeignKey("MgmtActivityTypeCode")]
    [InverseProperty("PimsManagementActivities")]
    public virtual PimsMgmtActivityType MgmtActivityTypeCodeNavigation { get; set; }

    [InverseProperty("ManagementActivity")]
    public virtual ICollection<PimsManagementActivityInvoice> PimsManagementActivityInvoices { get; set; } = new List<PimsManagementActivityInvoice>();

    [InverseProperty("ManagementActivity")]
    public virtual ICollection<PimsManagementActivityProperty> PimsManagementActivityProperties { get; set; } = new List<PimsManagementActivityProperty>();

    [InverseProperty("ManagementActivity")]
    public virtual ICollection<PimsMgmtActInvolvedParty> PimsMgmtActInvolvedParties { get; set; } = new List<PimsMgmtActInvolvedParty>();

    [InverseProperty("ManagementActivity")]
    public virtual ICollection<PimsMgmtActMinContact> PimsMgmtActMinContacts { get; set; } = new List<PimsMgmtActMinContact>();

    [InverseProperty("ManagementActivity")]
    public virtual ICollection<PimsMgmtActivityActivitySubtyp> PimsMgmtActivityActivitySubtyps { get; set; } = new List<PimsMgmtActivityActivitySubtyp>();

    [InverseProperty("ManagementActivity")]
    public virtual ICollection<PimsMgmtActivityDocument> PimsMgmtActivityDocuments { get; set; } = new List<PimsMgmtActivityDocument>();

    [ForeignKey("ServiceProviderOrgId")]
    [InverseProperty("PimsManagementActivities")]
    public virtual PimsOrganization ServiceProviderOrg { get; set; }

    [ForeignKey("ServiceProviderPersonId")]
    [InverseProperty("PimsManagementActivities")]
    public virtual PimsPerson ServiceProviderPerson { get; set; }
}
