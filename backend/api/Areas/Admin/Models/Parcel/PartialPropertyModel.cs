using Model = Pims.Api.Models;

namespace Pims.Api.Areas.Admin.Models.Parcel
{
    public class PartialPropertyModel : Model.BaseModel
    {
        #region Properties
        public long Id { get; set; }

        public long StatusId { get; set; }

        public long ClassificationId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        #endregion
    }
}
