namespace Pims.Api.Models.Concepts
{
    public class PropertyImprovementModel : BaseAppModel
    {
        #region Properties

        public long? Id { get; set; }

        public long? LeaseId { get; set; }

        public LeaseModel Lease { get; set; }

        public string Address { get; set; }

        public string StructureSize { get; set; }

        public string ImprovementDescription { get; set; }

        public TypeModel<string> PropertyImprovementTypeCode { get; set; }

        #endregion
    }
}
