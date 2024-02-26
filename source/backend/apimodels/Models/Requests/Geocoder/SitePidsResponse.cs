using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Requests.Geocoder
{
    public class SitePidsResponse
    {
        #region Properties
        public Guid SiteId { get; set; }

        public IEnumerable<string> Pids { get; set; }
        #endregion
    }
}
