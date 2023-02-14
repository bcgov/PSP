using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class FinancialCodeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.IFinancialCodeEntity, FinancialCodeModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.EffectiveDate, src => src.EffectiveDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<Entity.PimsBusinessFunctionCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.BusinessFunction)
                .Inherits<Entity.IFinancialCodeEntity, FinancialCodeModel>();

            config.NewConfig<Entity.PimsCostTypeCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.CostType)
                .Inherits<Entity.IFinancialCodeEntity, FinancialCodeModel>();

            config.NewConfig<Entity.PimsWorkActivityCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.WorkActivity)
                .Inherits<Entity.IFinancialCodeEntity, FinancialCodeModel>();

            config.NewConfig<Entity.PimsChartOfAccountsCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.ChartOfAccounts)
                .Inherits<Entity.IFinancialCodeEntity, FinancialCodeModel>();

            config.NewConfig<Entity.PimsFinancialActivityCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.FinancialActivity)
                .Inherits<Entity.IFinancialCodeEntity, FinancialCodeModel>();

            config.NewConfig<Entity.PimsResponsibilityCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.Responsibility)
                .Inherits<Entity.IFinancialCodeEntity, FinancialCodeModel>();

            config.NewConfig<Entity.PimsYearlyFinancialCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.YearlyFinancial)
                .Inherits<Entity.IFinancialCodeEntity, FinancialCodeModel>();

            config.NewConfig<FinancialCodeModel, Entity.IFinancialCodeEntity>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.EffectiveDate, src => src.EffectiveDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<FinancialCodeModel, Entity.PimsBusinessFunctionCode>()
                .Map(dest => dest.Id, src => src.Id)
                .Inherits<FinancialCodeModel, Entity.IFinancialCodeEntity>();

            config.NewConfig<FinancialCodeModel, Entity.PimsCostTypeCode>()
                .Map(dest => dest.Id, src => src.Id)
                .Inherits<FinancialCodeModel, Entity.IFinancialCodeEntity>();

            config.NewConfig<FinancialCodeModel, Entity.PimsWorkActivityCode>()
                .Map(dest => dest.Id, src => src.Id)
                .Inherits<FinancialCodeModel, Entity.IFinancialCodeEntity>();

            config.NewConfig<FinancialCodeModel, Entity.PimsChartOfAccountsCode>()
                .Map(dest => dest.Id, src => src.Id)
                .Inherits<FinancialCodeModel, Entity.IFinancialCodeEntity>();

            config.NewConfig<FinancialCodeModel, Entity.PimsFinancialActivityCode>()
                .Map(dest => dest.Id, src => src.Id)
                .Inherits<FinancialCodeModel, Entity.IFinancialCodeEntity>();

            config.NewConfig<FinancialCodeModel, Entity.PimsResponsibilityCode>()
                .Map(dest => dest.Id, src => src.Id)
                .Inherits<FinancialCodeModel, Entity.IFinancialCodeEntity>();

            config.NewConfig<FinancialCodeModel, Entity.PimsYearlyFinancialCode>()
                .Map(dest => dest.Id, src => src.Id)
                .Inherits<FinancialCodeModel, Entity.IFinancialCodeEntity>();
        }
    }
}
