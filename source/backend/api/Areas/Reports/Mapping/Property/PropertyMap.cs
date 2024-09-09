using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Reports.Models.Property;

namespace Pims.Api.Areas.Reports.Mapping.Property
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, Model.PropertyModel>()
                .Map(dest => dest.PropertyTypeId, src => src.PropertyTypeCode)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.Municipality, src => src.Address.MunicipalityName)
                .Map(dest => dest.Postal, src => src.Address.PostalCode)
                .Map(dest => dest.Latitude, src => src.Location.Coordinate.Y)
                .Map(dest => dest.Longitude, src => src.Location.Coordinate.X)

                .Map(dest => dest.PID, src => src.ParcelIdentity)
                .Map(dest => dest.PIN, src => src.Pin)
                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.LandLegalDescription, src => src.LandLegalDescription);

            config.NewConfig<Entity.PimsPropertyVw, Model.PropertyModel>()
                .Map(dest => dest.PropertyTypeId, src => src.PropertyTypeCode)
                .Map(dest => dest.Address, src => $"{src.StreetAddress1} {src.StreetAddress2} {src.StreetAddress3}")
                .Map(dest => dest.Municipality, src => src.MunicipalityName)
                .Map(dest => dest.Postal, src => src.PostalCode)
                .Map(dest => dest.Latitude, src => src.Location.Coordinate.Y)
                .Map(dest => dest.Longitude, src => src.Location.Coordinate.X)

                .Map(dest => dest.PID, src => src.PidPadded)
                .Map(dest => dest.PIN, src => src.Pin)
                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.LandLegalDescription, src => src.LandLegalDescription);
        }
    }
}
