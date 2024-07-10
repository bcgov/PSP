namespace Pims.Dal
{
    public class PimsBaseHealthCheckOptions
    {
        public int Period { get; set; } = 1; // in minutes

        public bool Enabled { get; set; } = true;
    }
}
