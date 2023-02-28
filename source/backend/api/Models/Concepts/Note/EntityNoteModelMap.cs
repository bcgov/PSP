using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class EntityNoteModelMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // PimsActivityInstanceNote -> EntityNoteModel
            config.NewConfig<Entity.PimsActivityInstanceNote, EntityNoteModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.PimsActivityInstanceNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            // PimsActivityInstanceNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsActivityInstanceNote>()
                .Map(dest => dest.PimsActivityInstanceNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ActivityInstanceId, src => src.Parent.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsActivityInstanceNote, NoteParentModel>()
                .ConstructUsing(src => new NoteParentModel { Id = src.ActivityInstanceId });

            // PimsAcquisitionFileNote -> EntityNoteModel
            config.NewConfig<Entity.PimsAcquisitionFileNote, EntityNoteModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.AcquisitionFileNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            // PimsAcquisitionFileNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsAcquisitionFileNote>()
                .Map(dest => dest.AcquisitionFileNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.AcquisitionFileId, src => src.Parent.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            // PimsLeaseNote -> EntityNoteModel
            config.NewConfig<Entity.PimsLeaseNote, EntityNoteModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.LeaseNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            // PimsLeaseNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsLeaseNote>()
                .Map(dest => dest.LeaseNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.LeaseId, src => src.Parent.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
