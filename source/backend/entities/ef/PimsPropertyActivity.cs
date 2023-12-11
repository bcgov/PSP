using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY_ACTIVITY")]
    [Index(nameof(PropMgmtActivityStatusTypeCode), Name = "PRPACT_PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(PropMgmtActivitySubtypeCode), Name = "PRPACT_PROP_MGMT_ACTIVITY_SUBTYPE_CODE_IDX")]
    [Index(nameof(PropMgmtActivityTypeCode), Name = "PRPACT_PROP_MGMT_ACTIVITY_TYPE_CODE_IDX")]
    [Index(nameof(ServiceProviderOrgId), Name = "PRPACT_SERVICE_PROVIDER_ORG_ID_IDX")]
    [Index(nameof(ServiceProviderPersonId), Name = "PRPACT_SERVICE_PROVIDER_PERSON_ID_IDX")]
    public partial class PimsPropertyActivity
    {
        public PimsPropertyActivity()
        {
            PimsPropActInvolvedParties = new HashSet<PimsPropActInvolvedParty>();
            PimsPropActMinContacts = new HashSet<PimsPropActMinContact>();
            PimsPropPropActivities = new HashSet<PimsPropPropActivity>();
            PimsPropertyActivityDocuments = new HashSet<PimsPropertyActivityDocument>();
            PimsPropertyActivityInvoices = new HashSet<PimsPropertyActivityInvoice>();
        }

        [Key]
        [Column("PIMS_PROPERTY_ACTIVITY_ID")]
        public long PimsPropertyActivityId { get; set; }
        [Required]
        [Column("PROP_MGMT_ACTIVITY_TYPE_CODE")]
        [StringLength(20)]
        public string PropMgmtActivityTypeCode { get; set; }
        [Required]
        [Column("PROP_MGMT_ACTIVITY_SUBTYPE_CODE")]
        [StringLength(20)]
        public string PropMgmtActivitySubtypeCode { get; set; }
        [Required]
        [Column("PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string PropMgmtActivityStatusTypeCode { get; set; }
        [Column("SERVICE_PROVIDER_PERSON_ID")]
        public long? ServiceProviderPersonId { get; set; }
        [Column("SERVICE_PROVIDER_ORG_ID")]
        public long? ServiceProviderOrgId { get; set; }
        [Column("REQUEST_ADDED_DT", TypeName = "date")]
        public DateTime RequestAddedDt { get; set; }
        [Column("COMPLETION_DT", TypeName = "date")]
        public DateTime? CompletionDt { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(2000)]
        public string Description { get; set; }
        [Column("REQUEST_SOURCE")]
        [StringLength(2000)]
        public string RequestSource { get; set; }
        [Column("PRETAX_AMT", TypeName = "money")]
        public decimal? PretaxAmt { get; set; }
        [Column("GST_AMT", TypeName = "money")]
        public decimal? GstAmt { get; set; }
        [Column("PST_AMT", TypeName = "money")]
        public decimal? PstAmt { get; set; }
        [Column("TOTAL_AMT", TypeName = "money")]
        public decimal? TotalAmt { get; set; }
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

        [ForeignKey(nameof(PropMgmtActivityStatusTypeCode))]
        [InverseProperty(nameof(PimsPropMgmtActivityStatusType.PimsPropertyActivities))]
        public virtual PimsPropMgmtActivityStatusType PropMgmtActivityStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropMgmtActivitySubtypeCode))]
        [InverseProperty(nameof(PimsPropMgmtActivitySubtype.PimsPropertyActivities))]
        public virtual PimsPropMgmtActivitySubtype PropMgmtActivitySubtypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropMgmtActivityTypeCode))]
        [InverseProperty(nameof(PimsPropMgmtActivityType.PimsPropertyActivities))]
        public virtual PimsPropMgmtActivityType PropMgmtActivityTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(ServiceProviderOrgId))]
        [InverseProperty(nameof(PimsOrganization.PimsPropertyActivities))]
        public virtual PimsOrganization ServiceProviderOrg { get; set; }
        [ForeignKey(nameof(ServiceProviderPersonId))]
        [InverseProperty(nameof(PimsPerson.PimsPropertyActivities))]
        public virtual PimsPerson ServiceProviderPerson { get; set; }
        [InverseProperty(nameof(PimsPropActInvolvedParty.PimsPropertyActivity))]
        public virtual ICollection<PimsPropActInvolvedParty> PimsPropActInvolvedParties { get; set; }
        [InverseProperty(nameof(PimsPropActMinContact.PimsPropertyActivity))]
        public virtual ICollection<PimsPropActMinContact> PimsPropActMinContacts { get; set; }
        [InverseProperty(nameof(PimsPropPropActivity.PimsPropertyActivity))]
        public virtual ICollection<PimsPropPropActivity> PimsPropPropActivities { get; set; }
        [InverseProperty(nameof(PimsPropertyActivityDocument.PimsPropertyActivity))]
        public virtual ICollection<PimsPropertyActivityDocument> PimsPropertyActivityDocuments { get; set; }
        [InverseProperty(nameof(PimsPropertyActivityInvoice.PimsPropertyActivity))]
        public virtual ICollection<PimsPropertyActivityInvoice> PimsPropertyActivityInvoices { get; set; }
    }
}
