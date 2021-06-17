namespace Pims.Api.Areas.Tools.Models.Import
{
    public class PropertyModel : Pims.Api.Models.BaseModel
    {
        #region Properties
        public long Id { get; set; }

        public string ProjectNumber { get; set; }

        public long StatusId { get; set; }

        public string Status { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long ClassificationId { get; set; }

        public string Classification { get; set; }

        public long AgencyId { get; set; }

        public string SubAgency { get; set; }

        public string Agency { get; set; }

        public AddressModel Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsSensitive { get; set; }
        #endregion
    }
}
