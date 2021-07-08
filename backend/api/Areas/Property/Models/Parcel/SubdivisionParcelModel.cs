using Model = Pims.Api.Models;

namespace Pims.Api.Areas.Property.Models.Parcel
{
    public class SubdivisionParcelModel : Model.BaseAppModel
    {
        public long Id { get; set; }

        public string PID { get; set; }

        public int? PIN { get; set; }
    }
}
