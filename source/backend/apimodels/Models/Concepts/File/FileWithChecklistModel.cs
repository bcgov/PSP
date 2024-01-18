using System.Collections.Generic;

namespace Pims.Api.Models.Concepts.File
{
    public class FileWithChecklistModel : FileModel
    {
        #region Properties

        /// <summary>
        /// get/set - A list of file checklist items.
        /// </summary>
        public IList<FileChecklistItemModel> FileChecklistItems { get; set; }
        #endregion
    }
}
