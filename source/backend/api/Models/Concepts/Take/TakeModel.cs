using System;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// TakeModel class, provides a model to represent the take.
    /// LINK @frontend/src/models/api/Take.ts
    /// </summary>
    public class TakeModel : BaseAppModel
    {
        #region Properties
        public long Id { get; set; }

        public string Description { get; set; }

        public float? NewHighwayDedicationArea { get; set; }

        public TypeModel<string> AreaUnitTypeCode { get; set; }

        public bool? IsAcquiredForInventory { get; set; }

        public bool? IsThereSurplus { get; set; }

        public bool? IsNewLicenseToConstruct { get; set; }

        public bool? IsNewHighwayDedication { get; set; }

        public bool? IsNewLandAct { get; set; }

        public bool? IsNewInterestInSrw { get; set; }

        public float? LicenseToConstructArea { get; set; }

        public DateTime? LtcEndDt { get; set; }

        public float? LandActArea { get; set; }

        public DateTime? LandActEndDt { get; set; }

        public AcquisitionFileModel PropertyAcquisitionFile { get; set; }

        public long PropertyAcquisitionFileId { get; set; }

        public float? StatutoryRightOfWayArea { get; set; }

        public DateTime? SrwEndDt { get; set; }

        public float? SurplusArea { get; set; }

        public TypeModel<string> TakeSiteContamTypeCode { get; set; }

        public TypeModel<string> TakeTypeCode { get; set; }

        public TypeModel<string> TakeStatusTypeCode { get; set; }

        public TypeModel<string> LandActTypeCode { get; set; }
        #endregion
    }
}
