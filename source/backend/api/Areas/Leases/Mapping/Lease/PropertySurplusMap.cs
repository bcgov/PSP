using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PropertySurplusMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, Model.SurplusDeclarationModel>()
                .Map(dest => dest.Comment, src => src.SurplusDeclarationComment)
                .Map(dest => dest.Date, src => src.SurplusDeclarationDate)
                .Map(dest => dest.Type, src => src.SurplusDeclarationTypeCodeNavigation);
        }
    }
}
