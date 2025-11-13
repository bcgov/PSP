using System.Collections.Generic;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentSearchResultModel : DocumentModel
    {
        public List<DocumentRelationshipModel> AcquisitionDocuments { get; set; }

        public List<DocumentRelationshipModel> DispositionDocuments { get; set; }

        public List<DocumentRelationshipModel> LeaseDocuments { get; set; }

        public List<DocumentRelationshipModel> ManagementDocuments { get; set; }

        public List<DocumentRelationshipModel> MgmtActivitiesDocuments { get; set; }

        public List<DocumentRelationshipModel> ProjectDocuments { get; set; }

        public List<PropertyDocumentRelationshipModel> PropertiesDocuments { get; set; }

        public List<DocumentRelationshipModel> ResearchDocuments { get; set; }

        public List<DocumentRelationshipModel> DocumentRelationships => GetAllRelationships();

        private List<DocumentRelationshipModel> GetAllRelationships()
        {
            List<DocumentRelationshipModel> relations = new();

            relations.AddRange(AcquisitionDocuments);
            relations.AddRange(DispositionDocuments);
            relations.AddRange(LeaseDocuments);
            relations.AddRange(ManagementDocuments);
            relations.AddRange(MgmtActivitiesDocuments);
            relations.AddRange(ProjectDocuments);
            relations.AddRange(PropertiesDocuments);
            relations.AddRange(ResearchDocuments);

            return relations;
        }
    }
}
