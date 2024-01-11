using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Lease
{
    public class PropertyImprovementModel : BaseAuditModel
    {
        #region Properties

        public long? Id { get; set; }

        public long? LeaseId { get; set; }

        public LeaseModel Lease { get; set; }

        public string Address { get; set; }

        public string StructureSize { get; set; }

        public string ImprovementDescription { get; set; }

        public CodeTypeModel<string> PropertyImprovementTypeCode { get; set; }

        #endregion
    }
}
