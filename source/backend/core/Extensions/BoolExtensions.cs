namespace Pims.Core.Extensions
{
    public static class BoolExtensions
    {
        public static string BoolToYesNo(this bool val)
        {
            return val ? "Yes" : "No";
        }

        public static string BoolToYesNoUnknown(this bool? val)
        {
            if(val == null)
            {
                return "Unknown";
            }
            return val.Value ? "Yes" : "No";
        }
    }
}
