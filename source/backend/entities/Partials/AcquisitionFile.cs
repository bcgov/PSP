using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionFile class, provides an entity for the datamodel to manage acquisition files.
    /// </summary>
    public partial class PimsAcquisitionFile : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionFileId; set => this.AcquisitionFileId = value; }

        // FIXME: add the suffix when available
        [NotMapped]
        public string FileNumberFormatted { get => GenerateAcquisitionFileNumber(this.RegionCode, this.FileNo /*, this.FileNoSuffix */); }
        #endregion

        /// <summary>
        /// Generates a new Acquisition File Number in the following format.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// Format: YY-XXXXXXXXXXXX-ZZ
        /// </item>
        /// <item>
        /// Prefix - (YY above) The prefix numbers for an Acquisition file correspond with the MoTI region
        /// </item>
        /// <item>
        /// File # - (XXXXX... above) Acquisition File number is created and each file number should increase in increments of 1.
        /// The digit base number is unique to the file. Do not pad the number with zeros.
        /// </item>
        /// <item>
        /// Suffix - (ZZ above) The suffix numbers for an Acquisition file defaults to 01 for "Main Files".
        /// </item>
        /// </list>
        /// </remarks>
        /// <returns>The formatted Acquisition File Number.</returns>
        public static string GenerateAcquisitionFileNumber(short prefix, int fileNumber, int suffix = 1)
        {
            return $"{prefix:00}-{fileNumber}-{suffix:00}";
        }
    }
}
