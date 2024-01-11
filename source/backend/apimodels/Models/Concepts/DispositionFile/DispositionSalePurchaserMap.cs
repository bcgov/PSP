using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.DispositionFile;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionSalePurchaserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionPurchaser, DispositionSalePurchaserModel>()
                .Map(dest => dest.Id, src => src.DispositionPurchaserId)
                .Map(dest => dest.DispositionSaleId, src => src.DispositionSaleId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<DispositionSalePurchaserModel, Entity.PimsDispositionPurchaser>()
                .Map(dest => dest.DispositionPurchaserId, src => src.Id)
                .Map(dest => dest.DispositionSaleId, src => src.DispositionSaleId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
