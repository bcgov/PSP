using Pims.Api.Models.Concepts.File;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFilePropertyModel : FilePropertyModel
    {
        #region Properties

        public new AcquisitionFileModel File { get; set; }

        #endregion
    }
}
