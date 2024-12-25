using Mapster;
using Pims.Api.Models.Base;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentQueueMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDocumentQueue, DocumentQueueModel>()
                .Map(dest => dest.Id, src => src.DocumentQueueId)
                .Map(dest => dest.DocumentExternalId, src => src.DocumentExternalId)
                .Map(dest => dest.DocumentId, src => src.DocumentId)
                .Map(dest => dest.DocumentQueueStatusType, src => src.DocumentQueueStatusTypeCodeNavigation == null ? new PimsDocumentQueueStatusType() { Id = src.DocumentQueueStatusTypeCode } : src.DocumentQueueStatusTypeCodeNavigation)
                .Map(dest => dest.DataSourceTypeCode, src => src.DataSourceTypeCodeNavigation)
                .Map(dest => dest.DocumentProcessStartTimestamp, src => src.DocProcessStartDt)
                .Map(dest => dest.DocumentProcessEndTimestamp, src => src.DocProcessEndDt)
                .Map(dest => dest.DocumentProcessRetries, src => src.DocProcessRetries)
                .Map(dest => dest.PimsDocument, src => src.DocumentNavigation)
                .Map(dest => dest.MayanError, src => src.MayanError)
                .Map(dest => dest.DocumentQueueStatusTypeCode, src => src.DocumentQueueStatusTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentQueueModel, Entity.PimsDocumentQueue>()
                .Map(dest => dest.DocumentQueueId, src => src.Id)
                .Map(dest => dest.DocumentExternalId, src => src.DocumentExternalId)
                .Map(dest => dest.DocumentId, src => src.DocumentId)
                .Map(dest => dest.DocumentQueueStatusTypeCode, src => src.DocumentQueueStatusType.Id)
                .Map(dest => dest.DataSourceTypeCode, src => src.DataSourceTypeCode.Id)
                .Map(dest => dest.DocProcessStartDt, src => src.DocumentProcessStartTimestamp)
                .Map(dest => dest.DocProcessEndDt, src => src.DocumentProcessEndTimestamp)
                .Map(dest => dest.DocProcessRetries, src => src.DocumentProcessRetries)
                .Map(dest => dest.DocumentNavigation, src => src.PimsDocument)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.MayanError, src => src.MayanError)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
