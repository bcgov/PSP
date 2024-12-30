using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileTakingStatusesModel : BaseAuditModel
    {
        /// <summary>
        /// get/set - AcquisitionFile Taking type id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent Acquisition file id.
        /// </summary>
        public long AcquisitionFileId { get; set; }

        /// <summary>
        /// get/set - Taking type code.
        /// </summary>
        public virtual CodeTypeModel<string> TakingStatusTypeCode { get; set; }
    }
}
