using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Note
{
    public class NoteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNote, NoteModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.Note, src => src.NoteTxt)
                .Map(dest => dest.IsSystemGenerated, src => src.IsSystemGenerated ?? false)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<NoteModel, Entity.PimsNote>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.NoteTxt, src => src.Note)
                .Map(dest => dest.IsSystemGenerated, src => src.IsSystemGenerated)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
