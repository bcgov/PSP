using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY_ACQUISITION_FILE")]
    [Index(nameof(AcquisitionFileId), Name = "PRACQF_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(PropertyId), Name = "PRACQF_PROPERTY_ID_IDX")]
    [Index(nameof(PropertyId), nameof(AcquisitionFileId), Name = "PRACQF_PROP_ACQ_TUC", IsUnique = true)]
    public partial class PimsPropertyAcquisitionFile
    {
        public PimsPropertyAcquisitionFile()
        {
            PimsInthldrPropInterests = new HashSet<PimsInthldrPropInterest>();
            PimsTakes = new HashSet<PimsTake>();
        }

        [Key]
        [Column("PROPERTY_ACQUISITION_FILE_ID")]
        public long PropertyAcquisitionFileId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }
        [Column("PROPERTY_NAME")]
        [StringLength(500)]
        public string PropertyName { get; set; }
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }
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

        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsPropertyAcquisitionFiles))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [ForeignKey(nameof(PropertyId))]
        [InverseProperty(nameof(PimsProperty.PimsPropertyAcquisitionFiles))]
        public virtual PimsProperty Property { get; set; }
        [InverseProperty(nameof(PimsInthldrPropInterest.PropertyAcquisitionFile))]
        public virtual ICollection<PimsInthldrPropInterest> PimsInthldrPropInterests { get; set; }
        [InverseProperty(nameof(PimsTake.PropertyAcquisitionFile))]
        public virtual ICollection<PimsTake> PimsTakes { get; set; }
    }
}
