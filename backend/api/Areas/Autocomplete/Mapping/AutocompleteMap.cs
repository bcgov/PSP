using Mapster;
using Entity = Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Api.Areas.Autocomplete.Mapping
{
    public class AutocompleteMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<IEnumerable<object>, Models.AutocompleteResponseModel>()
                .Map(dest => dest.Predictions, src => src);

            config.ForType<Entity.PimsOrganization, Models.AutocompletePrediction>()
                .Map(dest => dest.Text, src => src.Name)
                .Map(dest => dest.Id, src => src.Id);
        }
    }
}
