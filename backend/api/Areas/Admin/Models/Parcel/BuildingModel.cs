using System;
using System.Collections.Generic;
using Model = Pims.Api.Models;

namespace Pims.Api.Areas.Admin.Models.Parcel
{
    public class BuildingModel : Model.BaseModel
    {
        #region Properties
        public long Id { get; set; }

        public long ParcelId { get; set; }

        public long AgencyId { get; set; }

        public string SubAgency { get; set; }

        public string Agency { get; set; }

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

        public DateTime? LeaseExpiry { get; set; }

        public string OccupantName { get; set; }

        public bool TransferLeaseOnSale { get; set; }

        public string BuildingTenancy { get; set; }

        public float RentableArea { get; set; }

        public float TotalArea { get; set; }

        public bool IsSensitive { get; set; }

        public IEnumerable<BuildingEvaluationModel> Evaluations { get; set; } = new List<BuildingEvaluationModel>();

        public IEnumerable<BuildingFiscalModel> Fiscals { get; set; } = new List<BuildingFiscalModel>();
        #endregion
    }
}
