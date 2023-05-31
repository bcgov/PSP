using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFilePayeeChequeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcqPayeeCheque, AcquisitionFilePayeeChequeModel>()
                .Map(dest => dest.Id, src => src.AcqPayeeChequeId)
                .Map(dest => dest.AcquisitionPayeeId, src => src.AcquisitionPayeeId)
                .Map(dest => dest.PretaxAmount, src => src.PretaxAmt)
                .Map(dest => dest.TaxAmount, src => src.TaxAmt)
                .Map(dest => dest.TotalAmount, src => src.TotalAmt)
                .Map(dest => dest.GstAmount, src => src.GstNumber)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<AcquisitionFilePayeeChequeModel, Entity.PimsAcqPayeeCheque>()
                .Map(dest => dest.AcqPayeeChequeId, src => src.Id)
                .Map(dest => dest.AcquisitionPayeeId, src => src.AcquisitionPayeeId)
                .Map(dest => dest.PretaxAmt, src => src.PretaxAmount)
                .Map(dest => dest.TaxAmt, src => src.TaxAmount)
                .Map(dest => dest.TotalAmt, src => src.TotalAmount)
                .Map(dest => dest.GstNumber, src => src.GstAmount)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}