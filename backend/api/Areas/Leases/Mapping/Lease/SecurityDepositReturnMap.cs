using System.Collections.Generic;
using System.Linq;
using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class SecurityDepositReturnMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsSecurityDepositReturn, Model.SecurityDepositReturnModel>()
                .Map(dest => dest.Id, src => src.SecurityDepositReturnId)
                .Map(dest => dest.ParentDepositId, src => src.SecurityDepositId)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.ReturnDate, src => src.ReturnDate)
                .Map(dest => dest.PersonDepositReturnHolder, src => src.PimsSecurityDepositReturnHolder.Person)
                .Map(dest => dest.PersonDepositReturnHolderId, src => src.PimsSecurityDepositReturnHolder.PersonId)
                .Map(dest => dest.OrganizationDepositReturnHolder, src => src.PimsSecurityDepositReturnHolder.Organization)
                .Map(dest => dest.OrganizationDepositReturnHolderId, src => src.PimsSecurityDepositReturnHolder.OrganizationId)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();

            config.NewConfig<Model.SecurityDepositReturnModel, Entity.PimsSecurityDepositReturn>()
               .Map(dest => dest.SecurityDepositReturnId, src => src.Id)
                .Map(dest => dest.SecurityDepositId, src => src.ParentDepositId)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.ReturnDate, src => src.ReturnDate)
                .AfterMapping((src, dest) =>
                {
                    if (src.PersonDepositReturnHolderId.HasValue)
                    {
                        dest.PimsSecurityDepositReturnHolder = new Entity.PimsSecurityDepositReturnHolder() { PersonId = src.PersonDepositReturnHolderId };
                    }
                    else if (src.OrganizationDepositReturnHolderId.HasValue)
                    {
                        dest.PimsSecurityDepositReturnHolder = new Entity.PimsSecurityDepositReturnHolder() { OrganizationId = src.OrganizationDepositReturnHolderId };
                    }
                })
                .Inherits<Api.Models.BaseModel, Entity.IBaseEntity>();
        }
    }
}
