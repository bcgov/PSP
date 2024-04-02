using Pims.Api.Models.Concepts.File;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFilePropertyModel : FilePropertyModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship's disposition file.
        /// </summary>
        public new DispositionFileModel File { get; set; }
        #endregion
    }
}
