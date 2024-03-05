using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileOfferMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionOffer, DispositionFileOfferModel>()
                .Map(dest => dest.Id, src => src.DispositionOfferId)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.DispositionOfferStatusTypeCode, src => src.DispositionOfferStatusTypeCode)
                .Map(dest => dest.DispositionOfferStatusType, src => src.DispositionOfferStatusTypeCodeNavigation)
                .Map(dest => dest.OfferName, src => src.OfferName)
                .Map(dest => dest.OfferDate, src => src.OfferDt)
                .Map(dest => dest.OfferExpiryDate, src => src.OfferExpiryDt)
                .Map(dest => dest.OfferAmount, src => src.OfferAmt)
                .Map(dest => dest.OfferNote, src => src.OfferNote)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<DispositionFileOfferModel, Entity.PimsDispositionOffer>()
                .Map(dest => dest.DispositionOfferId, src => src.Id)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.DispositionOfferStatusTypeCode, src => src.DispositionOfferStatusTypeCode)
                .Map(dest => dest.OfferName, src => src.OfferName)
                .Map(dest => dest.OfferDt, src => src.OfferDate)
                .Map(dest => dest.OfferExpiryDt, src => src.OfferExpiryDate)
                .Map(dest => dest.OfferAmt, src => src.OfferAmount)
                .Map(dest => dest.OfferNote, src => src.OfferNote)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
