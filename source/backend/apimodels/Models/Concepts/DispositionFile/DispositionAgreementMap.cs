using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionAgreementMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionAgreement, DispositionAgreementModel>()
                .Map(dest => dest.AgreementId, src => src.DispositionAgreementId)
                .Map(dest => dest.FileId, src => src.DispositionFileId)
                .Map(dest => dest.AgreementType, src => src.DispositionAgreementTypeCodeNavigation)
                .Map(dest => dest.AgreementStatusType, src => src.DspAgreementStatusTypeCodeNavigation)
                .Map(dest => dest.AgreementDate, src => src.AgreementDate)
                .Map(dest => dest.CompletionDate, src => src.CompletionDate)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.CommencementDate, src => src.CommencementDate)
                .Map(dest => dest.PossessionDate, src => src.PossessionDate)
                .Map(dest => dest.DepositAmount, src => src.DepositAmount)
                .Map(dest => dest.NoLaterThanDays, src => src.NoLaterThanDays)
                .Map(dest => dest.PurchasePrice, src => src.PurchasePrice)
                .Map(dest => dest.LegalSurveyPlanNum, src => src.LegalSurveyPlanNum)
                .Map(dest => dest.OfferDate, src => src.OfferDate)
                .Map(dest => dest.ExpiryDateTime, src => src.ExpiryTs)
                .Map(dest => dest.SignedDate, src => src.SignedDate)
                .Map(dest => dest.InspectionDate, src => src.InspectionDate)
                .Map(dest => dest.CancellationNote, src => src.CancellationNote)
                .Inherits<Entity.IBaseAppEntity, BaseConcurrentModel>();

            config.NewConfig<DispositionAgreementModel, Entity.PimsDispositionAgreement>()
                .Map(dest => dest.DispositionAgreementId, src => src.AgreementId)
                .Map(dest => dest.DispositionFileId, src => src.FileId)
                .Map(dest => dest.DispositionAgreementTypeCode, src => src.AgreementType.Id)
                .Map(dest => dest.DspAgreementStatusTypeCode, src => src.AgreementStatusType.Id)
                .Map(dest => dest.AgreementDate, src => src.AgreementDate)
                .Map(dest => dest.CompletionDate, src => src.CompletionDate)
                .Map(dest => dest.TerminationDate, src => src.TerminationDate)
                .Map(dest => dest.CommencementDate, src => src.CommencementDate)
                .Map(dest => dest.PossessionDate, src => src.PossessionDate)
                .Map(dest => dest.DepositAmount, src => src.DepositAmount)
                .Map(dest => dest.NoLaterThanDays, src => src.NoLaterThanDays)
                .Map(dest => dest.PurchasePrice, src => src.PurchasePrice)
                .Map(dest => dest.LegalSurveyPlanNum, src => src.LegalSurveyPlanNum)
                .Map(dest => dest.OfferDate, src => src.OfferDate)
                .Map(dest => dest.ExpiryTs, src => src.ExpiryDateTime)
                .Map(dest => dest.SignedDate, src => src.SignedDate)
                .Map(dest => dest.InspectionDate, src => src.InspectionDate)
                .Map(dest => dest.CancellationNote, src => src.CancellationNote)
                .Inherits<BaseConcurrentModel, Entity.IBaseAppEntity>();
        }
    }
}
