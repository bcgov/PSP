using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_FOLIO_ADDRESS")]
    [Index(nameof(RollNumber), Name = "BCFADR_ROLL_NUMBER_IDX")]
    public partial class BcaFolioAddress
    {
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("ADDRESS_ID")]
        [StringLength(255)]
        public string AddressId { get; set; }
        [Column("PRIMARY_FLAG")]
        public bool? PrimaryFlag { get; set; }
        [Column("UNIT_NUMBER")]
        [StringLength(255)]
        public string UnitNumber { get; set; }
        [Column("STREET_NUMBER")]
        [StringLength(255)]
        public string StreetNumber { get; set; }
        [Column("STREET_DIRECTION_PREFIX")]
        [StringLength(255)]
        public string StreetDirectionPrefix { get; set; }
        [Column("STREET_NAME")]
        [StringLength(255)]
        public string StreetName { get; set; }
        [Column("STREET_TYPE")]
        [StringLength(255)]
        public string StreetType { get; set; }
        [Column("STREET_DIRECTION_SUFFIX")]
        [StringLength(255)]
        public string StreetDirectionSuffix { get; set; }
        [Column("CITY")]
        [StringLength(255)]
        public string City { get; set; }
        [Column("PROVINCE_STATE")]
        [StringLength(255)]
        public string ProvinceState { get; set; }
        [Column("POSTAL_ZIP")]
        [StringLength(255)]
        public string PostalZip { get; set; }
        [Column("MAP_REFERENCE_NUMBER")]
        [StringLength(255)]
        public string MapReferenceNumber { get; set; }
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
        public virtual BcaFolioRecord RollNumberNavigation { get; set; }
    }
}
