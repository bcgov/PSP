using System;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// TakeModel class, provides a model to represent the take.
    /// </summary>
    public class TakeModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        public string Description { get; set; }

        public bool IsSurplusSeverance { get; set; }

        public bool IsLicenseToConstruct { get; set; }

        public bool IsNewRightOfWay { get; set; }

        public bool IsSection16 { get; set; }

        public bool IsStatutoryRightOfWay { get; set; }

        public float? LicenseToConstructArea { get; set; }

        public DateTime? LtcEndDt { get; set; }

        public float? NewRightOfWayArea { get; set; }

        public float? Section16Area { get; set; }

        public DateTime? Section16EndDt { get; set; }

        public DateTime? SrwEndDt { get; set; }

        public AcquisitionFileModel PropertyAcquisitionFile { get; set; }

        public long PropertyAcquisitionFileId { get; set; }

        public float? StatutoryRightOfWayArea { get; set; }

        public float? SurplusSeveranceArea { get; set; }

        public string TakeSiteContamTypeCode { get; set; }

        public string TakeTypeCode { get; set; }

        public string TakeStatusTypeCode { get; set; }
        #endregion
    }
}
