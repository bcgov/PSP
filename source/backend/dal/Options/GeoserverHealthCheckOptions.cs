namespace Pims.Dal
{
    public class GeoserverHealthCheckOptions : PimsBaseHealthCheckOptions
    {
        public string Url { get; set; }

        public string StatusCode { get; set; }
    }
}
