using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class SecurityDepositReturnMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.SecurityDepositReturn, Model.SecurityDepositReturnModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.RowVersion, src => src.RowVersion)
                .Map(dest => dest.SecurityDepositTypeId, src => src.SecurityDepositTypeId)
                .Map(dest => dest.SecurityDepositType, src => src.SecurityDepositType.Description)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.DepositTotal, src => src.DepositTotal)
                .Map(dest => dest.ClaimsAgainst, src => src.ClaimsAgainst)
                .Map(dest => dest.ReturnAmount, src => src.ReturnAmount)
                .Map(dest => dest.ChequeNumber, src => src.ChequeNumber)
                .Map(dest => dest.PayeeName, src => src.PayeeName)
                .Map(dest => dest.PayeeAddress, src => src.PayeeAddress)
                .Map(dest => dest.TerminationNote, src => src.TerminationNote);
        }
    }
}
