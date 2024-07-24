using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Defines the activities that are associated with this property.
/// </summary>
[Table("PIMS_PROPERTY_ACTIVITY")]
[Index("PropMgmtActivityStatusTypeCode", Name = "PRPACT_PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE_IDX")]
[Index("PropMgmtActivitySubtypeCode", Name = "PRPACT_PROP_MGMT_ACTIVITY_SUBTYPE_CODE_IDX")]
[Index("PropMgmtActivityTypeCode", Name = "PRPACT_PROP_MGMT_ACTIVITY_TYPE_CODE_IDX")]
[Index("ServiceProviderOrgId", Name = "PRPACT_SERVICE_PROVIDER_ORG_ID_IDX")]
[Index("ServiceProviderPersonId", Name = "PRPACT_SERVICE_PROVIDER_PERSON_ID_IDX")]
public partial class PimsPropertyActivity
{
    [Key]
    [Column("PIMS_PROPERTY_ACTIVITY_ID")]
    public long PimsPropertyActivityId { get; set; }

    /// <summary>
    /// Type of property management activity.
    /// </summary>
    [Required]
    [Column("PROP_MGMT_ACTIVITY_TYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivityTypeCode { get; set; }

    /// <summary>
    /// Subtype of property management activity.
    /// </summary>
    [Required]
    [Column("PROP_MGMT_ACTIVITY_SUBTYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivitySubtypeCode { get; set; }

    /// <summary>
    /// Status of the property management activity.
    /// </summary>
    [Required]
    [Column("PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivityStatusTypeCode { get; set; }

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
    [StringLength(500)]
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

    [InverseProperty("PimsPropertyActivity")]
    public virtual ICollection<PimsPropActInvolvedParty> PimsPropActInvolvedParties { get; set; } = new List<PimsPropActInvolvedParty>();

    [InverseProperty("PimsPropertyActivity")]
    public virtual ICollection<PimsPropActMinContact> PimsPropActMinContacts { get; set; } = new List<PimsPropActMinContact>();

    [InverseProperty("PimsPropertyActivity")]
    public virtual ICollection<PimsPropPropActivity> PimsPropPropActivities { get; set; } = new List<PimsPropPropActivity>();

    [InverseProperty("PimsPropertyActivity")]
    public virtual ICollection<PimsPropertyActivityDocument> PimsPropertyActivityDocuments { get; set; } = new List<PimsPropertyActivityDocument>();

    [InverseProperty("PimsPropertyActivity")]
    public virtual ICollection<PimsPropertyActivityInvoice> PimsPropertyActivityInvoices { get; set; } = new List<PimsPropertyActivityInvoice>();

    [ForeignKey("PropMgmtActivityStatusTypeCode")]
    [InverseProperty("PimsPropertyActivities")]
    public virtual PimsPropMgmtActivityStatusType PropMgmtActivityStatusTypeCodeNavigation { get; set; }

    [ForeignKey("PropMgmtActivitySubtypeCode")]
    [InverseProperty("PimsPropertyActivities")]
    public virtual PimsPropMgmtActivitySubtype PropMgmtActivitySubtypeCodeNavigation { get; set; }

    [ForeignKey("PropMgmtActivityTypeCode")]
    [InverseProperty("PimsPropertyActivities")]
    public virtual PimsPropMgmtActivityType PropMgmtActivityTypeCodeNavigation { get; set; }

    [ForeignKey("ServiceProviderOrgId")]
    [InverseProperty("PimsPropertyActivities")]
    public virtual PimsOrganization ServiceProviderOrg { get; set; }

    [ForeignKey("ServiceProviderPersonId")]
    [InverseProperty("PimsPropertyActivities")]
    public virtual PimsPerson ServiceProviderPerson { get; set; }
}
