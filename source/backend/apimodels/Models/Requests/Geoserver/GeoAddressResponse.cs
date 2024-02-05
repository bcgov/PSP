<<<<<<<< Updated upstream:source/backend/apimodels/Models/Requests/Geocoder/GeoAddressResponse.cs
namespace Pims.Api.Models.Requests.Geocoder
========
namespace Pims.Api.Models.Requests.Geoserver
>>>>>>>> Stashed changes:source/backend/apimodels/Models/Requests/Geoserver/GeoAddressResponse.cs
{
    public class GeoAddressResponse
    {
        #region Properties
        public string SiteId { get; set; }

        public string FullAddress { get; set; }

        public string Address1 { get; set; }

        public string AdministrativeArea { get; set; }

        public string ProvinceCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public long Score { get; set; }
        #endregion
    }
}
