using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class CompensationRequisitionMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsCompensationRequisition, CompensationRequisitionModel>()
                .Map(dest => dest.Id, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.FiscalYear, src => src.FiscalYear)
                .Map(dest => dest.AgreementDate, src => src.AgreementDt)
                .Map(dest => dest.ExpropriationNoticeServedDate, src => src.ExpropNoticeServedDt)
                .Map(dest => dest.ExpropriationVestingDate, src => src.ExpropVestingDt)
                .Map(dest => dest.SpecialInstruction, src => src.SpecialInstruction)
                .Map(dest => dest.DetailedRemarks, src => src.DetailedRemarks)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Financials, src => src.PimsCompReqH120s)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<CompensationRequisitionModel, Entity.PimsCompensationRequisition>()
                .Map(dest => dest.CompensationRequisitionId, src => src.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.FiscalYear, src => src.FiscalYear)
                .Map(dest => dest.AgreementDt, src => src.AgreementDate)
                .Map(dest => dest.ExpropNoticeServedDt, src => src.ExpropriationNoticeServedDate)
                .Map(dest => dest.ExpropVestingDt, src => src.ExpropriationVestingDate)
                .Map(dest => dest.GenerationDt, src => src.GenerationDate)
                .Map(dest => dest.SpecialInstruction, src => src.SpecialInstruction)
                .Map(dest => dest.DetailedRemarks, src => src.DetailedRemarks)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PimsCompReqH120s, src => src.Financials)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
