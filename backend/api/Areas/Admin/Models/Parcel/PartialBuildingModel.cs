using Model = Pims.Api.Models;

namespace Pims.Api.Areas.Admin.Models.Parcel
{
    public class PartialBuildingModel : Model.BaseAppModel
    {
        #region Properties
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        #endregion
    }
}
