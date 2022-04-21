namespace Pims.Core.Extensions
{
    public static class BoolExtensions
    {
        public static string BoolToYesNo(this bool val)
        {
            return val ? "Yes" : "No";
        }
    }
}
