using Pims.Api.Models.Concepts.File;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Property;

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
