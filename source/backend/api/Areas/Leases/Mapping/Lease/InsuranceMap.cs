using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class InsuranceMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsInsurance, Model.InsuranceModel>()
                .Map(dest => dest.Id, src => src.InsuranceId)
                .Map(dest => dest.InsuranceType, src => src.InsuranceTypeCodeNavigation)
                .Map(dest => dest.OtherInsuranceType, src => src.OtherInsuranceType)
                .Map(dest => dest.CoverageDescription, src => src.CoverageDescription)
                .Map(dest => dest.CoverageLimit, src => src.CoverageLimit)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.IsInsuranceInPlace, src => src.IsInsuranceInPlace)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();
        }
    }
}
