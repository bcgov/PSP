using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Role.AccessRequest
{
    public class RoleClaimMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRoleClaim, RoleClaimModel>()
                .MaxDepth(3)
                .Map(dest => dest.Id, src => src.RoleClaimId)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Claim, src => src.Claim)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.ClaimId, src => src.ClaimId)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<RoleClaimModel, Entity.PimsRoleClaim>()
                .MaxDepth(3)
                .Map(dest => dest.RoleClaimId, src => src.Id)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Claim, src => src.Claim)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.ClaimId, src => src.ClaimId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
