namespace Pims.Dal.Options
{
    public class GeoserverHealthCheckOptions : PimsBaseHealthCheckOptions
    {
        public string Url { get; set; }

        public string StatusCode { get; set; }
    }
}
