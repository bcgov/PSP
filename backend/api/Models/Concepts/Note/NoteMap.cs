using Mapster;
using Pims.Dal.Entities.Models;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class NoteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNote, NoteModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Note, src => src.NoteTxt)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<NoteModel, Entity.PimsNote>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.NoteTxt, src => src.Note)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
