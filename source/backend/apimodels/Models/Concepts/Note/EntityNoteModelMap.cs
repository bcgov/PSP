using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Note
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
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            // PimsAcquisitionFileNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsAcquisitionFileNote>()
                .Map(dest => dest.AcquisitionFileNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.AcquisitionFileId, src => src.Parent.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            // PimsDispositionFileNote -> EntityNoteModel
            config.NewConfig<Entity.PimsDispositionFileNote, EntityNoteModel>()
                .Map(dest => dest.Id, src => src.DispositionFileNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            // PimsDispositionFileNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsDispositionFileNote>()
                .Map(dest => dest.DispositionFileNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.DispositionFileId, src => src.Parent.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            // PimsLeaseNote -> EntityNoteModel
            config.NewConfig<Entity.PimsLeaseNote, EntityNoteModel>()
                .Map(dest => dest.Id, src => src.LeaseNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            // PimsLeaseNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsLeaseNote>()
                .Map(dest => dest.LeaseNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.LeaseId, src => src.Parent.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            // PimsManagementNote -> EntityNoteModel
            config.NewConfig<Entity.PimsManagementFileNote, EntityNoteModel>()
                .Map(dest => dest.Id, src => src.ManagementFileNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            // PimsManagementNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsManagementFileNote>()
                .Map(dest => dest.ManagementFileNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ManagementFileId, src => src.Parent.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            // PimsProjectNote -> EntityNoteModel
            config.NewConfig<Entity.PimsProjectNote, EntityNoteModel>()
                .Map(dest => dest.Id, src => src.ProjectNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            // PimsProjectNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsProjectNote>()
                .Map(dest => dest.ProjectNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ProjectId, src => src.Parent.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            // PimsResearchFileNote -> EntityNoteModel
            config.NewConfig<Entity.PimsResearchFileNote, EntityNoteModel>()
                .Map(dest => dest.Id, src => src.ResearchFileNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            // PimsResearchFileNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsResearchFileNote>()
                .Map(dest => dest.ResearchFileNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.ResearchFileId, src => src.Parent.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            // PimsPropertyNote -> EntityNoteModel
            config.NewConfig<Entity.PimsPropertyNote, EntityNoteModel>()
                .Map(dest => dest.Id, src => src.PropertyNoteId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Parent, src => src)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            // PimsPropertyNote <- EntityNoteModel
            config.NewConfig<EntityNoteModel, Entity.PimsPropertyNote>()
                .Map(dest => dest.PropertyNoteId, src => src.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.PropertyId, src => src.Parent.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsProjectNote, NoteParentModel>()
                .ConstructUsing(src => new NoteParentModel { Id = src.ProjectNoteId });
        }
    }
}
