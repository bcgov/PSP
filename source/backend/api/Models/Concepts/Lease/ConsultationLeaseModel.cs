namespace Pims.Api.Models.Concepts
{
    public class ConsultationLeaseModel : BaseAppModel
    {
        #region Properties

        public long Id { get; set; }

        public TypeModel<string> ConsultationType { get; set; }

        public TypeModel<string> ConsultationStatusType { get; set; }

        public long ParentLeaseId { get; set; }

        public string OtherDescription { get; set; }

        #endregion
    }
}
