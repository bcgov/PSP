namespace Pims.Api.Models.Concepts.Research
{
    using System;

    public class ResearchModel : BaseAppModel
    {
        public long ResearchFileId { get; set; }

        public TypeCode ResearchFileStatusTypeCode { get; set; }

        public string Name { get; set; }

        public string RfileNumber { get; set; }

        public string UpdatedByIdir { get; set; }

        public string CreatedByIdir { get; set; }
    }
}
