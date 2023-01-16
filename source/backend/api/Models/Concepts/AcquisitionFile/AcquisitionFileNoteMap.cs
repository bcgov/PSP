using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileNoteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFileNote, EntityNoteModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.AcquisitionFileNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<EntityNoteModel, Entity.PimsAcquisitionFileNote>()
                .Map(dest => dest.AcquisitionFileNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.AcquisitionFileId, src => src.Parent.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsActivityInstanceNote, NoteParentModel>()
                .ConstructUsing(src => new NoteParentModel { Id = src.ActivityInstanceId });
        }
    }
}
