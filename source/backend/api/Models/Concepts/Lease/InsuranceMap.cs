using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class InsuranceMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsInsurance, InsuranceModel>()
                .Map(dest => dest.Id, src => src.InsuranceId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.InsuranceType, src => src.InsuranceTypeCodeNavigation)
                .Map(dest => dest.OtherInsuranceType, src => src.OtherInsuranceType)
                .Map(dest => dest.CoverageDescription, src => src.CoverageDescription)
                .Map(dest => dest.CoverageLimit, src => src.CoverageLimit)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.IsInsuranceInPlace, src => src.IsInsuranceInPlace)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<InsuranceModel, Entity.PimsInsurance>()
                .Map(dest => dest.InsuranceId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.InsuranceTypeCode, src => src.InsuranceType.Id)
                .Map(dest => dest.OtherInsuranceType, src => src.OtherInsuranceType)
                .Map(dest => dest.CoverageDescription, src => src.CoverageDescription)
                .Map(dest => dest.CoverageLimit, src => src.CoverageLimit)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.IsInsuranceInPlace, src => src.IsInsuranceInPlace)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
