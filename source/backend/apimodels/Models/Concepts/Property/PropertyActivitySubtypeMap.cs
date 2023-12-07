using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyActivitySubtypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropMgmtActivitySubtype, PropertyActivitySubtypeModel>()
                .Map(dest => dest.TypeCode, src => src.PropMgmtActivitySubtypeCode)
                .Map(dest => dest.ParentTypeCode, src => src.PropMgmtActivityTypeCode)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled);
        }
    }
}
