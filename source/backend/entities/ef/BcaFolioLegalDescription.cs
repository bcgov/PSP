using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_FOLIO_LEGAL_DESCRIPTION")]
    [Index(nameof(RollNumber), Name = "BCAFLD_ROLL_NUMBER_IDX")]
    public partial class BcaFolioLegalDescription
    {
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("FORMATTED_LEGAL_DESCRIPTION")]
        [StringLength(1024)]
        public string FormattedLegalDescription { get; set; }
        [Column("PID")]
        [StringLength(255)]
        public string Pid { get; set; }
        [Column("LOT")]
        [StringLength(255)]
        public string Lot { get; set; }
        [Column("STRATA_LOT")]
        [StringLength(255)]
        public string StrataLot { get; set; }
        [Column("PARCEL")]
        [StringLength(255)]
        public string Parcel { get; set; }
        [Column("BLOCK")]
        [StringLength(255)]
        public string Block { get; set; }
        [Column("SUB_BLOCK")]
        [StringLength(255)]
        public string SubBlock { get; set; }
        [Column("PLAN_NUMBER")]
        [StringLength(255)]
        public string PlanNumber { get; set; }
        [Column("SUB_LOT")]
        [StringLength(255)]
        public string SubLot { get; set; }
        [Column("PART_1")]
        [StringLength(255)]
        public string Part1 { get; set; }
        [Column("PART_2")]
        [StringLength(255)]
        public string Part2 { get; set; }
        [Column("PART_3")]
        [StringLength(255)]
        public string Part3 { get; set; }
        [Column("PART_4")]
        [StringLength(255)]
        public string Part4 { get; set; }
        [Column("DISTRICT_LOT")]
        [StringLength(255)]
        public string DistrictLot { get; set; }
        [Column("LEGAL_SUBDIVISION")]
        [StringLength(255)]
        public string LegalSubdivision { get; set; }
        [Column("SECTION")]
        [StringLength(255)]
        public string Section { get; set; }
        [Column("TOWNSHIP")]
        [StringLength(255)]
        public string Township { get; set; }
        [Column("RANGE")]
        [StringLength(255)]
        public string Range { get; set; }
        [Column("MERIDIAN")]
        [StringLength(255)]
        public string Meridian { get; set; }
        [Column("MERIDIAN_SHORT")]
        [StringLength(255)]
        public string MeridianShort { get; set; }
        [Column("BCA_GROUP")]
        [StringLength(255)]
        public string BcaGroup { get; set; }
        [Column("LAND_DISTRICT")]
        [StringLength(255)]
        public string LandDistrict { get; set; }
        [Column("LAND_DISTRICT_DESCRIPTION")]
        [StringLength(255)]
        public string LandDistrictDescription { get; set; }
        [Column("PORTION")]
        [StringLength(255)]
        public string Portion { get; set; }
        [Column("EXCEPT_PLAN")]
        [StringLength(255)]
        public string ExceptPlan { get; set; }
        [Column("FIRST_NATION_RESERVE_NUMBER")]
        [StringLength(255)]
        public string FirstNationReserveNumber { get; set; }
        [Column("FIRST_NATION_RESERVE_DESCRIPTION")]
        [StringLength(255)]
        public string FirstNationReserveDescription { get; set; }
        [Column("LEASE_LICENSE_NUMBER")]
        [StringLength(255)]
        public string LeaseLicenseNumber { get; set; }
        [Column("LAND_BRANCH_FILE_NUMBER")]
        [StringLength(255)]
        public string LandBranchFileNumber { get; set; }
        [Column("AIR_SPACE_PARCEL_NUMBER")]
        [StringLength(255)]
        public string AirSpaceParcelNumber { get; set; }
        [Column("NTS_LOCATION")]
        [StringLength(255)]
        public string NtsLocation { get; set; }
        [Column("LEGAL_TEXT")]
        [StringLength(1024)]
        public string LegalText { get; set; }
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
