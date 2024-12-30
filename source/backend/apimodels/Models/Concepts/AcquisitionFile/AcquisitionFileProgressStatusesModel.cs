using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileProgressStatusesModel : BaseAuditModel
    {
        /// <summary>
        /// get/set - AcquisitionFile Progress id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent Acquisition file id.
        /// </summary>
        public long AcquisitionFileId { get; set; }

        /// <summary>
        /// get/set - Progress Status type code.
        /// </summary>
        public virtual CodeTypeModel<string> ProgressStatusTypeCode { get; set; }
    }
}
