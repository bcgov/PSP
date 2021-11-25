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
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.SecurityDepositHolderTypeId, src => src.SecDepHolderTypeCode)
                .Map(dest => dest.SecurityDepositHolderType, src => src.SecDepHolderTypeCodeNavigation.Description)
                .Map(dest => dest.SecurityDepositTypeId, src => src.SecurityDepositTypeCode)
                .Map(dest => dest.SecurityDepositType, src => src.SecurityDepositTypeCodeNavigation.Description)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.AmountPaid, src => src.AmountPaid)
                .Map(dest => dest.TotalAmount, src => src.TotalAmount)
                .Map(dest => dest.AnnualInterestRate, src => src.AnnualInterestRate)
                .Map(dest => dest.DepositDate, src => src.DepositDate);
        }
    }
}
