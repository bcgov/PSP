using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Property.Models.Search;

namespace Pims.Api.Areas.Property.Mapping.Search
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Property, Model.PropertyModel>()
                .Map(dest => dest.PropertyType, src => src.PropertyTypeId)
                .Map(dest => dest.Classification, src => src.ClassificationId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSensitive, src => src.IsSensitive)

                .Map(dest => dest.Latitude, src => src.Location.Y)
                .Map(dest => dest.Longitude, src => src.Location.X)
                .Map(dest => dest.AddressId, src => src.AddressId)
                .Map(dest => dest.Address, src => src.Address == null ? null : src.Address.ToString().Trim())
                .Map(dest => dest.Province, src => src.Address == null ? null : src.Address.Province.Code)
                .Map(dest => dest.Municipality, src => src.Address == null ? null : src.Address.Municipality)
                .Map(dest => dest.Postal, src => src.Address == null ? null : src.Address.Postal);
        }
    }
}
