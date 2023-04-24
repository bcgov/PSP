using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AgreementMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAgreement, AgreementModel>()
                .Map(dest => dest.AgreementId, src => src.AgreementId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AgreementType, src => src.AgreementTypeCodeNavigation)
                .Map(dest => dest.AgreementDate, src => src.AgreementDate)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.CompletionDate, src => src.CompletionDate)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.CommencementDate, src => src.CommencementDate)
                .Map(dest => dest.DepositAmount, src => src.DepositAmount)
                .Map(dest => dest.NoLaterThanDays, src => src.NoLaterThanDays)
                .Map(dest => dest.PurchasePrice, src => src.PurchasePrice)
                .Map(dest => dest.LegalSurveyPlanNum, src => src.LegalSurveyPlanNum)
                .Map(dest => dest.OfferDate, src => src.OfferDate)
                .Map(dest => dest.ExpiryDateTime, src => src.ExpiryTs)
                .Map(dest => dest.SignedDate, src => src.SignedDate)
                .Map(dest => dest.InspectionDate, src => src.InspectionDate)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<AgreementModel, Entity.PimsAgreement>()
                .Map(dest => dest.AgreementId, src => src.AgreementId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AgreementTypeCode, src => src.AgreementType.Id)
                .Map(dest => dest.AgreementDate, src => src.AgreementDate)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.CompletionDate, src => src.CompletionDate)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.CommencementDate, src => src.CommencementDate)
                .Map(dest => dest.DepositAmount, src => src.DepositAmount)
                .Map(dest => dest.NoLaterThanDays, src => src.NoLaterThanDays)
                .Map(dest => dest.PurchasePrice, src => src.PurchasePrice)
                .Map(dest => dest.LegalSurveyPlanNum, src => src.LegalSurveyPlanNum)
                .Map(dest => dest.OfferDate, src => src.OfferDate)
                .Map(dest => dest.ExpiryTs, src => src.ExpiryDateTime)
                .Map(dest => dest.SignedDate, src => src.SignedDate)
                .Map(dest => dest.InspectionDate, src => src.InspectionDate)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
