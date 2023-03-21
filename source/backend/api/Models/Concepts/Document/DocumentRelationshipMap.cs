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

            config.NewConfig<Entity.PimsActivityTemplateDocument, DocumentRelationshipModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.ActivityTemplateDocumentId)
                .Map(dest => dest.ParentId, src => src.ActivityTemplateId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.Templates)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsActivityTemplateDocument>()
                .Map(dest => dest.ActivityTemplateDocumentId, src => src.Id)
                .Map(dest => dest.ActivityTemplateId, src => src.ParentId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsAcquisitionFileDocument, DocumentRelationshipModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.AcquisitionFiles)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsAcquisitionFileDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsResearchFileDocument, DocumentRelationshipModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.ResearchFiles)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsResearchFileDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsProjectDocument, DocumentRelationshipModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.Projects)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsProjectDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);
        }
    }
}
