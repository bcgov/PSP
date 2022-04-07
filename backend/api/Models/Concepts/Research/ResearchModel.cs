namespace Pims.Api.Models.Concepts
{
    using System;

    public class ResearchModel : BaseAppModel
    {
        public long Id { get; set; }

        public TypeCode ResearchFileStatusTypeCode { get; set; }

        public string Name { get; set; }

        public string RoadName { get; set; }

        public string RoadAlias { get; set; }

        public string RfileNumber { get; set; }

        public string UpdatedByIdir { get; set; }

        public string CreatedByIdir { get; set; }
    }
}
