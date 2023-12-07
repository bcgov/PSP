using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.H120Category
{
    public class H120CategoryMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsH120Category, H120CategoryModel>()
                .Map(dest => dest.Id, src => src.H120CategoryId)
                .Map(dest => dest.FinancialActivityId, src => src.FinancialActivityId)
                .Map(dest => dest.CostTypeId, src => src.CostTypeId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.H120CategoryNo, src => src.H120CategoryNo)
                .Map(dest => dest.WorkActivityId, src => src.WorkActivityId)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<H120CategoryModel, Entity.PimsH120Category>()
                .Map(dest => dest.H120CategoryId, src => src.Id)
                .Map(dest => dest.FinancialActivityId, src => src.FinancialActivityId)
                .Map(dest => dest.CostTypeId, src => src.CostTypeId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.H120CategoryNo, src => src.H120CategoryNo)
                .Map(dest => dest.WorkActivityId, src => src.WorkActivityId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
