using Pims.Api.Models.Concepts.File;

namespace Pims.Api.Models.Concepts.ManagementFile
{
    public class ManagementFilePropertyModel : FilePropertyModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship's disposition file.
        /// </summary>
        public new ManagementFileModel File { get; set; }
        #endregion
    }
}
