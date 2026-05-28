using System.Collections.Generic;

namespace Pims.Api.Models.Concepts.Document
{
    /// <summary>
    /// Carries the current user's file-access scope for document queries.
    /// </summary>
    public class DocumentAccessContext
    {
        public HashSet<short> UserRegions { get; set; } = new HashSet<short>();

        public long? PersonId { get; set; }

        public long? ContractorPersonId { get; set; }

        public bool CanViewAcquisitionFiles { get; set; } = true;

        public bool CanViewDispositionFiles { get; set; } = true;

        public bool CanViewLeases { get; set; } = true;

        public bool CanViewManagementFiles { get; set; } = true;

        public bool CanViewResearchFiles { get; set; } = true;

        public bool CanViewProjects { get; set; } = true;

        public bool CanViewProperties { get; set; } = true;
    }
}
