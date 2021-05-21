
using System;
using System.Diagnostics.CodeAnalysis;
using Entity = Pims.Dal.Entities;

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
        public static byte[] GenerateRowVersion(string data)
        {
            return System.Text.Encoding.UTF8.GetBytes(data);
        }
    }
}
