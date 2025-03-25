/*
* Frontend model
* LINK @frontend/src\models\api\DispositionFile.ts:43
*/

using Pims.Api.Models.Concepts.File;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileTeamModel : FileTeamModel
    {
        #region Properties

        /// <summary>
        /// Parent Disposition File.
        /// </summary>
        public long DispositionFileId { get; set; }

        /// <summary>
        /// Parent Disposition File.
        /// </summary>
        public override long ParentFileId
        {
            get { return DispositionFileId; } set { DispositionFileId = value; }
        }

        #endregion
    }
}
