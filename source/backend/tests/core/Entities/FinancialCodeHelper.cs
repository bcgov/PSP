using System;
using Pims.Api.Models.Concepts;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a financial code.
        /// </summary>
        /// <param name="type">Financial code type.</param>
        /// <param name="id">Financial code's id.</param>
        /// <param name="code">Financial code value.</param>
        /// <param name="description">Financial code description.</param>
        /// <returns>A financial code entity.</returns>
        public static Entity.IFinancialCodeEntity CreateFinancialCode(FinancialCodeTypes type, long id, string code, string description = "")
        {
            return type switch
            {
                FinancialCodeTypes.BusinessFunction => new Entity.PimsBusinessFunctionCode() { Id = id, Code = code, Description = description, ConcurrencyControlNumber = 1 },
                FinancialCodeTypes.ChartOfAccounts => new Entity.PimsChartOfAccountsCode() { Id = id, Code = code, Description = description, ConcurrencyControlNumber = 1 },
                FinancialCodeTypes.YearlyFinancial => new Entity.PimsYearlyFinancialCode() { Id = id, Code = code, Description = description, ConcurrencyControlNumber = 1 },
                FinancialCodeTypes.CostType => new Entity.PimsCostTypeCode() { Id = id, Code = code, Description = description, ConcurrencyControlNumber = 1 },
                FinancialCodeTypes.FinancialActivity => new Entity.PimsFinancialActivityCode() { Id = id, Code = code, Description = description, ConcurrencyControlNumber = 1 },
                FinancialCodeTypes.WorkActivity => new Entity.PimsWorkActivityCode() { Id = id, Code = code, Description = description, ConcurrencyControlNumber = 1 },
                FinancialCodeTypes.Responsibility => new Entity.PimsResponsibilityCode() { Id = id, Code = code, Description = description, ConcurrencyControlNumber = 1 },
                _ => throw new InvalidOperationException("Financial code type must be a known type"),
            };
        }
    }
}
