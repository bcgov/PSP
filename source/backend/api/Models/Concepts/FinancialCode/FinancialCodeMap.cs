using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class FinancialCodeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.IFinancialCodeEntity<string>, FinancialCodeModel<string>>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.EffectiveDate, src => src.EffectiveDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate);

            config.NewConfig<Entity.IFinancialCodeEntity<int>, FinancialCodeModel<int>>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.EffectiveDate, src => src.EffectiveDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate);

            config.NewConfig<Entity.IFinancialCodeEntity<short>, FinancialCodeModel<short>>()
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.EffectiveDate, src => src.EffectiveDate)
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDate);


            config.NewConfig<Entity.PimsBusinessFunctionCode, FinancialCodeModel<string>>()
                .Map(dest => dest.Type, src => FinancialCodeTypes.BusinessFunction)
                .Inherits<Entity.IFinancialCodeEntity<string>, FinancialCodeModel<string>>();

            config.NewConfig<Entity.PimsCostTypeCode, FinancialCodeModel<string>>()
                .Map(dest => dest.Type, src => FinancialCodeTypes.CostTypes)
                .Inherits<Entity.IFinancialCodeEntity<string>, FinancialCodeModel<string>>();

            config.NewConfig<Entity.PimsWorkActivityCode, FinancialCodeModel<string>>()
                .Map(dest => dest.Type, src => FinancialCodeTypes.WorkActivity)
                .Inherits<Entity.IFinancialCodeEntity<string>, FinancialCodeModel<string>>();

            config.NewConfig<Entity.PimsChartOfAccountsCode, FinancialCodeModel<int>>()
                .Map(dest => dest.Type, src => FinancialCodeTypes.ChartOfAccounts)
                .Inherits<Entity.IFinancialCodeEntity<int>, FinancialCodeModel<int>>();

            config.NewConfig<Entity.PimsFinancialActivityCode, FinancialCodeModel<short>>()
                .Map(dest => dest.Type, src => FinancialCodeTypes.FinancialActivity)
                .Inherits<Entity.IFinancialCodeEntity<short>, FinancialCodeModel<short>>();

            config.NewConfig<Entity.PimsResponsibilityCode, FinancialCodeModel<int>>()
                .Map(dest => dest.Type, src => FinancialCodeTypes.Responsibility)
                .Inherits<Entity.IFinancialCodeEntity<int>, FinancialCodeModel<int>>();

            config.NewConfig<Entity.PimsYearlyFinancialCode, FinancialCodeModel<string>>()
                .Map(dest => dest.Type, src => FinancialCodeTypes.YearlyFinancial)
                .Inherits<Entity.IFinancialCodeEntity<string>, FinancialCodeModel<string>>();

        }
    }
}
