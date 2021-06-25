using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Mapping.Lookup
{
    public class AgencyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Agency, Model.AgencyModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.SortOrder, src => src.SortOrder)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AgencyModel, Entity.Agency>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.SortOrder, src => src.SortOrder)
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
