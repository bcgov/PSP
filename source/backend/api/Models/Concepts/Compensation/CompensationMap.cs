using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class CompensationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsCompensationRequisition, CompensationModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.FiscalYear, src => src.FiscalYear)
                .Map(dest => dest.AgreementDateTime, src => src.AgreementDt)
                .Map(dest => dest.ExpropiationNoticeServedDatetTime, src => src.ExpropNoticeServedDt)
                .Map(dest => dest.ExpropiationVestingDateTime, src => src.ExpropVestingDt)
                .Map(dest => dest.GenerationDatetTime, src => src.GenerationDt)
                .Map(dest => dest.SpecialInstruction, src => src.SpecialInstruction)
                .Map(dest => dest.DetailedRemarks, src => src.DetailedRemarks)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Financials, src => src.PimsCompReqH120s)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<CompensationModel, Entity.PimsCompensationRequisition>()
                .PreserveReference(true)
                  .Map(dest => dest.CompensationRequisitionId, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.FiscalYear, src => src.FiscalYear)
                .Map(dest => dest.AgreementDt, src => src.AgreementDateTime)
                .Map(dest => dest.ExpropNoticeServedDt, src => src.ExpropiationNoticeServedDatetTime)
                .Map(dest => dest.ExpropVestingDt, src => src.ExpropiationVestingDateTime)
                .Map(dest => dest.GenerationDt, src => src.GenerationDatetTime)
                .Map(dest => dest.SpecialInstruction, src => src.SpecialInstruction)
                .Map(dest => dest.DetailedRemarks, src => src.DetailedRemarks)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PimsCompReqH120s, src => src.Financials)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
