using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    public static class TypeCodeExtensions
    {
        /// <summary>
        /// Get a string representing the type, appending a passed description if this type refers to "OTHER".
        /// </summary>
        /// <param name="type">The type code.</param>
        /// <param name="otherDescription">The optional description to include if this type code is set to "OTHER".</param>
        /// <returns></returns>
        public static string GetTypeDescriptionOther(this ITypeEntity<string> type, string otherDescription)
        {
            string description = type?.Description;
            if (type?.Id == "OTHER")
            {
                return $"{description} - {otherDescription}".Trim();
            }
            return description;
        }
    }
}
