using System.Text;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    public static class PersonExtensions
    {
        public static string FormatName(this PimsPerson pimsPerson)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(pimsPerson.FirstName);

            if (!string.IsNullOrEmpty(pimsPerson.MiddleNames))
            {
                stringBuilder.Append(pimsPerson.MiddleNames);
            }

            stringBuilder.Append(pimsPerson.Surname);

            if (!string.IsNullOrEmpty(pimsPerson.NameSuffix))
            {
                stringBuilder.Append(pimsPerson.NameSuffix);
            }

            return stringBuilder.ToString();
        }
    }
}
