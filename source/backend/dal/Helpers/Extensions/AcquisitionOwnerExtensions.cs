using System.Linq;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    public static class AcquisitionOwnerExtensions
    {
        public static string FormatOwnerName(this PimsAcquisitionOwner pimsOwner)
        {
            if (pimsOwner is null)
            {
                return string.Empty;
            }

            string[] names = { pimsOwner.GivenName, pimsOwner.OtherName, pimsOwner.LastNameAndCorpName };

            return string.Join(" ", names.Where(n => n != null && n.Trim() != string.Empty));
        }
    }
}
