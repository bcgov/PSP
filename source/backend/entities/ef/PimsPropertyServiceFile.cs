using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY_SERVICE_FILE")]
    [Index(nameof(PropertyServiceFileTypeCode), Name = "PRPSVC_PROPERTY_SERVICE_FILE_TYPE_CODE_IDX")]
    public partial class PimsPropertyServiceFile
    {
        public PimsPropertyServiceFile()
        {
            PimsPropertyPropertyServiceFiles = new HashSet<PimsPropertyPropertyServiceFile>();
        }

        [Key]
        [Column("PROPERTY_SERVICE_FILE_ID")]
        public long PropertyServiceFileId { get; set; }
        [Required]
        [Column("PROPERTY_SERVICE_FILE_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyServiceFileTypeCode { get; set; }
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

        [ForeignKey(nameof(PropertyServiceFileTypeCode))]
        [InverseProperty(nameof(PimsPropertyServiceFileType.PimsPropertyServiceFiles))]
        public virtual PimsPropertyServiceFileType PropertyServiceFileTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsPropertyPropertyServiceFile.PropertyServiceFile))]
        public virtual ICollection<PimsPropertyPropertyServiceFile> PimsPropertyPropertyServiceFiles { get; set; }
    }
}
