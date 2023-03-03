using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_OWNER")]
    [Index(nameof(AcquisitionFileId), Name = "ACQOWN_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(AddressId), Name = "ACQOWN_ADDRESS_ID_IDX")]
    [Index(nameof(LastNameOrCorpName1), Name = "ACQOWN_LAST_NAME_OR_CORP_NAME_1_IDX")]
    public partial class PimsAcquisitionOwner
    {
        [Key]
        [Column("ACQUISITION_OWNER_ID")]
        public long AcquisitionOwnerId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long? AcquisitionFileId { get; set; }
        [Column("ADDRESS_ID")]
        public long? AddressId { get; set; }
        [Required]
        [Column("LAST_NAME_OR_CORP_NAME_1")]
        [StringLength(300)]
        public string LastNameOrCorpName1 { get; set; }
        [Column("LAST_NAME_OR_CORP_NAME_2")]
        [StringLength(300)]
        public string LastNameOrCorpName2 { get; set; }
        [Column("GIVEN_NAME")]
        [StringLength(300)]
        public string GivenName { get; set; }
        [Column("INCORPORATION_NUMBER")]
        [StringLength(50)]
        public string IncorporationNumber { get; set; }
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

        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsAcquisitionOwners))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [ForeignKey(nameof(AddressId))]
        [InverseProperty(nameof(PimsAddress.PimsAcquisitionOwners))]
        public virtual PimsAddress Address { get; set; }
    }
}
