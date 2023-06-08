using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsFinancialActivityCode class, provides an entity for the datamodel to manage a list of financial activity codes.
    /// </summary>
    public partial class PimsFinancialActivityCode : IFinancialCodeEntity
    {
        [NotMapped]
        public bool IsDisabled => ExpiryDate.HasValue || System.DateTime.Now.Date < EffectiveDate.Date;
    }
}
