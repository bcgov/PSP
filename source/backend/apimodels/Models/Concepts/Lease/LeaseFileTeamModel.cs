using Pims.Api.Models.Concepts.File;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeaseFileTeamModel : FileTeamModel
    {
        #region Properties

        /// <summary>
        /// Parent Lease File.
        /// </summary>
        public long LeaseId { get; set; }

        /// <summary>
        /// Parent Lease File.
        /// </summary>
        public override long ParentFileId
        {
            get { return LeaseId; }
            set { LeaseId = value; }
        }

        #endregion
    }
}
