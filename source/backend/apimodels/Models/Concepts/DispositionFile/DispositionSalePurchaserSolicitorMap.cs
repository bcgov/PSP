using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionSalePurchaseSolicitorMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDspPurchSolicitor, DispositionSalePurchaserSolicitorModel>()
                .Map(dest => dest.Id, src => src.DspPurchSolicitorId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<DispositionSalePurchaserSolicitorModel, Entity.PimsDspPurchSolicitor>()
                .Map(dest => dest.DspPurchSolicitorId, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
