using Mapster;
using Pims.Dal.Entities.Models;
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
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber);

            config.NewConfig<EntityNoteModel, Entity.PimsActivityInstanceNote>()
                .Map(dest => dest.PimsActivityInstanceNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ActivityInstanceId, src => src.Parent.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion);

            config.NewConfig<Entity.PimsActivityInstanceNote, ParentModel>()
                .ConstructUsing(src => new ParentModel { Id = src.ActivityInstanceId });
        }
    }
}
