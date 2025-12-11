using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ManagementFile
{
    public class ManagementFileContactMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsManagementFileContact, ManagementFileContactModel>()
                .Map(dest => dest.Id, src => src.ManagementFileContactId)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.Purpose, src => src.Purpose)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ManagementFileContactModel, Entity.PimsManagementFileContact>()
                .Map(dest => dest.ManagementFileContactId, src => src.Id)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.Purpose, src => src.Purpose)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
