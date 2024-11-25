using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    public static class AcquisitionFileExtensions
    {
        /// <summary>
        /// Returns the suffix portion of the supplied Acquisition File Number.
        /// </summary>
        /// <param name="pimsAcquisitionFile">The Acquisition File entity.</param>
        /// <returns>The file number suffix (e.g. "1", "2", "3", etc) if it is found, or -1 if it is not.</returns>
        public static int GetAcquisitionNumberSuffix(this PimsAcquisitionFile pimsAcquisitionFile)
        {
            if (pimsAcquisitionFile is null)
            {
                return -1;
            }

            // TODO: Fix acquisition file number
            return 0;

            /*
            int lastIndex = pimsAcquisitionFile.FileNumber.LastIndexOf('-');
            if (lastIndex >= 0 && lastIndex < pimsAcquisitionFile.FileNumber.Length - 1)
            {
                string suffix = pimsAcquisitionFile.FileNumber.Substring(lastIndex + 1);
                return int.TryParse(suffix, out int number) ? number : -1;
            }
            else
            {
                return -1;
            }
            */
        }
    }
}
