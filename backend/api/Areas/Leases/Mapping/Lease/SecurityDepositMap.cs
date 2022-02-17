using System.Collections.Generic;
using System.Linq;
using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class SecurityDepositMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsSecurityDeposit, Model.SecurityDepositModel>()
                .Map(dest => dest.Id, src => src.SecurityDepositId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.AmountPaid, src => src.AmountPaid)
                .Map(dest => dest.DepositDate, src => src.DepositDate)
                .Map(dest => dest.DepositType, src => src.SecurityDepositTypeCodeNavigation)
                .Map(dest => dest.OtherTypeDescription, src => src.OtherDepositTypeDesc)
                .Map(dest => dest.DepositReturns, src => src.PimsSecurityDepositReturns)
                .Map(dest => dest.PersonDepositHolder, src => src.PimsSecurityDepositHolder.Person)
                .Map(dest => dest.PersonDepositHolderId, src => src.PimsSecurityDepositHolder.PersonId)
                .Map(dest => dest.OrganizationDepositHolder, src => src.PimsSecurityDepositHolder.Organization)
                .Map(dest => dest.OrganizationDepositHolderId, src => src.PimsSecurityDepositHolder.OrganizationId)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();

            config.NewConfig<Model.SecurityDepositModel, Entity.PimsSecurityDeposit>()
                .Map(dest => dest.SecurityDepositId, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.AmountPaid, src => src.AmountPaid)
                .Map(dest => dest.DepositDate, src => src.DepositDate)
                .Map(dest => dest.SecurityDepositTypeCode, src => src.DepositType.Id)
                .Map(dest => dest.OtherDepositTypeDesc, src => src.OtherTypeDescription)
                .AfterMapping((src, dest) =>
                {
                    if (src.PersonDepositHolderId.HasValue)
                    {
                        dest.PimsSecurityDepositHolder = new Entity.PimsSecurityDepositHolder() { PersonId = src.PersonDepositHolderId };
                    }
                    else if (src.OrganizationDepositHolderId.HasValue)
                    {
                        dest.PimsSecurityDepositHolder = new Entity.PimsSecurityDepositHolder() { OrganizationId = src.OrganizationDepositHolderId };
                    }
                })
                .Inherits<Api.Models.BaseModel, Entity.IBaseEntity>();


        }
    }
}
