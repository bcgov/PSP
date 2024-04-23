using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Concepts.FinancialCode
{
    /// <summary>
    /// Available financial code lookups.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum FinancialCodeTypes
    {
        [EnumMember(Value = "BusinessFunction")]
        BusinessFunction,

        [EnumMember(Value = "CostType")]
        CostType,

        [EnumMember(Value = "WorkActivity")]
        WorkActivity,

        [EnumMember(Value = "ChartOfAccounts")]
        ChartOfAccounts,

        [EnumMember(Value = "FinancialActivity")]
        FinancialActivity,

        [EnumMember(Value = "Responsibility")]
        Responsibility,

        [EnumMember(Value = "YearlyFinancial")]
        YearlyFinancial,
    }
}
