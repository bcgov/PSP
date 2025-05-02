using Mapster;
using Pims.Api.Models.Base;
using Pims.Core.Extensions;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Product
{
    public class ProductMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProduct, ProductModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ProjectProducts, src => src.PimsProjectProducts)
                .Map(dest => dest.AcquisitionFiles, src => src.PimsAcquisitionFiles)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.StartDate, src => src.StartDate.ToNullableDateOnly())
                .Map(dest => dest.CostEstimate, src => src.CostEstimate)
                .Map(dest => dest.CostEstimateDate, src => src.CostEstimateDate.ToNullableDateOnly())
                .Map(dest => dest.Objective, src => src.Objective)
                .Map(dest => dest.Scope, src => src.Scope)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<ProductModel, Entity.PimsProduct>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.PimsProjectProducts, src => src.ProjectProducts)
                .Map(dest => dest.PimsAcquisitionFiles, src => src.AcquisitionFiles)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.StartDate, src => src.StartDate.ToNullableDateTime())
                .Map(dest => dest.CostEstimate, src => src.CostEstimate)
                .Map(dest => dest.CostEstimateDate, src => src.CostEstimateDate.ToNullableDateTime())
                .Map(dest => dest.Objective, src => src.Objective)
                .Map(dest => dest.Scope, src => src.Scope)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();

            config.NewConfig<Entity.PimsProductHist, Entity.PimsProduct>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.CostEstimate, src => src.CostEstimate)
                .Map(dest => dest.CostEstimateDate, src => src.CostEstimateDate)
                .Map(dest => dest.Objective, src => src.Objective)
                .Map(dest => dest.Scope, src => src.Scope);
        }
    }
}
