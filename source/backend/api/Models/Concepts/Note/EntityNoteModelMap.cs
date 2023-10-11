using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class EntityNoteModelMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // PimsAcquisitionFileNote -> EntityNoteModel
            config.NewConfig<Entity.PimsAcquisitionFileNote, EntityNoteModel>()
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

            config.NewConfig<Entity.PimsProjectNote, EntityNoteModel>()
                .Map(dest => dest.Id, src => src.ProjectNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<EntityNoteModel, Entity.PimsProjectNote>()
                .Map(dest => dest.ProjectNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ProjectId, src => src.Parent.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            // PimsResearchFileNote -> EntityNoteModel
            config.NewConfig<Entity.PimsResearchFileNote, EntityNoteModel>()
                .Map(dest => dest.Id, src => src.ResearchFileNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            // PimsResearchFileNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsResearchFileNote>()
                .Map(dest => dest.ResearchFileNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ResearchFileId, src => src.Parent.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsProjectNote, NoteParentModel>()
                .ConstructUsing(src => new NoteParentModel { Id = src.ProjectNoteId });
        }
    }
}
