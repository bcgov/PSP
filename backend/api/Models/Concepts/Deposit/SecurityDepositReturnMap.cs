using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class SecurityDepositReturnMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsSecurityDepositReturn, SecurityDepositReturnModel>()
                .Map(dest => dest.Id, src => src.SecurityDepositReturnId)
                .Map(dest => dest.ParentDepositId, src => src.SecurityDepositId)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.InterestPaid, src => src.InterestPaid)
                .Map(dest => dest.ReturnDate, src => src.ReturnDate)
                .Map(dest => dest.ContactHolder, src => src.PimsSecurityDepositReturnHolder)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<SecurityDepositReturnModel, Entity.PimsSecurityDepositReturn>()
               .Map(dest => dest.SecurityDepositReturnId, src => src.Id)
                .Map(dest => dest.SecurityDepositId, src => src.ParentDepositId)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.InterestPaid, src => src.InterestPaid)
                .Map(dest => dest.ReturnDate, src => src.ReturnDate)
                .Map(dest => dest.PimsSecurityDepositReturnHolder, src => src.ContactHolder)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
