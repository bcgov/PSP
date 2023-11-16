using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ProjectProductMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProjectProduct, ProjectProductModel>()
                .Map(dest => dest.Id, src => src.ProjectProductId)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.Product, src => src.Product)
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.Project, src => src.Project)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<ProjectProductModel, Entity.PimsProjectProduct>()
                .Map(dest => dest.ProjectProductId, src => src.Id)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.ProductId, src => src.Product.Id)
                .Map(dest => dest.Product, src => src.Product)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
