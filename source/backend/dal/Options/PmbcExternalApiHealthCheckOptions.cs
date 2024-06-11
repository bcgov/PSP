namespace Pims.Dal
{
    public class PmbcExternalApiHealthCheckOptions : PimsBaseHealthCheckOptions
    {
        public string Url { get; set; }

        public string StatusCode { get; set; }
    }
}
