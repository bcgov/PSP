using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// Available financial code lookups.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum FinancialCodeTypes
    {
        [EnumMember(Value = "business-function")]
        BusinessFunction,

        [EnumMember(Value = "cost-types")]
        CostType,

        [EnumMember(Value = "work-activity")]
        WorkActivity,

        [EnumMember(Value = "chart-of-accounts")]
        ChartOfAccounts,

        [EnumMember(Value = "financial-activity")]
        FinancialActivity,

        [EnumMember(Value = "responsibility")]
        Responsibility,

        [EnumMember(Value = "yearly-financial")]
        YearlyFinancial,
    }
}
