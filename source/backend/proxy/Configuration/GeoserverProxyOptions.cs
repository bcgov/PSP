using System.ComponentModel.DataAnnotations;

namespace Pims.Core.Http.Configuration
{
    /// <summary>
    /// GeoserverProxyOptions class, provides a way to configure the proxy controller for geoserver.
    /// </summary>
    public class GeoserverProxyOptions
    {
        #region Properties

        /// <summary>
        /// get/set - the internal url of the geocoder server.
        /// </summary>
        [Required(ErrorMessage = "Configuration 'ProxyUrl' is required.")]
        public string ProxyUrl { get; set; }

        /// <summary>
        /// get/set - the username of the geoserver service account for the inventory layer.
        /// </summary>
        [Required(ErrorMessage = "Configuration 'ServiceUser' is required.")]
        public string ServiceUser { get; set; }

        /// <summary>
        /// get/set - the password of the geoserver service account for the inventory layer.
        /// </summary>
        [Required(ErrorMessage = "Configuration 'ServicePassword' is required.")]
        public string ServicePassword { get; set; }
        #endregion
    }
}
