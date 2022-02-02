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
                .Map(dest => dest.DepositType, src => src.SecurityDepositTypeCodeNavigation)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.ReturnDate, src => src.ReturnDate)
                .Map(dest => dest.PayeeName, src => src.PayeeName)
                .Map(dest => dest.PayeeAddress, src => src.PayeeAddress)
                .Map(dest => dest.PersonDepositReturnHolder, src => src.PimsSecurityDepositReturnHolders.FirstOrDefault(h => h.Person != null))
                .Map(dest => dest.PersonDepositReturnHolderId, src => src.PimsSecurityDepositReturnHolders.Select(h => h.PersonId).FirstOrDefault())
                .Map(dest => dest.OrganizationDepositReturnHolder, src => src.PimsSecurityDepositReturnHolders.FirstOrDefault(h => h.Organization != null))
                .Map(dest => dest.OrganizationDepositReturnHolderId, src => src.PimsSecurityDepositReturnHolders.Select(h => h.OrganizationId).FirstOrDefault())
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();

            config.NewConfig<Model.SecurityDepositReturnModel, Entity.PimsSecurityDepositReturn>()
               .Map(dest => dest.SecurityDepositReturnId, src => src.Id)
                .Map(dest => dest.SecurityDepositId, src => src.ParentDepositId)
                .Map(dest => dest.SecurityDepositTypeCode, src => src.DepositType.Id)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.ReturnDate, src => src.ReturnDate)
                .Map(dest => dest.PayeeName, src => src.PayeeName)
                .Map(dest => dest.PayeeAddress, src => src.PayeeAddress)
                .Map(dest => dest.PimsSecurityDepositReturnHolders, src => new List<Entity.PimsSecurityDepositReturnHolder>())
                .AfterMapping((src, dest) =>
                {
                    if (src.PersonDepositReturnHolderId.HasValue)
                    {
                        dest.PimsSecurityDepositReturnHolders.Add(new Entity.PimsSecurityDepositReturnHolder() { PersonId = src.PersonDepositReturnHolderId });
                    }
                    else if (src.OrganizationDepositReturnHolderId.HasValue)
                    {
                        dest.PimsSecurityDepositReturnHolders.Add(new Entity.PimsSecurityDepositReturnHolder() { OrganizationId = src.OrganizationDepositReturnHolderId });
                    }
                })
                .Inherits<Api.Models.BaseModel, Entity.IBaseEntity>();
        }
    }
}
