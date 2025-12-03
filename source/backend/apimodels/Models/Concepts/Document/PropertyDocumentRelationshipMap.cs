using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.CodeTypes;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document.Document
{
    public class PropertyDocumentRelationshipMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyDocument, PropertyDocumentRelationshipModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentId, src => src.FileId)
                .Map(dest => dest.Document, src => src.Document)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.RelationshipType, src => DocumentRelationType.Properties)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();
        }
    }
}
