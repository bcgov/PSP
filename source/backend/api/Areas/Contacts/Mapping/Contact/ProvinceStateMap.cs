using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Contact.Models.Contact;

namespace Pims.Api.Areas.Contact.Mapping.Contact
{
    public class ProvinceStateMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProvinceState, Model.ProvinceStateModel>()
                .Map(dest => dest.ProvinceStateId, src => src.ProvinceStateId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.ProvinceStateCode, src => src.ProvinceStateCode);
        }
    }
}
