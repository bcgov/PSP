using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ActivityNoteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsActivityInstanceNote, EntityNoteModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.PimsActivityInstanceNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<EntityNoteModel, Entity.PimsActivityInstanceNote>()
                .Map(dest => dest.PimsActivityInstanceNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ActivityInstanceId, src => src.Parent.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsActivityInstanceNote, NoteParentModel>()
                .ConstructUsing(src => new NoteParentModel { Id = src.ActivityInstanceId });
        }
    }
}
