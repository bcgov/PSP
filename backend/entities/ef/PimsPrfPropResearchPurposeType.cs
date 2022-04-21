using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PRF_PROP_RESEARCH_PURPOSE_TYPE")]
    [Index(nameof(PropertyResearchFileId), Name = "PRSPRP_PROPERTY_RESEARCH_FILE_ID_IDX")]
    [Index(nameof(PropResearchPurposeTypeCode), nameof(PropertyResearchFileId), Name = "PRSPRP_PROP_PURPOSE_TUC", IsUnique = true)]
    [Index(nameof(PropResearchPurposeTypeCode), Name = "PRSPRP_PROP_RESEARCH_PURPOSE_TYPE_CODE_IDX")]
    public partial class PimsPrfPropResearchPurposeType
    {
        [Key]
        [Column("PRF_PROP_RESEARCH_PURPOSE_ID")]
        public long PrfPropResearchPurposeId { get; set; }
        [Column("PROPERTY_RESEARCH_FILE_ID")]
        public long? PropertyResearchFileId { get; set; }
        [Required]
        [Column("PROP_RESEARCH_PURPOSE_TYPE_CODE")]
        [StringLength(20)]
        public string PropResearchPurposeTypeCode { get; set; }
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

        [ForeignKey(nameof(PropResearchPurposeTypeCode))]
        [InverseProperty(nameof(PimsPropResearchPurposeType.PimsPrfPropResearchPurposeTypes))]
        public virtual PimsPropResearchPurposeType PropResearchPurposeTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropertyResearchFileId))]
        [InverseProperty(nameof(PimsPropertyResearchFile.PimsPrfPropResearchPurposeTypes))]
        public virtual PimsPropertyResearchFile PropertyResearchFile { get; set; }
    }
}
