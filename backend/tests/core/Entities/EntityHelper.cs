using System.Diagnostics.CodeAnalysis;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static partial class EntityHelper
    {
        /// <summary>
        /// Generate a random rowversion.
        /// </summary>
        /// <param name="data">Data to convert into row version.</param>
        /// <returns></returns>
        public static byte[] GenerateConcurrencyControlNumber(string data)
        {
            return System.Text.Encoding.UTF8.GetBytes(data);
        }
    }
}
