using Mapster;
using Pims.Api.Constants;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentRelationshipMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsActivityInstanceDocument, DocumentRelationshipModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.ActivityInstanceDocumentId)
                .Map(dest => dest.ParentId, src => src.ActivityInstanceId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.Activities)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsActivityInstanceDocument>()
                .Map(dest => dest.ActivityInstanceDocumentId, src => src.Id)
                .Map(dest => dest.ActivityInstanceId, src => src.ParentId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
