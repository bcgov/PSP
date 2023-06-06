using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsChartOfAccountsCode class, provides an entity for the datamodel to manage a list of chart of account codes.
    /// </summary>
    public partial class PimsChartOfAccountsCode : IFinancialCodeEntity
    {
        [NotMapped]
        public bool IsDisabled => ExpiryDate.HasValue || System.DateTime.Now.Date < EffectiveDate.Date;
    }
}
