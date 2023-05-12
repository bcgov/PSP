using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_PAYEE")]
    [Index(nameof(AcquisitionOwnerId), Name = "ACQPAY_ACQUISITION_OWNER_ID_IDX")]
    [Index(nameof(CompensationRequisitionId), Name = "ACQPAY_COMPENSATION_REQUISITION_ID_IDX")]
    public partial class PimsAcquisitionPayee
    {
        public PimsAcquisitionPayee()
        {
            PimsAcqPayeeCheques = new HashSet<PimsAcqPayeeCheque>();
        }

        [Key]
        [Column("ACQUISITION_PAYEE_ID")]
        public long AcquisitionPayeeId { get; set; }
        [Column("ACQUISITION_OWNER_ID")]
        public long AcquisitionOwnerId { get; set; }
        [Column("COMPENSATION_REQUISITION_ID")]
        public long CompensationRequisitionId { get; set; }
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

        [ForeignKey(nameof(AcquisitionOwnerId))]
        [InverseProperty(nameof(PimsAcquisitionOwner.PimsAcquisitionPayees))]
        public virtual PimsAcquisitionOwner AcquisitionOwner { get; set; }
        [ForeignKey(nameof(CompensationRequisitionId))]
        [InverseProperty(nameof(PimsCompensationRequisition.PimsAcquisitionPayees))]
        public virtual PimsCompensationRequisition CompensationRequisition { get; set; }
        [InverseProperty(nameof(PimsAcqPayeeCheque.AcquisitionPayee))]
        public virtual ICollection<PimsAcqPayeeCheque> PimsAcqPayeeCheques { get; set; }
    }
}
