using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Mapping.Lookup
{
    public class LookupMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Province, Model.LookupModel>()
                 .Map(dest => dest.Id, src => src.Id)
                 .Map(dest => dest.Name, src => src.Code)
                 .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                 .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                 .Map(dest => dest.Type, src => src.GetType().Name)
                 .Inherits<Entity.BaseEntity, Models.BaseModel>();

            config.NewConfig<Entity.Country, Model.LookupModel>()
                 .Map(dest => dest.Id, src => src.Id)
                 .Map(dest => dest.Name, src => src.Code)
                 .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                 .Map(dest => dest.Type, src => src.GetType().Name)
                 .Inherits<Entity.BaseEntity, Models.BaseModel>();

            config.NewConfig<Entity.TypeEntity<string>, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Description != null ? src.Description : src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.BaseEntity, Models.BaseModel>();

            config.NewConfig<Entity.TypeEntity<int>, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.BaseEntity, Models.BaseModel>();

            config.NewConfig<Entity.Organization, Model.LookupModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Identifier)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.BaseEntity, Models.BaseModel>();
        }
    }
}
