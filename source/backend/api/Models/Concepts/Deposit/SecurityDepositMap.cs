using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class SecurityDepositMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsSecurityDeposit, SecurityDepositModel>()
                .Map(dest => dest.Id, src => src.SecurityDepositId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.AmountPaid, src => src.AmountPaid)
                .Map(dest => dest.DepositDate, src => src.DepositDate)
                .Map(dest => dest.DepositType, src => src.SecurityDepositTypeCodeNavigation)
                .Map(dest => dest.OtherTypeDescription, src => src.OtherDepositTypeDesc)
                .Map(dest => dest.DepositReturns, src => src.PimsSecurityDepositReturns)
                .Map(dest => dest.ContactHolder, src => src.PimsSecurityDepositHolder)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<SecurityDepositModel, Entity.PimsSecurityDeposit>()
                .Map(dest => dest.SecurityDepositId, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.AmountPaid, src => src.AmountPaid)
                .Map(dest => dest.DepositDate, src => src.DepositDate)
                .Map(dest => dest.SecurityDepositTypeCode, src => src.DepositType.Id)
                .Map(dest => dest.OtherDepositTypeDesc, src => src.OtherTypeDescription)
                .Map(dest => dest.PimsSecurityDepositHolder, src => src.ContactHolder)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
