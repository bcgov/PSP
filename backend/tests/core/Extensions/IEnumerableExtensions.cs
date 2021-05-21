using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Core.Test
{
    /// <summary>
    /// IEnumerableExtensions static class, provides extension methods for IEnumerable.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Lazy loads the enumerable to enable testing "IEnumerator IEnumerable.GetEnumerator()"
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable AsWeakEnumerable(this IEnumerable source)
        {
            foreach (object o in source)
            {
                yield return o;
            }
        }
    }
}
