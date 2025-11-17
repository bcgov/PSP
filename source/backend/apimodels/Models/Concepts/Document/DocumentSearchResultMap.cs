using System.Linq;
using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentSearchResultMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDocument, DocumentSearchResultModel>()
                .Map(dest => dest.Id, src => src.DocumentId)
                .Map(dest => dest.MayanDocumentId, src => src.MayanId)
                .Map(dest => dest.DocumentType, src => src.DocumentType)
                .Map(dest => dest.StatusTypeCode, src => src.DocumentStatusTypeCodeNavigation)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.DocumentQueueStatusTypeCode, src => src.PimsDocumentQueues.Count > 0 ? src.PimsDocumentQueues.FirstOrDefault().DocumentQueueStatusTypeCodeNavigation : null)
                .Map(dest => dest.AcquisitionDocuments, src => src.PimsAcquisitionFileDocuments)
                .Map(dest => dest.DispositionDocuments, src => src.PimsDispositionFileDocuments)
                .Map(dest => dest.ManagementDocuments, src => src.PimsManagementFileDocuments)
                .Map(dest => dest.LeaseDocuments, src => src.PimsLeaseDocuments)
                .Map(dest => dest.MgmtActivitiesDocuments, src => src.PimsMgmtActivityDocuments)
                .Map(dest => dest.ProjectDocuments, src => src.PimsProjectDocuments)
                .Map(dest => dest.PropertiesDocuments, src => src.PimsPropertyDocuments)
                .Map(dest => dest.ResearchDocuments, src => src.PimsResearchFileDocuments)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();
        }
    }
}
