using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.CodeTypes;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document.Document
{
    public class DocumentRelationshipMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFileDocument, DocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.AcquisitionFiles)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsAcquisitionFileDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsResearchFileDocument, DocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.ResearchFiles)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsResearchFileDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsProjectDocument, DocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.Projects)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsProjectDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsFormType, DocumentRelationshipModel>()
                .Map(dest => dest.ParentId, src => src.FormTypeCode)

                // .Map(dest => dest.IsDisabled, src => src.IsDisabled) TODO: Verify if this needs to be removed
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.Templates);

            config.NewConfig<DocumentRelationshipModel, Entity.PimsFormType>()
                .Map(dest => dest.FormTypeCode, src => src.ParentId)

                // .Map(dest => dest.IsDisabled, src => src.IsDisabled)  TODO: Verify if this needs to be removed
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsLeaseDocument, DocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.Leases)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsLeaseDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsMgmtActivityDocument, DocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.ManagementActivities)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsMgmtActivityDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsDispositionFileDocument, DocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.DispositionFiles)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsDispositionFileDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsManagementFileDocument, DocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.ManagementFiles)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsManagementFileDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);

            config.NewConfig<Entity.PimsPropertyDocument, DocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.Properties)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<DocumentRelationshipModel, Entity.PimsPropertyDocument>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.FileId, src => src.ParentId)
                .Map(dest => dest.DocumentId, src => src.Document.Id)
                .Map(dest => dest.Document, src => src.Document);
        }
    }
}
