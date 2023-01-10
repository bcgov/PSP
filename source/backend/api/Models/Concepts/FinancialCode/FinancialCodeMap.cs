using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class FinancialCodeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.IFinancialCodeEntity<string>, FinancialCodeModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.EffectiveDate, src => src.EffectiveDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<Entity.IFinancialCodeEntity<int>, FinancialCodeModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.EffectiveDate, src => src.EffectiveDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<Entity.IFinancialCodeEntity<short>, FinancialCodeModel>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.EffectiveDate, src => src.EffectiveDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<Entity.PimsBusinessFunctionCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.BusinessFunction)
                .Inherits<Entity.IFinancialCodeEntity<string>, FinancialCodeModel>();

            config.NewConfig<Entity.PimsCostTypeCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.CostTypes)
                .Inherits<Entity.IFinancialCodeEntity<string>, FinancialCodeModel>();

            config.NewConfig<Entity.PimsWorkActivityCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.WorkActivity)
                .Inherits<Entity.IFinancialCodeEntity<string>, FinancialCodeModel>();

            config.NewConfig<Entity.PimsChartOfAccountsCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.ChartOfAccounts)
                .Inherits<Entity.IFinancialCodeEntity<int>, FinancialCodeModel>();

            config.NewConfig<Entity.PimsFinancialActivityCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.FinancialActivity)
                .Inherits<Entity.IFinancialCodeEntity<short>, FinancialCodeModel>();

            config.NewConfig<Entity.PimsResponsibilityCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.Responsibility)
                .Inherits<Entity.IFinancialCodeEntity<int>, FinancialCodeModel>();

            config.NewConfig<Entity.PimsYearlyFinancialCode, FinancialCodeModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => FinancialCodeTypes.YearlyFinancial)
                .Inherits<Entity.IFinancialCodeEntity<string>, FinancialCodeModel>();
        }
    }
}
