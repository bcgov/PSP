namespace Pims.Api.Areas.Admin.Models.Parcel
{
    public class BuildingFiscalModel : Api.Models.BaseModel
    {
        #region Properties
        public long BuildingId { get; set; }

        public int FiscalYear { get; set; }

        public string Key { get; set; }

        public decimal Value { get; set; }

        public string Note { get; set; }
        #endregion
    }
}
