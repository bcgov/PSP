using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Insurance.Models;

namespace Pims.Api.Areas.Insurance.Mapping
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

            config.NewConfig<Model.InsuranceModel, Entity.PimsInsurance>()
                .Map(dest => dest.InsuranceId, src => src.Id)
                .Map(dest => dest.InsuranceTypeCode, src => src.InsuranceType.Id)
                .Map(dest => dest.OtherInsuranceType, src => src.OtherInsuranceType)
                .Map(dest => dest.CoverageDescription, src => src.CoverageDescription)
                .Map(dest => dest.CoverageLimit, src => src.CoverageLimit)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.IsInsuranceInPlace, src => src.IsInsuranceInPlace)
                .Inherits<Api.Models.BaseModel, Entity.IBaseEntity>();
        }
    }
}
