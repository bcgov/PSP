namespace Pims.Dal
{
    /// <summary>
    /// AllHealthCheckOptions class, provides a way to configure PIMS Health Checks.
    /// </summary>
    public class AllHealthCheckOptions
    {
        #region Properties

        public string Port { get; set; }

        public string LivePath { get; set; }

        public string ReadyPath { get; set; }

        public GeocoderHealthCheckOptions Geocoder { get; set; }

        public GeoserverHealthCheckOptions Geoserver { get; set; }

        public LtsaHealthCheckOptions Ltsa { get; set; }

        public PmbcExternalApiHealthCheckOptions PmbcExternalApi { get; set; }

        public PimsBaseHealthCheckOptions SqlServer { get; set; }

        public PimsBaseHealthCheckOptions PimsDBCollation { get; set; }

        public PimsBaseHealthCheckOptions ApiMetrics { get; set; }

        public PimsBaseHealthCheckOptions Cdogs { get; set; }

        public PimsBaseHealthCheckOptions Mayan { get; set; }
        #endregion
    }
}
