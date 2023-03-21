using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileOwnerMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionOwner, AcquisitionFileOwnerModel>()
                .Map(dest => dest.Id, src => src.AcquisitionOwnerId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.LastNameOrCorp1, src => src.LastNameAndCorpName)
                .Map(dest => dest.LastNameOrCorp2, src => src.LastNameAndCorpName)
                .Map(dest => dest.GivenName, src => src.GivenName)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.Address, src => src.Address);

            config.NewConfig<AcquisitionFileOwnerModel, Entity.PimsAcquisitionOwner>()
                .Map(dest => dest.AcquisitionOwnerId, src => src.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.GivenName, src => src.GivenName)
                .Map(dest => dest.LastNameAndCorpName, src => src.LastNameOrCorp1)
                .Map(dest => dest.LastNameAndCorpName, src => src.LastNameOrCorp2)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.AddressId, src => src.Address.Id)
                .Map(dest => dest.Address, src => src.Address);
        }
    }
}
