using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileOwnerSolicitorMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionOwnerSolicitor, AcquisitionFileOwnerSolicitorModel>()
                .Map(dest => dest.Id, src => src.OwnerSolicitorId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<AcquisitionFileOwnerSolicitorModel, Entity.PimsAcquisitionOwnerSolicitor>()
                .Map(dest => dest.OwnerSolicitorId, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
