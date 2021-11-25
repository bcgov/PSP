using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsProperty
    {
        public PimsProperty()
        {
            PimsProjectProperties = new HashSet<PimsProjectProperty>();
            PimsPropertyActivities = new HashSet<PimsPropertyActivity>();
            PimsPropertyEvaluations = new HashSet<PimsPropertyEvaluation>();
            PimsPropertyLeases = new HashSet<PimsPropertyLease>();
            PimsPropertyOrganizations = new HashSet<PimsPropertyOrganization>();
            PimsPropertyPropertyServiceFiles = new HashSet<PimsPropertyPropertyServiceFile>();
            PimsPropertyTaxes = new HashSet<PimsPropertyTax>();
        }

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
        public string LandLegalDescription { get; set; }
        public Geometry Boundary { get; set; }
        public Geometry Location { get; set; }
        public string EncumbranceReason { get; set; }
        public string SurplusDeclarationComment { get; set; }
        public DateTime? SurplusDeclarationDate { get; set; }
        public bool? IsOwned { get; set; }
        public bool? IsPropertyOfInterest { get; set; }
        public bool? IsVisibleToOtherAgencies { get; set; }
        public bool? IsSensitive { get; set; }
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

        public virtual PimsAddress Address { get; set; }
        public virtual PimsDistrict DistrictCodeNavigation { get; set; }
        public virtual PimsOrganization PropMgmtOrg { get; set; }
        public virtual PimsAreaUnitType PropertyAreaUnitTypeCodeNavigation { get; set; }
        public virtual PimsPropertyClassificationType PropertyClassificationTypeCodeNavigation { get; set; }
        public virtual PimsPropertyDataSourceType PropertyDataSourceTypeCodeNavigation { get; set; }
        public virtual PimsPerson PropertyManager { get; set; }
        public virtual PimsPropertyStatusType PropertyStatusTypeCodeNavigation { get; set; }
        public virtual PimsPropertyTenureType PropertyTenureTypeCodeNavigation { get; set; }
        public virtual PimsPropertyType PropertyTypeCodeNavigation { get; set; }
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        public virtual PimsSurplusDeclarationType SurplusDeclarationTypeCodeNavigation { get; set; }
        public virtual ICollection<PimsProjectProperty> PimsProjectProperties { get; set; }
        public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; }
        public virtual ICollection<PimsPropertyEvaluation> PimsPropertyEvaluations { get; set; }
        public virtual ICollection<PimsPropertyLease> PimsPropertyLeases { get; set; }
        public virtual ICollection<PimsPropertyOrganization> PimsPropertyOrganizations { get; set; }
        public virtual ICollection<PimsPropertyPropertyServiceFile> PimsPropertyPropertyServiceFiles { get; set; }
        public virtual ICollection<PimsPropertyTax> PimsPropertyTaxes { get; set; }
    }
}
