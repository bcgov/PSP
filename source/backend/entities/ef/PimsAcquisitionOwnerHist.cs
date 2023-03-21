using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_OWNER_HIST")]
    [Index(nameof(AcquisitionOwnerHistId), nameof(EndDateHist), Name = "PIMS_ACQOWN_H_UK", IsUnique = true)]
    public partial class PimsAcquisitionOwnerHist
    {
        [Key]
        [Column("_ACQUISITION_OWNER_HIST_ID")]
        public long AcquisitionOwnerHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("ACQUISITION_OWNER_ID")]
        public long AcquisitionOwnerId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long? AcquisitionFileId { get; set; }
        [Column("ADDRESS_ID")]
        public long? AddressId { get; set; }
        [Column("IS_ORGANIZATION")]
        public bool IsOrganization { get; set; }
        [Column("LAST_NAME_AND_CORP_NAME")]
        [StringLength(300)]
        public string LastNameAndCorpName { get; set; }
        [Column("OTHER_NAME")]
        [StringLength(300)]
        public string OtherName { get; set; }
        [Column("GIVEN_NAME")]
        [StringLength(300)]
        public string GivenName { get; set; }
        [Column("INCORPORATION_NUMBER")]
        [StringLength(50)]
        public string IncorporationNumber { get; set; }
        [Column("REGISTRATION_NUMBER")]
        [StringLength(50)]
        public string RegistrationNumber { get; set; }
        [Column("CONTACT_EMAIL_ADDR")]
        [StringLength(250)]
        public string ContactEmailAddr { get; set; }
        [Column("CONTACT_PHONE_NUM")]
        [StringLength(20)]
        public string ContactPhoneNum { get; set; }
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
