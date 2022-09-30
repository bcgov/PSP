using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("BCA_OWNERSHIP_GROUP")]
    [Index(nameof(RollNumber), Name = "BCAOWG_ROLL_NUMBER_IDX")]
    public partial class BcaOwnershipGroup
    {
        public BcaOwnershipGroup()
        {
            BcaOwners = new HashSet<BcaOwner>();
        }

        [Key]
        [Column("OWNERSHIP_GROUP_ID")]
        [StringLength(32)]
        public string OwnershipGroupId { get; set; }
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("ASSESSMENT_NOTICE_RETURNED")]
        public bool? AssessmentNoticeReturned { get; set; }
        [Column("ASSESSMENT_NOTICE_SUPPRESSED")]
        public bool? AssessmentNoticeSuppressed { get; set; }
        [Column("CHANGE_TYPE")]
        [StringLength(16)]
        public string ChangeType { get; set; }
        [Column("CHANGE_TYPE_DESCRIPTION")]
        [StringLength(255)]
        public string ChangeTypeDescription { get; set; }
        [Column("CHANGE_DATE", TypeName = "date")]
        public DateTime? ChangeDate { get; set; }
        [Column("FORMATTED_MAILING_ADDR_LINE_1")]
        [StringLength(40)]
        public string FormattedMailingAddrLine1 { get; set; }
        [Column("FORMATTED_MAILING_ADDR_LINE_2")]
        [StringLength(40)]
        public string FormattedMailingAddrLine2 { get; set; }
        [Column("FORMATTED_MAILING_ADDR_LINE_3")]
        [StringLength(40)]
        public string FormattedMailingAddrLine3 { get; set; }
        [Column("FORMATTED_MAILING_ADDR_LINE_4")]
        [StringLength(40)]
        public string FormattedMailingAddrLine4 { get; set; }
        [Column("FORMATTED_MAILING_ADDR_LINE_5")]
        [StringLength(40)]
        public string FormattedMailingAddrLine5 { get; set; }
        [Column("FORMATTED_MAILING_ADDR_LINE_6")]
        [StringLength(40)]
        public string FormattedMailingAddrLine6 { get; set; }
        [Column("MAILING_ADDR_ATTENTION")]
        [StringLength(255)]
        public string MailingAddrAttention { get; set; }
        [Column("MAILING_ADDR_CARE_OF")]
        [StringLength(255)]
        public string MailingAddrCareOf { get; set; }
        [Column("MAILING_ADDR_FLOOR")]
        [StringLength(255)]
        public string MailingAddrFloor { get; set; }
        [Column("MAILING_ADDR_UNIT_NUMBER")]
        [StringLength(255)]
        public string MailingAddrUnitNumber { get; set; }
        [Column("MAILING_ADDR_STREET_DIRECTION_PREFIX")]
        [StringLength(255)]
        public string MailingAddrStreetDirectionPrefix { get; set; }
        [Column("MAILING_ADDR_STREET_NUMBER")]
        [StringLength(255)]
        public string MailingAddrStreetNumber { get; set; }
        [Column("MAILING_ADDR_STREET_NAME")]
        [StringLength(255)]
        public string MailingAddrStreetName { get; set; }
        [Column("MAILING_ADDR_STREET_TYPE")]
        [StringLength(255)]
        public string MailingAddrStreetType { get; set; }
        [Column("MAILING_ADDR_STREET_DIRECTION_SUFFIX")]
        [StringLength(255)]
        public string MailingAddrStreetDirectionSuffix { get; set; }
        [Column("MAILING_ADDR_CITY")]
        [StringLength(255)]
        public string MailingAddrCity { get; set; }
        [Column("MAILING_ADDR_PROVINCE_STATE")]
        [StringLength(255)]
        public string MailingAddrProvinceState { get; set; }
        [Column("MAILING_ADDR_COUNTRY")]
        [StringLength(255)]
        public string MailingAddrCountry { get; set; }
        [Column("MAILING_ADDR_POSTAL_ZIP")]
        [StringLength(255)]
        public string MailingAddrPostalZip { get; set; }
        [Column("MAILING_ADDR_FREE_FORM")]
        [StringLength(255)]
        public string MailingAddrFreeForm { get; set; }
        [Column("MAILING_ADDR_COMPARTMENT")]
        [StringLength(255)]
        public string MailingAddrCompartment { get; set; }
        [Column("MAILING_ADDR_DELIVERY_INSTALLATION_TYPE")]
        [StringLength(255)]
        public string MailingAddrDeliveryInstallationType { get; set; }
        [Column("MAILING_ADDR_MODE_OF_DELIVERY")]
        [StringLength(255)]
        public string MailingAddrModeOfDelivery { get; set; }
        [Column("MAILING_ADDR_MODE_OF_DELIVERY_VALUE")]
        [StringLength(255)]
        public string MailingAddrModeOfDeliveryValue { get; set; }
        [Column("MAILING_ADDR_SITE")]
        [StringLength(255)]
        public string MailingAddrSite { get; set; }
        [Column("MAILING_ADDR_BULK_MAIL_CODE")]
        [StringLength(255)]
        public string MailingAddrBulkMailCode { get; set; }
        [Column("CHANGE_SOURCE")]
        [StringLength(16)]
        public string ChangeSource { get; set; }
        [Column("CHANGE_SOURCE_DESCRIPTION")]
        [StringLength(255)]
        public string ChangeSourceDescription { get; set; }
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

        [ForeignKey(nameof(RollNumber))]
        [InverseProperty(nameof(BcaFolioRecord.BcaOwnershipGroups))]
        public virtual BcaFolioRecord RollNumberNavigation { get; set; }
        [InverseProperty(nameof(BcaOwner.OwnershipGroup))]
        public virtual ICollection<BcaOwner> BcaOwners { get; set; }
    }
}
