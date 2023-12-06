using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Claim
{

    public class ClaimMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsClaim, ClaimModel>()
                .Map(dest => dest.Id, src => src.ClaimId)
                .Map(dest => dest.Key, src => src.ClaimUid)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.KeycloakRoleId, src => src.KeycloakRoleId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<ClaimModel, Entity.PimsClaim>()
                .Map(dest => dest.ClaimId, src => src.Id)
                .Map(dest => dest.ClaimUid, src => src.Key)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.KeycloakRoleId, src => src.KeycloakRoleId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
