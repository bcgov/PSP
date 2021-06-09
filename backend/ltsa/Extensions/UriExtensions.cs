using Pims.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
