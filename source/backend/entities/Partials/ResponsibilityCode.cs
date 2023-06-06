using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsResponsibilityCode class, provides an entity for the datamodel to manage a list of responsibility codes.
    /// </summary>
    public partial class PimsResponsibilityCode : IFinancialCodeEntity
    {
        [NotMapped]
        public bool IsDisabled => ExpiryDate.HasValue || System.DateTime.Now.Date < EffectiveDate.Date;
    }
}
