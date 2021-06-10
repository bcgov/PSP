using System.Linq;

namespace Pims.Ltsa.Extensions
{
    public static class UriExtensions
    {
        public static string AppendToURL(this string baseURL, params string[] segments)
        {
            return string.Join("/", new[] { baseURL.TrimEnd('/') }.Concat(segments.Select(s => s.Trim('/'))));
        }
    }
}
