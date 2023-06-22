using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsYearlyFinancialCode class, provides an entity for the datamodel to manage a list of yearly financial codes.
    /// </summary>
    public partial class PimsYearlyFinancialCode : IFinancialCodeEntity
    {
        [NotMapped]
        public bool IsDisabled => ExpiryDate.HasValue || System.DateTime.Now.Date < EffectiveDate.Date;
    }
}
