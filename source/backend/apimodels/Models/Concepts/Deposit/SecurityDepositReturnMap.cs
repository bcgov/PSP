using System;
using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Deposit
{
    public class SecurityDepositReturnMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsSecurityDepositReturn, SecurityDepositReturnModel>()
                .Map(dest => dest.Id, src => src.SecurityDepositReturnId)
                .Map(dest => dest.ParentDepositId, src => src.SecurityDepositId)
                .Map(dest => dest.TerminationDate, src => DateOnly.FromDateTime(src.TerminationDate))
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.InterestPaid, src => src.InterestPaid)
                .Map(dest => dest.ReturnDate, src => DateOnly.FromDateTime(src.ReturnDate))
                .Map(dest => dest.ContactHolder, src => src.PimsSecurityDepositReturnHolder)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<SecurityDepositReturnModel, Entity.PimsSecurityDepositReturn>()
               .Map(dest => dest.SecurityDepositReturnId, src => src.Id)
                .Map(dest => dest.SecurityDepositId, src => src.ParentDepositId)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate.ToDateTime(TimeOnly.MinValue))
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.InterestPaid, src => src.InterestPaid)
                .Map(dest => dest.ReturnDate, src => src.ReturnDate.ToDateTime(TimeOnly.MinValue))
                .Map(dest => dest.PimsSecurityDepositReturnHolder, src => src.ContactHolder)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
