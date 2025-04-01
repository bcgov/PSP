using Pims.Api.Models.Concepts.File;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileTeamModel : FileTeamModel
    {
        #region Properties

        /// <summary>
        /// Parent Acquisition File.
        /// </summary>
        public long AcquisitionFileId { get; set; }

        /// <summary>
        /// Parent Acquisition File.
        /// </summary>
        public override long ParentFileId
        {
            get { return AcquisitionFileId; }
            set { AcquisitionFileId = value; }
        }

        #endregion
    }
}
