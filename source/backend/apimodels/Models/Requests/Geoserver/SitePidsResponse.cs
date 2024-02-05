using System;
using System.Collections.Generic;

<<<<<<<< Updated upstream:source/backend/apimodels/Models/Requests/Geocoder/SitePidsResponse.cs
namespace Pims.Api.Models.Requests.Geocoder
========
namespace Pims.Api.Models.Requests.Geoserver
>>>>>>>> Stashed changes:source/backend/apimodels/Models/Requests/Geoserver/SitePidsResponse.cs
{
    public class SitePidsResponse
    {
        #region Properties
        public Guid SiteId { get; set; }

        public IEnumerable<string> Pids { get; set; }
        #endregion
    }
}
