using System.Text;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    public static class AcquisitionOwnerExtensions
    {
        public static string FormatOwnerName(this PimsAcquisitionOwner pimsOwner)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(pimsOwner.GivenName);

            if (!string.IsNullOrEmpty(pimsOwner.OtherName))
            {
                stringBuilder.Append(" " + pimsOwner.OtherName);
            }

            if (!string.IsNullOrEmpty(pimsOwner.LastNameAndCorpName))
            {
                stringBuilder.Append(" " + pimsOwner.LastNameAndCorpName);
            }

            return stringBuilder.ToString();
        }
    }
}
