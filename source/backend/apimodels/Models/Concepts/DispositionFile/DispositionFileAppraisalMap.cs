using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileAppraisalMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionAppraisal, DispositionFileAppraisalModel>()
                .Map(dest => dest.Id, src => src.DispositionAppraisalId)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.AppraisedAmount, src => src.AppraisedAmt)
                .Map(dest => dest.AppraisalDate, src => src.AppraisalDt)
                .Map(dest => dest.BcaValueAmount, src => src.BcaValueAmt)
                .Map(dest => dest.BcaRollYear, src => src.BcaRollYear)
                .Map(dest => dest.ListPriceAmount, src => src.ListPriceAmt)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<DispositionFileAppraisalModel, Entity.PimsDispositionAppraisal>()
                .Map(dest => dest.DispositionAppraisalId, src => src.Id)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.AppraisedAmt, src => src.AppraisedAmount)
                .Map(dest => dest.AppraisalDt, src => src.AppraisalDate)
                .Map(dest => dest.BcaValueAmt, src => src.BcaValueAmount)
                .Map(dest => dest.BcaRollYear, src => src.BcaRollYear)
                .Map(dest => dest.ListPriceAmt, src => src.ListPriceAmount)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
