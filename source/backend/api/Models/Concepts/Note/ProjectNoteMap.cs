using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ProjectNoteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProjectNote, EntityNoteModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.ProjectNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<EntityNoteModel, Entity.PimsProjectNote>()
                .Map(dest => dest.ProjectNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ProjectId, src => src.Parent.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsProjectNote, NoteParentModel>()
                .ConstructUsing(src => new NoteParentModel { Id = src.ProjectNoteId });
        }
    }
}
