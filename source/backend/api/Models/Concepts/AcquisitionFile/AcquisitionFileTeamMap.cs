using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileTeamMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFileTeam, AcquisitionFileTeamModel>()
                .Map(dest => dest.Id, src => src.AcquisitionFileTeamId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.TeamProfileType, src => src.AcqFlTeamProfileTypeCodeNavigation)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.TeamProfileTypeCode, src => src.AcqFlTeamProfileTypeCode)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<AcquisitionFileTeamModel, Entity.PimsAcquisitionFileTeam>()
                .Map(dest => dest.AcquisitionFileTeamId, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.AcqFlTeamProfileTypeCode, src => src.TeamProfileTypeCode)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
