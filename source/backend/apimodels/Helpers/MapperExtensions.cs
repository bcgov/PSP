using Pims.Api.Models.Base;

namespace Pims.Api.Helpers.Extensions
{
    /// <summary>
    /// MapperExtensions static class, provides extension methods to help mapping properties.
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Null coalescing method to get the type from an id.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeId(this CodeTypeModel<string> type)
        {
            return type?.Id;
        }
    }
}
