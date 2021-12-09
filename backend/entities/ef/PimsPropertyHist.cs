using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsPropertyHist
    {
        public long PropertyHistId { get; set; }
        public DateTime EffectiveDateHist { get; set; }
        public DateTime? EndDateHist { get; set; }
        public long PropertyId { get; set; }
        public long? PropertyManagerId { get; set; }
        public long? PropMgmtOrgId { get; set; }
        public string PropertyTypeCode { get; set; }
        public string PropertyClassificationTypeCode { get; set; }
        public long AddressId { get; set; }
        public short RegionCode { get; set; }
        public short DistrictCode { get; set; }
        public string PropertyTenureTypeCode { get; set; }
        public string PropertyAreaUnitTypeCode { get; set; }
        public string PropertyStatusTypeCode { get; set; }
        public string SurplusDeclarationTypeCode { get; set; }
        public string PropertyDataSourceTypeCode { get; set; }
        public DateTime PropertyDataSourceEffectiveDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Pid { get; set; }
        public int? Pin { get; set; }
        public float LandArea { get; set; }
        public string EncumbranceReason { get; set; }
        public string SurplusDeclarationComment { get; set; }
        public DateTime? SurplusDeclarationDate { get; set; }
        public bool IsOwned { get; set; }
        public bool IsPropertyOfInterest { get; set; }
        public bool IsVisibleToOtherAgencies { get; set; }
        public bool IsSensitive { get; set; }
        public string Zoning { get; set; }
        public string ZoningPotential { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserid { get; set; }
        public Guid? AppCreateUserGuid { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public Guid? AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }
    }
}
