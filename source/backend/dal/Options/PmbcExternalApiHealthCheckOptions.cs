namespace Pims.Dal.Options
{
    public class PmbcExternalApiHealthCheckOptions : PimsBaseHealthCheckOptions
    {
        public string Url { get; set; }

        public string StatusCode { get; set; }
    }
}
