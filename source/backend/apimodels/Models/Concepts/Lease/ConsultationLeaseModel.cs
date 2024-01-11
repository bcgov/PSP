using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Lease
{
    public class ConsultationLeaseModel : BaseAuditModel
    {
        #region Properties

        public long Id { get; set; }

        public CodeTypeModel<string> ConsultationType { get; set; }

        public CodeTypeModel<string> ConsultationStatusType { get; set; }

        public long ParentLeaseId { get; set; }

        public string OtherDescription { get; set; }

        #endregion
    }
}
