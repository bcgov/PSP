using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ORGANIZATION_HIST")]
    [Index(nameof(OrganizationHistId), nameof(EndDateHist), Name = "PIMS_ORG_H_UK", IsUnique = true)]
    public partial class PimsOrganizationHist
    {
        [Key]
        [Column("_ORGANIZATION_HIST_ID")]
        public long OrganizationHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
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
    }
}
