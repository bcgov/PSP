using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropMgmtActivityType class, provides an entity for the datamodel to manage types.
    /// </summary>
    public partial class PimsPropMgmtActivityType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => PropMgmtActivityTypeCode; set => PropMgmtActivityTypeCode = value; }
    }
}
