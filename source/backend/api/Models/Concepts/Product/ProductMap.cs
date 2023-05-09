using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ProductMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProduct, ProductModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ParentProject, src => src.ParentProject)
                .Map(dest => dest.ParentProjectId, src => src.ParentProjectId)
                .Map(dest => dest.AcquisitionFiles, src => src.PimsAcquisitionFiles)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.CostEstimate, src => src.CostEstimate)
                .Map(dest => dest.CostEstimateDate, src => src.CostEstimateDate)
                .Map(dest => dest.Objective, src => src.Objective)
                .Map(dest => dest.Scope, src => src.Scope)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<ProductModel, Entity.PimsProduct>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.ParentProjectId, src => src.ParentProjectId)
                .Map(dest => dest.PimsAcquisitionFiles, src => src.AcquisitionFiles)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.CostEstimate, src => src.CostEstimate)
                .Map(dest => dest.CostEstimateDate, src => src.CostEstimateDate)
                .Map(dest => dest.Objective, src => src.Objective)
                .Map(dest => dest.Scope, src => src.Scope)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
