using System;
using System.Collections.Generic;

namespace Pims.Api.Areas.Tools.Models.Import
{
    public class ParcelBuildingModel : Pims.Api.Models.BaseAppModel
    {
        #region Properties
        public long Id { get; set; }

        public long ParcelId { get; set; }

        public long AgencyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public AddressModel Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public long BuildingConstructionTypeId { get; set; }

        public string BuildingConstructionType { get; set; }

        public int BuildingFloorCount { get; set; }

        public long BuildingPredominateUseId { get; set; }

        public string BuildingPredominateUse { get; set; }

        public long BuildingOccupantTypeId { get; set; }

        public string BuildingOccupantType { get; set; }

        public string BuildingTenancy { get; set; }

        public float RentableArea { get; set; }

        public string OccupantName { get; set; }

        public DateTime? LeaseExpiry { get; set; }

        public bool IsSensitive { get; set; }

        public IEnumerable<BuildingEvaluationModel> Evaluations { get; set; } = new List<BuildingEvaluationModel>();

        public IEnumerable<BuildingFiscalModel> Fiscals { get; set; } = new List<BuildingFiscalModel>();
        #endregion
    }
}
