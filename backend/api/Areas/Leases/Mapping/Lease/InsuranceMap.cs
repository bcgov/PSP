using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class InsuranceMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Insurance, Model.InsuranceModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.InsuranceType, src => src.InsuranceType)
                .Map(dest => dest.InsurerOrganization, src => src.InsurerOrganization)
                .Map(dest => dest.InsurerContact, src => src.InsurerContact)
                .Map(dest => dest.MotiRiskManagementContact, src => src.MotiRiskManagementContact)
                .Map(dest => dest.BctfaRiskManagementContact, src => src.BctfaRiskManagementContact)
                .Map(dest => dest.InsurancePayeeType, src => src.InsurancePayeeType)
                .Map(dest => dest.OtherInsuranceType, src => src.OtherInsuranceType)
                .Map(dest => dest.CoverageDescription, src => src.CoverageDescription)
                .Map(dest => dest.CoverageLimit, src => src.CoverageLimit)
                .Map(dest => dest.InsuredValue, src => src.InsuredValue)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.RiskAssessmentCompletedDate, src => src.RiskAssessmentCompletedDate)
                .Map(dest => dest.InsuranceInPlace, src => (src.StartDate.Date <= System.DateTime.Now.Date) && (System.DateTime.Now.Date < src.ExpiryDate.Date));
        }
    }
}
