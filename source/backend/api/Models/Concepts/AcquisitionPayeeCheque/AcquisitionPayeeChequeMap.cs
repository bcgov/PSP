using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionPayeeChequeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcqPayeeCheque, AcquisitionPayeeChequeModel>()
                .Map(dest => dest.Id, src => src.AcqPayeeChequeId)
                .Map(dest => dest.AcquisitionPayeeId, src => src.AcquisitionPayeeId)
                .Map(dest => dest.PretaxAmout, src => src.PretaxAmt)
                .Map(dest => dest.IsGSTRequired, src => src.IsGstRequired)
                .Map(dest => dest.TaxAmount, src => src.TaxAmt)
                .Map(dest => dest.TotalAmount, src => src.TotalAmt)
                // .Map(dest => dest.GSTNumber, src => src.GSTNumber) // TODO : Check for number
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<AcquisitionPayeeChequeModel, Entity.PimsAcqPayeeCheque>()
                .Map(dest => dest.AcqPayeeChequeId, src => src.Id)
                .Map(dest => dest.AcquisitionPayeeId, src => src.AcquisitionPayeeId)
                .Map(dest => dest.PretaxAmt, src => src.PretaxAmout)
                .Map(dest => dest.IsGstRequired, src => src.IsGSTRequired)
                .Map(dest => dest.TaxAmt, src => src.TaxAmount)
                .Map(dest => dest.TotalAmt, src => src.TotalAmount)
                // .Map(dest => dest.GSTNumber, src => src.GSTNumber) // TODO : Check for number
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
